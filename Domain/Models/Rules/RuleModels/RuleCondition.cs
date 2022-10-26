using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Models.Rules.RuleModels
{
    public class RuleCondition
    {
        public string Key { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Conditions Condition { get; set; }

        [JsonProperty("val")]
        public object Value { get; set; }
    }
}
