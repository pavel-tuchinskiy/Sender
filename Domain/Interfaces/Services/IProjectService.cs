using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IProjectService
    {
        Task SendProjectAsync(List<Project> projects);
    }
}
