using Domain.Interfaces.Services;
using Domain.Models.MessageTemplates;
using Domain.Models.Project;
using Domain.Models.Response;
using Domain.Models.Rules;
using Domain.Models.Rules.RuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ISenderService _senderService;
        private readonly List<Rule> _rules;
        private readonly Templates _templates;

        public ProjectsController(IProjectService projectService, ISenderService senderService,
            IOptionsSnapshot<RulesRoot> rules, IOptionsSnapshot<Templates> templates)
        {
            _projectService = projectService;
            _senderService = senderService;
            _rules = rules.Value.Rules;
            _templates = templates.Value;
        }

        [HttpPost]
        public async Task<HttpResponseResult<Project>> Post(ProjectsRoot projects)
        {
            foreach(var rule in _rules)
            {
                var filteredProjects = _projectService.FilterProjects(projects.Projects, rule);
                await _senderService.SendRangeAsync(filteredProjects, rule.Effects, _templates);
            } 

            return new HttpResponseResult<Project>(200);
        }

        [HttpPost("TelegramSpam")]
        public async Task<HttpResponseResult<Project>> TelegramSpam(string phone, int messageCount, string message)
        {
            await _senderService.TelegramSpamToUser(phone, messageCount, message);

            return new HttpResponseResult<Project>(200);
        }
    }
}
