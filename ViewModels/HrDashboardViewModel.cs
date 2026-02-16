using EnterpriseProjectWorkforceManagementSystem.Models;

namespace EnterpriseProjectWorkforceManagementSystem.ViewModels;

public class HrDashboardViewModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int TotalRecords { get; set; }
    public int PresentCount { get; set; }
    public int LateCount { get; set; }
    public int AbsentCount { get; set; }
    public Dictionary<string, int> LateByEmployee { get; set; } = new();
    public List<Attendance> MonthlyRecords { get; set; } = [];
}
