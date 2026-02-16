using EnterpriseProjectWorkforceManagementSystem.Models;
using EnterpriseProjectWorkforceManagementSystem.Services;
using EnterpriseProjectWorkforceManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProjectWorkforceManagementSystem.Controllers;

[Authorize]
public class ProjectController : Controller
{
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectController(IProjectService projectService, UserManager<ApplicationUser> userManager)
    {
        _projectService = projectService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var projects = await _projectService.GetAllAsync();
        return View(projects);
    }

    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create()
    {
        return View(await BuildFormVmAsync(new ProjectFormViewModel()));
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<IActionResult> Create(ProjectFormViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(await BuildFormVmAsync(vm));

        var project = new Project
        {
            ProjectName = vm.ProjectName,
            ClientName = vm.ClientName,
            ProjectType = vm.ProjectType,
            Budget = vm.Budget,
            StartDate = vm.StartDate,
            Deadline = vm.Deadline,
            Status = vm.Status,
            ManagerId = vm.ManagerId
        };

        await _projectService.CreateAsync(project, vm.EngineerIds);
        return RedirectToAction(nameof(Index));
    }

    private async Task<ProjectFormViewModel> BuildFormVmAsync(ProjectFormViewModel vm)
    {
        var managers = await _userManager.GetUsersInRoleAsync("Manager");
        var engineers = await _userManager.GetUsersInRoleAsync("Engineer");
        vm.Managers = managers.Select(m => new SelectListItem(m.FullName, m.Id)).ToList();
        vm.Engineers = engineers.Select(e => new SelectListItem(e.FullName, e.Id)).ToList();
        return vm;
    }
}
