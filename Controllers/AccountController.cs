using EnterpriseProjectWorkforceManagementSystem.Models;
using EnterpriseProjectWorkforceManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProjectWorkforceManagementSystem.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View(model);
        }

        if (await _userManager.IsInRoleAsync(user, "Admin")) return RedirectToAction("Admin", "Dashboard");
        if (await _userManager.IsInRoleAsync(user, "Manager")) return RedirectToAction("Manager", "Dashboard");
        if (await _userManager.IsInRoleAsync(user, "Engineer")) return RedirectToAction("Engineer", "Dashboard");
        if (await _userManager.IsInRoleAsync(user, "HR")) return RedirectToAction("HR", "Dashboard");
        if (await _userManager.IsInRoleAsync(user, "CEO")) return RedirectToAction("CEO", "Dashboard");

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}
