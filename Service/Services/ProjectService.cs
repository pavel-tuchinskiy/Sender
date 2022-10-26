using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.Rules;
using Microsoft.Extensions.Configuration;
using Service.Extensions;
using Service.Helpers;

namespace Service.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IConfiguration _configuration;
        private readonly ISenderService _senderService;

        public ProjectService(IConfiguration configuration, ISenderService senderService)
        {
            _configuration = configuration;
            _senderService = senderService;
        }

        public async Task SendProjectAsync(List<Project> projects)
        {
            var rulesPath = _configuration.GetSection(Constants.RULES_PATH).Value;
            var rules = JsonParser.DeserializeFile<RulesRoot>(rulesPath).Rules;

            foreach(var rule in rules)
            {
                var projectsToSend = projects.AsQueryable().Filter(rule).ToList();

                await _senderService.SendRangeAsync(projectsToSend, rule.Effects);
            }
        }
    }
}
