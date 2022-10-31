using Domain.Models.Project;
using Domain.Models.Rules.RuleModels;

namespace Domain.Interfaces.Services
{
    public interface IProjectService
    {
        List<Project> FilterProjects(List<Project> projects, Rule rules);
    }
}
