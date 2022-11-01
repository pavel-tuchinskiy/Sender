using Domain.Interfaces.Services;
using Domain.Models.Project;
using Domain.Models.Rules.RuleModels;
using Newtonsoft.Json;
using Serilog;
using Service.Extensions;

namespace Service.Services
{
    public class ProjectService : IProjectService
    {
        public ProjectService() {}

        public List<Project> FilterProjects(List<Project> projects, Rule rule)
        {
            Log.Debug("Filtering projects started");
            var filteredProjects = projects.AsQueryable().Filter(rule).ToList();

            Log.Debug("Filtering projects complited. Result: {projects}", JsonConvert.SerializeObject(filteredProjects));
            return filteredProjects;
        }
    }
}
