using Domain.Models.Rules.EffectModels;

namespace Domain.Attributes
{
    public class ChannelTypeAttribute : Attribute
    {
        public ChannelType ChannelType { get; }
        public ChannelTypeAttribute(ChannelType channelType) => ChannelType = channelType;
    }
}
