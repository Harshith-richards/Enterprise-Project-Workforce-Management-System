using System.ComponentModel.DataAnnotations;

namespace EnterpriseProjectWorkforceManagementSystem.Models;

public class Attendance
{
    public int Id { get; set; }

    [Required]
    public string EmployeeId { get; set; } = string.Empty;
    public ApplicationUser Employee { get; set; } = null!;

    public DateTime Date { get; set; }
    public TimeSpan CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public AttendanceStatus Status { get; set; }
}

public enum AttendanceStatus
{
    Present = 1,
    Late = 2,
    Absent = 3
}
