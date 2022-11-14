using Domain.Models.Project;
using Domain.Models.Rules.RuleModels;
using LanguageExt.Common;

namespace Domain.Interfaces.Services
{
    public interface IProjectService
    {
        Result<List<Project>> FilterProjects(List<Project> projects, Rule rules);
    }
}
