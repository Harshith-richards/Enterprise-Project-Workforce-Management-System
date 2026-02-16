using EnterpriseProjectWorkforceManagementSystem.Models;
using EnterpriseProjectWorkforceManagementSystem.ViewModels;

namespace EnterpriseProjectWorkforceManagementSystem.Services;

public interface IAttendanceService
{
    Task MarkAttendanceAsync(string employeeId, TimeSpan checkIn, TimeSpan? checkOut);
    Task<List<Attendance>> GetEmployeeAttendanceAsync(string employeeId);
    Task<HrDashboardViewModel> GetHrDashboardAsync(int year, int month);
}
