using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseProjectWorkforceManagementSystem.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    public DateTime DateJoined { get; set; } = DateTime.UtcNow;

    public ICollection<ProjectAssignment> ProjectAssignments { get; set; } = new List<ProjectAssignment>();
    public ICollection<Attendance> AttendanceRecords { get; set; } = new List<Attendance>();
}
