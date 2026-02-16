using EnterpriseProjectWorkforceManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProjectWorkforceManagementSystem.Controllers;

[Authorize(Roles = "Admin,HR")]
public class EmployeeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public EmployeeController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.OrderBy(u => u.FullName).ToListAsync();
        return View(users);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound();
        return View(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationUser model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user is null) return NotFound();

        user.FullName = model.FullName;
        user.Department = model.Department;
        user.DateJoined = model.DateJoined;

        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(Index));
    }
}
