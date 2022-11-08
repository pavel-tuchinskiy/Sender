using Domain.Interfaces.Strategy;
using Domain.Models.Message;
using Domain.Models.MessageTemplates;

namespace Service.Helpers.Services.MessagesStrategies
{
    public class TelegramMessageStrategy : IMessage
    {
        private readonly TelegramTemplate _template;
        private readonly List<string> _placeholders;

        public TelegramMessageStrategy(TelegramTemplate template, List<string> placeholders)
        {
            _template = template;
            _placeholders = placeholders;
        }

        public List<Message> CreateMessages<T>(List<T> items)
        {
            var messages = new List<Message>();

            foreach (var item in items)
            {
                messages.Add(MessageBuilder.CreateMessage<T, TelegramTemplate, TelegramMessage>(item, _template, _placeholders));
            }

            return messages;
        }
    }
}
