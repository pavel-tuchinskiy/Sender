using Domain.Interfaces.Services;
using Domain.Models.Configuration;
using Domain.Models.Message;
using Domain.Models.MessageTemplates;
using Domain.Models.Response;
using Domain.Models.Rules.EffectModels;
using Microsoft.Extensions.Configuration;
using Service.Helpers;

namespace Service.Services
{
    public class SenderService : ISenderService
    {
        private readonly IConfiguration _configuration;
        private readonly ChannelsConfiguration _channelsConfiguration;

        public SenderService(IConfiguration configuration)
        {
            _configuration = configuration;
            var configString = configuration.GetSection(Constants.CHANNELS_CONFIG).Value;
            _channelsConfiguration = JsonParser.DeserializeFile<ChannelsConfiguration>(configString);
        }

        public async Task SendRangeAsync<T>(List<T> objects, List<Effect> effects)
        {
            var templatesPath = _configuration.GetSection(Constants.TEMPLATES_PATH).Value;
            var templates = JsonParser.DeserializeFile<Templates>(templatesPath);

            var smtpEffect = effects.FirstOrDefault(x => x.Type == ChannelType.SMTP);
            var telegramEffect = effects.FirstOrDefault(x => x.Type == ChannelType.Telegram);

            if(smtpEffect == null || telegramEffect == null)
            {
                throw new ResponseException("Can't find required effects");
            }

            var smtpPlaceholders = smtpEffect.Placeholders.Select(x => x.Key).ToList();
            var telegramPlaceholders = telegramEffect.Placeholders.Select(x => x.Key).ToList();

            var smtpSender = new SmtpSender(_channelsConfiguration.SmtpConfiguration);
            var telegramSender = new TelegramSender(_channelsConfiguration.TelegramConfiguration);

            foreach(var obj in objects)
            {
                var smtpTemplate = templates.SmtpTemplates.FirstOrDefault(x => x.Id == smtpEffect.TemplateId);
                var telegramTemplate = templates.TelegramTemplates.FirstOrDefault(x => x.Id == telegramEffect.TemplateId);

                if(smtpTemplate == null || telegramTemplate == null)
                {
                    throw new ResponseException("Can't find required templates");
                }

                var smtpMessage = MessageHelper.CreateMessage<SmtpMessage, T, SmtpTemplate>(smtpTemplate, obj, smtpPlaceholders);
                var telegramMessage = MessageHelper.CreateMessage<TelegramMessage, T, TelegramTemplate>(telegramTemplate, obj, telegramPlaceholders);

                await smtpSender.SendAsync(smtpMessage);
                await telegramSender.SendAsync(telegramMessage);
            }
        }

        public async Task TelegramSpamToUser(string phone, int messageCount, string message)
        {
            var telSender = new TelegramSender(new TelegramConfiguration
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
