using Domain.Interfaces.Strategy;
using Domain.Models.Message;
using Domain.Models.MessageTemplates;

namespace Service.Helpers.Services.MessagesStrategies
{
    public class SmtpMessageStrategy : IMessage
    {
        private readonly SmtpTemplate _template;
        private readonly List<string> _placeholders;

        public SmtpMessageStrategy(SmtpTemplate template, List<string> placeholders)
        {
            _template = template;
            _placeholders = placeholders;
        }

        public List<Message> CreateMessages<T>(List<T> items)
        {
            var messages = new List<Message>();

            foreach (var item in items)
            {
                messages.Add(MessageBuilder.CreateMessage<T, SmtpTemplate, SmtpMessage>(item, _template, _placeholders));
            }

            return messages;
        }
    }
}
