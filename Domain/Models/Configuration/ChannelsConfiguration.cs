using Domain.Interfaces.Options;
using Newtonsoft.Json;

namespace Domain.Models.Configuration
{
    public class ChannelsConfiguration : ISenderOptions
    {
        [JsonProperty("SmtpConfiguration")]
        public SmtpConfiguration SmtpConfiguration { get; set; }
        [JsonProperty("TelegramConfiguration")]
        public TelegramConfiguration TelegramConfiguration { get; set; }
    }
}
