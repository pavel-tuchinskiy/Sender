using Domain.Interfaces.Services;
using Domain.Models.Configuration;
using Domain.Models.Message;
using Domain.Models.MessageTemplates;
using Domain.Models.Rules.EffectModels;
using Microsoft.Extensions.Configuration;
using Serilog;
using Service.Helpers;
using Service.Helpers.SenderStrategies;
using Service.Services.Sender;

namespace Service.Services
{
    public class SenderService : ISenderService
    {
        private readonly IConfiguration _configuration;

        public SenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendRangeAsync<T>(List<T> objects, List<Effect> effects)
        {
            Log.Information("Sending objects started.");
            var parser = new JsonParser();
            var configString = _configuration.GetSection(Constants.CHANNELS_CONFIG).Value;
            var channelsConfiguration = parser.DeserializeFile<ChannelsConfiguration>(configString);
            var templatesPath = _configuration.GetSection(Constants.TEMPLATES_PATH).Value;
            var templates = parser.DeserializeFile<Templates>(templatesPath);

            bool res = false;
            foreach(var effect in effects)
            {
                var channel = ChannelFactory.GetChannelService(effect.Type, channelsConfiguration);

                res = await channel.SendRangeAsync(objects, effect, templates);
            }

            Log.Information("Sending objects completed");
            return res;
        }

        public async Task TelegramSpamToUser(string phone, int messageCount, string message)
        {
            var parser = new JsonParser();
            var configString = _configuration.GetSection(Constants.CHANNELS_CONFIG).Value;
            var channelsConfiguration = parser.DeserializeFile<ChannelsConfiguration>(configString);

            var telSender = new TelegramSender(new TelegramConfiguration
            {
                Api_Id = channelsConfiguration.TelegramConfiguration.Api_Id,
                Api_Hash = channelsConfiguration.TelegramConfiguration.Api_Hash,
                Phone = channelsConfiguration.TelegramConfiguration.Phone,
                Recepient_Phone = phone
            });

            var telMsg = new TelegramMessage { Body = message };
            await telSender.SendManyAsync(telMsg, messageCount);
        }
    }
}
