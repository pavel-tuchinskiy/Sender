using Domain.Models.Rules.EffectModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Models.Rules.RuleModels
{
    public class Rule
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Operator Operator { get; set; }
        [JsonProperty("conditions")]
        public List<RuleCondition> Conditions { get; set; }
        public List<Effect> Effects { get; set; }
    }
}
