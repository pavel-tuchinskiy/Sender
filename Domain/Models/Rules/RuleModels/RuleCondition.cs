using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Models.Rules.RuleModels
{
    public class RuleCondition
    {
        public string Key { get; set; }

        public Conditions Condition { get; set; }

        public object Val { get; set; }
    }
}
