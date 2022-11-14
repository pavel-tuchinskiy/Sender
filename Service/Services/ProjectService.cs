using Domain.Interfaces.Services;
using Domain.Models.Project;
using Domain.Models.Response;
using Domain.Models.Rules.RuleModels;
using Newtonsoft.Json;
using Serilog;
using Service.Extensions;

namespace Service.Services
{
    public class ProjectService : IProjectService
    {
        public List<Project> FilterProjects(List<Project> projects, Rule rule)
        {
            if(projects == null)
            {
                Log.Error("Projects is null");
                throw new ResponseException("Projects is null");
            }

            Log.Debug("Filtering projects started");
            var filteredProjects = projects.AsQueryable().Filter(rule).ToList();

            Log.Debug("Filtering projects complited. Result: {projects}", JsonConvert.SerializeObject(filteredProjects));
            return filteredProjects;
        }
    }
}
