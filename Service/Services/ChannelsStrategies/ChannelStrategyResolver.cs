using Domain.Interfaces.Strategy;
using Domain.Models.Configuration;
using Domain.Models.Rules.EffectModels;

namespace Service.Helpers.Services.ChannelsStrategies
{
    public class ChannelStrategyResolver
    {
        private readonly ChannelsConfiguration _channelsConfiguration;

        public ChannelStrategyResolver(ChannelsConfiguration channelsConfiguration)
        {
            _channelsConfiguration = channelsConfiguration;
        }

        public IChannel GetChannel(ChannelType channelType)
        {
            IChannel channel = null;

            if(channelType == ChannelType.SMTP)
            {
                channel = new SmtpChannelStrategy(_channelsConfiguration.SmtpConfiguration);
            }
            else if(channelType == ChannelType.Telegram)
            {
                channel = new TelegramChannelStrategy(_channelsConfiguration.TelegramConfiguration);
            }
            
            return channel;
        }
    }
}
