using EnterpriseProjectWorkforceManagementSystem.Data;
using EnterpriseProjectWorkforceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProjectWorkforceManagementSystem.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<Project>> GetAllWithDetailsAsync() =>
        await _context.Projects
            .Include(p => p.Manager)
            .Include(p => p.Assignments).ThenInclude(a => a.Engineer)
            .Include(p => p.Milestones)
            .ToListAsync();

    public async Task<Project?> GetDetailsByIdAsync(int id) =>
        await _context.Projects
            .Include(p => p.Manager)
            .Include(p => p.Assignments).ThenInclude(a => a.Engineer)
            .Include(p => p.Milestones)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<List<Project>> GetManagerProjectsAsync(string managerId) =>
        await _context.Projects
            .Include(p => p.Assignments)
            .Where(p => p.ManagerId == managerId)
            .ToListAsync();

    public async Task<List<Project>> GetEngineerProjectsAsync(string engineerId) =>
        await _context.Projects
            .Include(p => p.Milestones)
            .Where(p => p.Assignments.Any(a => a.EngineerId == engineerId))
            .ToListAsync();

    public async Task AutoUpdateDelayedProjectsAsync()
    {
        var now = DateTime.UtcNow.Date;
        var delayed = await _context.Projects
            .Where(p => p.Deadline < now && p.Status != ProjectStatus.Completed)
            .ToListAsync();

        foreach (var project in delayed)
        {
            project.Status = ProjectStatus.Delayed;
        }

        if (delayed.Count > 0)
            await _context.SaveChangesAsync();
    }
}
