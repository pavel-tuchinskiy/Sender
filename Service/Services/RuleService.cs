using Domain.Interfaces.Services;
using Domain.Models.Rules;
using Domain.Models.Rules.RuleModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
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
            Log.Information("Deserialization rule file({file}) started.", rulesPath);
            var rules = new JsonParser().DeserializeFile<RulesRoot>(rulesPath).Rules;

            Log.Information("Deserialization completed. Rules from file({rulesPath}): \n {rules}", rulesPath, JsonConvert.SerializeObject(rules));
            return rules;
        }
    }
}
