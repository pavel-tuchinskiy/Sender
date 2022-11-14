using Core.Helpers;
using Domain.Interfaces.Strategy;
using Domain.Models.Configuration;
using Domain.Models.Rules.EffectModels;
using System.Reflection;

namespace Service.Helpers.Services.ChannelsStrategies
{
    public class ChannelStrategyResolver
    {
        private Dictionary<ChannelType, IChannel> _channels;

        public ChannelStrategyResolver(ChannelsConfiguration channelsConfiguration)
        {
            _channels = StrategiesHelper.GetStrategies<ChannelType, IChannel>(typeof(ChannelStrategyResolver), channelsConfiguration);
        }

        public IChannel GetChannel(ChannelType channelType)
        {
            var channel = _channels[channelType];
            
            return channel;
        }
    }
}
