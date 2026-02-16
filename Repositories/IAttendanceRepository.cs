using EnterpriseProjectWorkforceManagementSystem.Models;

namespace EnterpriseProjectWorkforceManagementSystem.Repositories;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<List<Attendance>> GetMonthlyReportAsync(int year, int month);
    Task<Dictionary<string, int>> GetLateCountByEmployeeAsync(int year, int month);
    Task<List<Attendance>> GetByEmployeeAsync(string employeeId);
}
