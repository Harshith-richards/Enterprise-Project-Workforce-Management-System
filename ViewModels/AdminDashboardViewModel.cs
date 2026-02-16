namespace EnterpriseProjectWorkforceManagementSystem.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalEmployees { get; set; }
    public int TotalProjects { get; set; }
    public int ActiveProjects { get; set; }
    public int ProjectsNearDeadline { get; set; }
    public int DelayedProjects { get; set; }

    public int PresentCount { get; set; }
    public int LateCount { get; set; }
    public int AbsentCount { get; set; }

    public Dictionary<string, int> ProjectsByStatus { get; set; } = new();
}
