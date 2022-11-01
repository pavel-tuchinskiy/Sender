using Domain.Interfaces.Helpers;
using Domain.Interfaces.Services;
using Domain.Models.Configuration;
using Domain.Models.Message;
using Domain.Models.MessageTemplates;
using Domain.Models.Response;
using Domain.Models.Rules.EffectModels;
using Serilog;
using Service.Helpers;
using Service.Helpers.SenderStrategies;

namespace Service.Services.Sender
{
    public class TelegramChannelService : IChannel
    {
        private readonly ISender _senderChannel;

        public TelegramChannelService(ChannelsConfiguration configuration)
        {
            var senderResolver = new SenderStrategyResolver(configuration);
            _senderChannel = senderResolver.GetSender(ChannelType.Telegram);
        }
        public async Task<bool> SendRangeAsync<T>(List<T> objects, Effect effect, Templates templates)
        {
            if (effect == null)
            {
                Log.Error($"Effect is null");
                throw new ResponseException("Can't find required effects");
            }

            Log.Debug("Sending via telegram started.");

            var placeholders = effect.Placeholders.Select(x => x.Key).ToList();
            bool res = false;

            foreach (var obj in objects)
            {
                var template = templates.TelegramTemplates.FirstOrDefault(x => x.Id == effect.TemplateId);

                if (template == null)
                {
                    Log.Error("Can't find required email template: {id}", effect.TemplateId);
                    throw new ResponseException("Can't find required templates");
                }

                var message = MessageHelper.CreateMessage<TelegramMessage, T, TelegramTemplate>(template, obj, placeholders);
                res = await _senderChannel.SendAsync(message);

                if(res == false)
                {
                    Log.Error("Can't send telegram message: {message}", message.Body);
                    throw new ResponseException("Something went wrong while sending telegram message");
                }
            }

            Log.Debug("Sending via telegram completed.");
            return res;
        }
    }
}
