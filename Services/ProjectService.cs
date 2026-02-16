using EnterpriseProjectWorkforceManagementSystem.Data;
using EnterpriseProjectWorkforceManagementSystem.Models;
using EnterpriseProjectWorkforceManagementSystem.Repositories;
using EnterpriseProjectWorkforceManagementSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProjectWorkforceManagementSystem.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ApplicationDbContext _context;

    public ProjectService(IProjectRepository projectRepository, ApplicationDbContext context)
    {
        _projectRepository = projectRepository;
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        await _projectRepository.AutoUpdateDelayedProjectsAsync();
        return await _projectRepository.GetAllWithDetailsAsync();
    }

    public async Task<Project?> GetByIdAsync(int id) => await _projectRepository.GetDetailsByIdAsync(id);

    public async Task CreateAsync(Project project, List<string> engineerIds)
    {
        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();

        foreach (var engineerId in engineerIds.Distinct())
        {
            _context.ProjectAssignments.Add(new ProjectAssignment { ProjectId = project.Id, EngineerId = engineerId });
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Project project, List<string> engineerIds)
    {
        await _projectRepository.UpdateAsync(project);

        var existing = _context.ProjectAssignments.Where(x => x.ProjectId == project.Id);
        _context.ProjectAssignments.RemoveRange(existing);

        foreach (var engineerId in engineerIds.Distinct())
        {
            _context.ProjectAssignments.Add(new ProjectAssignment { ProjectId = project.Id, EngineerId = engineerId });
        }

        await _projectRepository.SaveChangesAsync();
    }

    public async Task<List<Project>> GetManagerProjectsAsync(string managerId) => await _projectRepository.GetManagerProjectsAsync(managerId);

    public async Task<List<Project>> GetEngineerProjectsAsync(string engineerId) => await _projectRepository.GetEngineerProjectsAsync(engineerId);

    public async Task<AdminDashboardViewModel> GetAdminDashboardAsync()
    {
        await _projectRepository.AutoUpdateDelayedProjectsAsync();
        var now = DateTime.UtcNow.Date;
        var projects = await _context.Projects.ToListAsync();
        var employees = await _context.Users.CountAsync();

        return new AdminDashboardViewModel
        {
            TotalEmployees = employees,
            TotalProjects = projects.Count,
            ActiveProjects = projects.Count(p => p.Status == ProjectStatus.Ongoing),
            DelayedProjects = projects.Count(p => p.Status == ProjectStatus.Delayed),
            ProjectsNearDeadline = projects.Count(p => p.Deadline <= now.AddDays(10) && p.Status != ProjectStatus.Completed),
            ProjectsByStatus = projects
                .GroupBy(p => p.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count())
        };
    }
}
