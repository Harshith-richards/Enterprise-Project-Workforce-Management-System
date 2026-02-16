using EnterpriseProjectWorkforceManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnterpriseProjectWorkforceManagementSystem.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IAttendanceService _attendanceService;

    public DashboardController(IProjectService projectService, IAttendanceService attendanceService)
    {
        _projectService = projectService;
        _attendanceService = attendanceService;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Admin()
    {
        var vm = await _projectService.GetAdminDashboardAsync();
        var attendance = await _attendanceService.GetHrDashboardAsync(DateTime.UtcNow.Year, DateTime.UtcNow.Month);
        vm.PresentCount = attendance.PresentCount;
        vm.LateCount = attendance.LateCount;
        vm.AbsentCount = attendance.AbsentCount;
        return View(vm);
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Manager()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var projects = await _projectService.GetManagerProjectsAsync(userId);
        return View(projects);
    }

    [Authorize(Roles = "Engineer")]
    public async Task<IActionResult> Engineer()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var projects = await _projectService.GetEngineerProjectsAsync(userId);
        return View(projects);
    }

    [Authorize(Roles = "HR")]
    public async Task<IActionResult> HR()
    {
        var vm = await _attendanceService.GetHrDashboardAsync(DateTime.UtcNow.Year, DateTime.UtcNow.Month);
        return View(vm);
    }

    [Authorize(Roles = "CEO")]
    public async Task<IActionResult> CEO()
    {
        var vm = await _projectService.GetAdminDashboardAsync();
        return View(vm);
    }
}
