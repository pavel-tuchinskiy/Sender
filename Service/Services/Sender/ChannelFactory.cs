using Domain.Interfaces.Services;
using Domain.Models.Configuration;
using Domain.Models.Rules.EffectModels;

namespace Service.Services.Sender
{
    public class ChannelFactory
    {
        public static IChannel GetChannelService(ChannelType type, ChannelsConfiguration configuration)
        {
            IChannel channel = null;

            if(type == ChannelType.SMTP)
            {
                channel = new SmtpChannelService(configuration);
            }
            else if(type == ChannelType.Telegram)
            {
                channel = new TelegramChannelService(configuration);
            }

            return channel;
        }
    }
}
