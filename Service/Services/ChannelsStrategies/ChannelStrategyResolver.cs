using Domain.Interfaces.Strategy;
using Domain.Models.Configuration;
using Domain.Models.Rules.EffectModels;
using System.Reflection;

namespace Service.Helpers.Services.ChannelsStrategies
{
    public class ChannelStrategyResolver
    {
        private readonly ChannelsConfiguration _channelsConfiguration;
        private Dictionary<ChannelType, IChannel> _channels;

        public ChannelStrategyResolver(ChannelsConfiguration channelsConfiguration)
        {
            _channelsConfiguration = channelsConfiguration;
            _channels = GetChannelsStrategies();
        }

        private Dictionary<ChannelType, IChannel> GetChannelsStrategies()
        {
            var senderTypes = Assembly.GetAssembly(typeof(ChannelStrategyResolver))
               .GetTypes()
               .Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(typeof(IChannel)));

            var sendersDic = new Dictionary<ChannelType, IChannel>();

            foreach (var senderType in senderTypes)
            {
                var instance = Activator.CreateInstance(senderType, _channelsConfiguration);
                var senderKey = (ChannelType)senderType.GetField("_channelType").GetValue(instance);
                sendersDic.Add(senderKey, (IChannel)instance);
            }

            return sendersDic;
        }

        public IChannel GetChannel(ChannelType channelType)
        {
            var channel = _channels[channelType];
            
            return channel;
        }
    }
}
