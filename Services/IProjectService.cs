using EnterpriseProjectWorkforceManagementSystem.Models;
using EnterpriseProjectWorkforceManagementSystem.ViewModels;

namespace EnterpriseProjectWorkforceManagementSystem.Services;

public interface IProjectService
{
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task CreateAsync(Project project, List<string> engineerIds);
    Task UpdateAsync(Project project, List<string> engineerIds);
    Task<List<Project>> GetManagerProjectsAsync(string managerId);
    Task<List<Project>> GetEngineerProjectsAsync(string engineerId);
    Task<AdminDashboardViewModel> GetAdminDashboardAsync();
}
