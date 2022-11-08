using Domain.Interfaces.Services;
using Domain.Models.Configuration;
using Domain.Models.Message;
using Domain.Models.MessageTemplates;
using Domain.Models.Response;
using Domain.Models.Rules.EffectModels;
using Microsoft.Extensions.Configuration;
using Serilog;
using Service.Helpers;
using Service.Helpers.Services.ChannelsStrategies;
using Service.Helpers.Services.MessagesStrategies;

namespace Service.Services
{
    public class SenderService : ISenderService
    {
        private ChannelsConfiguration _channelsConfiguration;
        private Templates _templates;
        private MessageStrategyResolver _messageResolver;
        private ChannelStrategyResolver _channelResolver;

        public SenderService(IConfiguration configuration)
        {
            var parser = new JsonParser();
            var configString = configuration.GetSection(Constants.CHANNELS_CONFIG).Value;
            _channelsConfiguration = parser.DeserializeFile<ChannelsConfiguration>(configString);
            var templatesPath = configuration.GetSection(Constants.TEMPLATES_PATH).Value;
            _templates = parser.DeserializeFile<Templates>(templatesPath);
            _messageResolver = new MessageStrategyResolver();
            _channelResolver = new ChannelStrategyResolver(_channelsConfiguration);
        }

        public async Task<bool> SendRangeAsync<T>(List<T> objects, List<Effect> effects)
        {
            if (effects == null)
            {
                Log.Error("Effects is null");
                throw new ResponseException("Effects is null");
            }

            Log.Debug("Sending objects started.");

            bool res = false;
            foreach(var effect in effects)
            {
                var messageFactory = _messageResolver.GetMessageFactory(effect, _templates);
                var messages = messageFactory.CreateMessages(objects);

                var channel = _channelResolver.GetChannel(effect.Type);

                foreach(var message in messages)
                {
                    res = await channel.SendAsync(message);
                }
            }

            Log.Debug("Sending objects completed with result: {res}", res);
            return res;
        }

        public async Task TelegramSpamToUser(string phone, int messageCount, string message)
        {
            var telSender = new TelegramChannelStrategy(new TelegramConfiguration
            {
                Api_Id = _channelsConfiguration.TelegramConfiguration.Api_Id,
                Api_Hash = _channelsConfiguration.TelegramConfiguration.Api_Hash,
                Phone = _channelsConfiguration.TelegramConfiguration.Phone,
                Recepient_Phone = phone
            });

            var telMsg = new TelegramMessage { Body = message };
            await telSender.SendManyAsync(telMsg, messageCount);
        }
    }
}
