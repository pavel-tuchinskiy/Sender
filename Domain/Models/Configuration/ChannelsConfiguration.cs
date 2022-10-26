using Newtonsoft.Json;

namespace Domain.Models.Configuration
{
    public class ChannelsConfiguration
    {
        [JsonProperty("SmtpConfiguration")]
        public SmtpConfiguration SmtpConfiguration { get; set; }
        [JsonProperty("TelegramConfiguration")]
        public TelegramConfiguration TelegramConfiguration { get; set; }
    }
}
