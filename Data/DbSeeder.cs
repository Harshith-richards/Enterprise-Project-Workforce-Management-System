using EnterpriseProjectWorkforceManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseProjectWorkforceManagementSystem.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        string[] roles = ["Admin", "Manager", "Engineer", "HR", "CEO"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var users = new[]
        {
            new { Email = "admin@hvaccorp.com", FullName = "System Admin", Role = "Admin", Department="IT" },
            new { Email = "manager@hvaccorp.com", FullName = "Project Manager", Role = "Manager", Department="Projects" },
            new { Email = "engineer@hvaccorp.com", FullName = "Lead Engineer", Role = "Engineer", Department="Engineering" },
            new { Email = "hr@hvaccorp.com", FullName = "HR Specialist", Role = "HR", Department="HR" },
            new { Email = "ceo@hvaccorp.com", FullName = "Chief Executive Officer", Role = "CEO", Department="Executive" }
        };

        foreach (var seed in users)
        {
            var user = await userManager.FindByEmailAsync(seed.Email);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = seed.Email,
                    Email = seed.Email,
                    FullName = seed.FullName,
                    Department = seed.Department,
                    DateJoined = DateTime.UtcNow.AddMonths(-6)
                };

                await userManager.CreateAsync(user, "P@ssw0rd123!");
                await userManager.AddToRoleAsync(user, seed.Role);
            }
        }

        if (!await context.Projects.AnyAsync())
        {
            var manager = await userManager.FindByEmailAsync("manager@hvaccorp.com");
            var engineer = await userManager.FindByEmailAsync("engineer@hvaccorp.com");

            if (manager is not null)
            {
                var project1 = new Project
                {
                    ProjectName = "Metro Tower HVAC Retrofit",
                    ClientName = "Metro Holdings",
                    ProjectType = ProjectType.HVAC,
                    Budget = 1200000,
                    StartDate = DateTime.UtcNow.Date.AddDays(-20),
                    Deadline = DateTime.UtcNow.Date.AddDays(45),
                    Status = ProjectStatus.Ongoing,
                    ManagerId = manager.Id
                };

                var project2 = new Project
                {
                    ProjectName = "Airport MEP Expansion",
                    ClientName = "City Aviation Authority",
                    ProjectType = ProjectType.MEP,
                    Budget = 2500000,
                    StartDate = DateTime.UtcNow.Date.AddDays(-60),
                    Deadline = DateTime.UtcNow.Date.AddDays(-5),
                    Status = ProjectStatus.Ongoing,
                    ManagerId = manager.Id
                };

                context.Projects.AddRange(project1, project2);
                await context.SaveChangesAsync();

                if (engineer is not null)
                {
                    context.ProjectAssignments.Add(new ProjectAssignment { ProjectId = project1.Id, EngineerId = engineer.Id });
                    context.ProjectAssignments.Add(new ProjectAssignment { ProjectId = project2.Id, EngineerId = engineer.Id });
                }

                context.Milestones.AddRange(
                    new Milestone { ProjectId = project1.Id, Title = "Ducting Installation", DueDate = DateTime.UtcNow.Date.AddDays(20), IsCompleted = false },
                    new Milestone { ProjectId = project2.Id, Title = "Electrical Testing", DueDate = DateTime.UtcNow.Date.AddDays(7), IsCompleted = false }
                );

                if (engineer is not null)
                {
                    context.Attendances.Add(new Attendance
                    {
                        EmployeeId = engineer.Id,
                        Date = DateTime.UtcNow.Date,
                        CheckInTime = new TimeSpan(8, 50, 0),
                        CheckOutTime = new TimeSpan(17, 15, 0),
                        Status = AttendanceStatus.Present
                    });
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
