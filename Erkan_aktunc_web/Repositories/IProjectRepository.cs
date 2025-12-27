using Erkan_aktunc_web.Models;

namespace Erkan_aktunc_web.Repositories
{
    public interface IProjectTaskRepository
    {
        Task<ProjectTask?> GetByIdAsync(int id);
        Task AddAsync(ProjectTask task);
        Task UpdateAsync(ProjectTask task);
        Task DeleteAsync(int id);
    }
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();

        Task<IEnumerable<Project>> GetAllByUserIdAsync(string userId);

        Task<Project?> GetByIdWithDetailsAsync(int id);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(int id);
    }
}