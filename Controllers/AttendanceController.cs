using EnterpriseProjectWorkforceManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnterpriseProjectWorkforceManagementSystem.Controllers;

[Authorize]
public class AttendanceController : Controller
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [Authorize(Roles = "Engineer,Admin")]
    public IActionResult Mark() => View();

    [Authorize(Roles = "Engineer,Admin")]
    [HttpPost]
    public async Task<IActionResult> Mark(TimeSpan checkInTime, TimeSpan? checkOutTime)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _attendanceService.MarkAttendanceAsync(userId, checkInTime, checkOutTime);
        TempData["Success"] = "Attendance recorded.";
        return RedirectToAction(nameof(MyRecords));
    }

    [Authorize(Roles = "Engineer,Admin")]
    public async Task<IActionResult> MyRecords()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var records = await _attendanceService.GetEmployeeAttendanceAsync(userId);
        return View(records);
    }

    [Authorize(Roles = "HR,Admin")]
    public async Task<IActionResult> Report(int? year, int? month)
    {
        var now = DateTime.UtcNow;
        var vm = await _attendanceService.GetHrDashboardAsync(year ?? now.Year, month ?? now.Month);
        return View(vm);
    }
}
