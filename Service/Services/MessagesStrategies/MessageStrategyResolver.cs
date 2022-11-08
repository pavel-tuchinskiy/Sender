using Domain.Interfaces.Strategy;
using Domain.Models.MessageTemplates;
using Domain.Models.Rules.EffectModels;
using System.Linq.Dynamic.Core;

namespace Service.Helpers.Services.MessagesStrategies
{
    public class MessageStrategyResolver
    {
        public IMessage GetMessageFactory(Effect effect, Templates templates)
        {
            var placeholders = effect.Placeholders.Select(x => x.Key).ToList();

            IMessage messageFactory = null;

            if (effect.Type == ChannelType.SMTP)
            {
                var template = templates.SmtpTemplates.FirstOrDefault(x => x.Id == effect.TemplateId);
                messageFactory = new SmtpMessageStrategy(template, placeholders);
            }
            else if (effect.Type == ChannelType.Telegram)
            {
                var template = templates.TelegramTemplates.FirstOrDefault(x => x.Id == effect.TemplateId);
                messageFactory = new TelegramMessageStrategy(template, placeholders);
            }

            return messageFactory;
        }
    }
}
