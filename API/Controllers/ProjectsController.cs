using Domain.Interfaces.Services;
using Domain.Models.Project;
using Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IRuleService _ruleService;
        private readonly ISenderService _senderService;

        public ProjectsController(IProjectService projectService, IRuleService ruleService, ISenderService senderService)
        {
            _projectService = projectService;
            _ruleService = ruleService;
            _senderService = senderService;
        }

        [HttpPost]
        public async Task<HttpResponseResult> Post(ProjectsRoot projects)
        {
            var rules = _ruleService.GetRules();

            foreach(var rule in rules)
            {
                var filteredProjects = _projectService.FilterProjects(projects.Projects, rule);
                await _senderService.SendRangeAsync(filteredProjects, rule.Effects);
            } 

            return new HttpResponseResult(200);
        }

        [HttpPost("TelegramSpam")]
        public async Task<HttpResponseResult> TelegramSpam(string phone, int messageCount, string message)
        {
            await _senderService.TelegramSpamToUser(phone, messageCount, message);

            return new HttpResponseResult(200);
        }
    }
}
