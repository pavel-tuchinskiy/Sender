using Domain.Interfaces.Services;
using Domain.Models.Rules;
using Domain.Models.Rules.RuleModels;
using Microsoft.Extensions.Configuration;
using Service.Helpers;

namespace Service.Services
{
    public class RuleService : IRuleService
    {
        private readonly IConfiguration _configuration;

        public RuleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Rule> GetRules()
        {
            var rulesPath = _configuration.GetSection(Constants.RULES_PATH).Value;
            var rules = JsonParser.DeserializeFile<RulesRoot>(rulesPath).Rules;
            return rules;
        }
    }
}
