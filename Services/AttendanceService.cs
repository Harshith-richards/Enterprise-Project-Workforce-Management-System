using EnterpriseProjectWorkforceManagementSystem.Models;
using EnterpriseProjectWorkforceManagementSystem.Repositories;
using EnterpriseProjectWorkforceManagementSystem.ViewModels;

namespace EnterpriseProjectWorkforceManagementSystem.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;

    public AttendanceService(IAttendanceRepository attendanceRepository)
    {
        _attendanceRepository = attendanceRepository;
    }

    public async Task MarkAttendanceAsync(string employeeId, TimeSpan checkIn, TimeSpan? checkOut)
    {
        var today = DateTime.UtcNow.Date;
        var status = checkIn > new TimeSpan(9, 0, 0) ? AttendanceStatus.Late : AttendanceStatus.Present;

        var existing = (await _attendanceRepository.GetAllAsync(a => a.EmployeeId == employeeId && a.Date == today)).FirstOrDefault();
        if (existing is null)
        {
            await _attendanceRepository.AddAsync(new Attendance
            {
                EmployeeId = employeeId,
                Date = today,
                CheckInTime = checkIn,
                CheckOutTime = checkOut,
                Status = status
            });
        }
        else
        {
            existing.CheckOutTime = checkOut;
            await _attendanceRepository.UpdateAsync(existing);
        }

        await _attendanceRepository.SaveChangesAsync();
    }

    public async Task<List<Attendance>> GetEmployeeAttendanceAsync(string employeeId) => await _attendanceRepository.GetByEmployeeAsync(employeeId);

    public async Task<HrDashboardViewModel> GetHrDashboardAsync(int year, int month)
    {
        var report = await _attendanceRepository.GetMonthlyReportAsync(year, month);
        var late = await _attendanceRepository.GetLateCountByEmployeeAsync(year, month);

        return new HrDashboardViewModel
        {
            Year = year,
            Month = month,
            TotalRecords = report.Count,
            PresentCount = report.Count(x => x.Status == AttendanceStatus.Present),
            LateCount = report.Count(x => x.Status == AttendanceStatus.Late),
            AbsentCount = report.Count(x => x.Status == AttendanceStatus.Absent),
            LateByEmployee = late,
            MonthlyRecords = report
        };
    }
}
