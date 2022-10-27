using Domain.Models.Rules.RuleModels;

namespace Domain.Interfaces.Services
{
    public interface IRuleService
    {
        List<Rule> GetRules();
    }
}
