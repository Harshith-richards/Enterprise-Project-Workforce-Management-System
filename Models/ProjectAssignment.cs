namespace EnterpriseProjectWorkforceManagementSystem.Models;

public class ProjectAssignment
{
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public string EngineerId { get; set; } = string.Empty;
    public ApplicationUser Engineer { get; set; } = null!;
}
