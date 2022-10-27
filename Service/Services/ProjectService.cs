using Domain.Interfaces.Services;
using Domain.Models.Project;
using Domain.Models.Rules.RuleModels;
using Service.Extensions;

namespace Service.Services
{
    public class ProjectService : IProjectService
    {
        public ProjectService() {}

        public async Task<List<Project>> FilterProjectsAsync(List<Project> projects, Rule rule)
        {
            var filteredProjects = await Task.Run(() => projects.AsQueryable().Filter(rule).ToList());

            return filteredProjects;
        }
    }
}
