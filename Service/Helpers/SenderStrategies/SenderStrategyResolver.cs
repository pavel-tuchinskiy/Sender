using Domain.Interfaces.Helpers;
using Domain.Models.Configuration;
using Domain.Models.Rules.EffectModels;

namespace Service.Helpers.SenderStrategies
{
    public class SenderStrategyResolver
    {
        private readonly ChannelsConfiguration _channelsConfiguration;

        public SenderStrategyResolver(ChannelsConfiguration channelsConfiguration)
        {
            _channelsConfiguration = channelsConfiguration;
        }

        public ISender GetSender(ChannelType channelType)
        {
            if(channelType == ChannelType.SMTP)
            {
                return new SmtpSender(_channelsConfiguration.SmtpConfiguration);
            }
            else if(channelType == ChannelType.Telegram)
            {
                return new TelegramSender(_channelsConfiguration.TelegramConfiguration);
            }
            else
            {
                return null;
            }
        }
    }
}
