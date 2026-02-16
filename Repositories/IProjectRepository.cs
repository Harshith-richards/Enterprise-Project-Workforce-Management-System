using EnterpriseProjectWorkforceManagementSystem.Models;

namespace EnterpriseProjectWorkforceManagementSystem.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<List<Project>> GetAllWithDetailsAsync();
    Task<Project?> GetDetailsByIdAsync(int id);
    Task<List<Project>> GetManagerProjectsAsync(string managerId);
    Task<List<Project>> GetEngineerProjectsAsync(string engineerId);
    Task AutoUpdateDelayedProjectsAsync();
}
