using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<HttpResponseResult> Post(List<Project> project)
        {
            await _projectService.SendProjectAsync(project);

            return new HttpResponseResult(200);
        }
    }
}
