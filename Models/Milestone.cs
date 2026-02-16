using System.ComponentModel.DataAnnotations;

namespace EnterpriseProjectWorkforceManagementSystem.Models;

public class Milestone
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}
