using Domain.Interfaces.Strategy;
using Domain.Models.Message;
using Domain.Models.MessageTemplates;
using Domain.Models.Rules.EffectModels;

namespace Service.Helpers.Services.MessagesStrategies
{
    public class TelegramMessageStrategy : IMessage
    {
        private readonly TemplateBase _template;
        private readonly List<string> _placeholders;
        public readonly ChannelType _channelType = ChannelType.Telegram;

        public TelegramMessageStrategy(TemplateBase template, List<string> placeholders)
        {
            _template = template;
            _placeholders = placeholders;
        }

        public List<Message> CreateMessages<T>(List<T> items)
        {
            var messages = new List<Message>();

            foreach (var item in items)
            {
                messages.Add(MessageBuilder.CreateMessage<T, TelegramTemplate, TelegramMessage>(item, (TelegramTemplate)_template, _placeholders));
            }

            return messages;
        }
    }
}
