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
    public class SmtpChannelService : IChannel
    {
        private readonly ISender _senderChannel;
        public SmtpChannelService(ChannelsConfiguration configuration)
        {
            var senderResolver = new SenderStrategyResolver(configuration);
            _senderChannel = senderResolver.GetSender(ChannelType.SMTP);
        }

        public async Task<bool> SendRangeAsync<T>(List<T> objects, Effect effect, Templates templates)
        {
            if (effect == null)
            {
                Log.Error($"Effect is null");
                throw new ResponseException("Can't find required effect");
            }

            Log.Debug("Sending via smtp started.");

            var placeholders = effect.Placeholders.Select(x => x.Key).ToList();
            bool res = false;

            foreach (var obj in objects)
            {
                var template = templates.SmtpTemplates.FirstOrDefault(x => x.Id == effect.TemplateId);

                if (template == null)
                {
                    Log.Error("Can't find required email template: {id}", effect.TemplateId);
                    throw new ResponseException("Can't find required template");
                }

                var message = MessageHelper.CreateMessage<SmtpMessage, T, SmtpTemplate>(template, obj, placeholders);

                res = await _senderChannel.SendAsync(message);

                if (res == false)
                {
                    Log.Error("Can't send email message: {message}", message);
                    throw new ResponseException("Something went wrong while sending email message");
                }
            }

            Log.Debug("Sending via smtp completed.");
            return res;
        }
    }
}
