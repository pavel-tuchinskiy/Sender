using Domain.Models.Project;
using Domain.Models.Rules.RuleModels;

namespace Domain.Interfaces.Services
{
    public interface IProjectService
    {
        Task<List<Project>> FilterProjectsAsync(List<Project> projects, Rule rules);
    }
}
