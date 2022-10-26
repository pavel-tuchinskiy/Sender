using System.Runtime.Serialization;

namespace Domain.Models.Rules.EffectModels
{
    public enum ChannelType
    {
        [EnumMember(Value = "smtp")]
        SMTP,
        [EnumMember(Value = "telegram")]
        Telegram
    }
}
