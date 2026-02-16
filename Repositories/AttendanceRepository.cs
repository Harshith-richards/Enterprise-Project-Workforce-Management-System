using EnterpriseProjectWorkforceManagementSystem.Data;
using EnterpriseProjectWorkforceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProjectWorkforceManagementSystem.Repositories;

public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<Attendance>> GetMonthlyReportAsync(int year, int month) =>
        await _context.Attendances
            .Include(a => a.Employee)
            .Where(a => a.Date.Year == year && a.Date.Month == month)
            .OrderBy(a => a.Employee.FullName)
            .ThenBy(a => a.Date)
            .ToListAsync();

    public async Task<Dictionary<string, int>> GetLateCountByEmployeeAsync(int year, int month) =>
        await _context.Attendances
            .Include(a => a.Employee)
            .Where(a => a.Date.Year == year && a.Date.Month == month && a.Status == AttendanceStatus.Late)
            .GroupBy(a => a.Employee.FullName)
            .Select(g => new { EmployeeName = g.Key, LateCount = g.Count() })
            .ToDictionaryAsync(x => x.EmployeeName, x => x.LateCount);

    public async Task<List<Attendance>> GetByEmployeeAsync(string employeeId) =>
        await _context.Attendances
            .Where(a => a.EmployeeId == employeeId)
            .OrderByDescending(a => a.Date)
            .ToListAsync();
}
