using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace Domain.Models.Rules.EffectModels
{
    public class Effect
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ChannelType Type { get; set; }
        [JsonProperty("template_id")]
        public int TemplateId { get; set; }
        public ExpandoObject Placeholders { get; set; }
    }
}
