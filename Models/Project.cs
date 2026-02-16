using System.ComponentModel.DataAnnotations;

namespace EnterpriseProjectWorkforceManagementSystem.Models;

public class Project
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string ProjectName { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    public ProjectType ProjectType { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime Deadline { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Upcoming;

    public string? ManagerId { get; set; }
    public ApplicationUser? Manager { get; set; }

    public ICollection<ProjectAssignment> Assignments { get; set; } = new List<ProjectAssignment>();
    public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
}

public enum ProjectType
{
    HVAC = 1,
    MEP = 2
}

public enum ProjectStatus
{
    Upcoming = 1,
    Ongoing = 2,
    Completed = 3,
    Delayed = 4
}
