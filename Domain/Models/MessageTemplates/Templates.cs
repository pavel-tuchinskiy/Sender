using Domain.Attributes;
using Domain.Interfaces.Options;
using Domain.Models.Rules.EffectModels;

namespace Domain.Models.MessageTemplates
{
    public class Templates : ISenderOptions
    {
        [ChannelType(ChannelType.SMTP)]
        public List<SmtpTemplate> SmtpTemplates { get; set; }

        [ChannelType(ChannelType.Telegram)]
        public List<TelegramTemplate> TelegramTemplates { get; set; }
    }
}
