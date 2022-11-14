using Domain.Interfaces.Options;
using Domain.Models.Rules.RuleModels;

namespace Domain.Models.Rules
{
    public class RulesRoot : ISenderOptions
    {
        public List<Rule> Rules { get; set; }
    }
}
