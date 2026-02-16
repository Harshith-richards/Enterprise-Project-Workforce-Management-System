using EnterpriseProjectWorkforceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseProjectWorkforceManagementSystem.ViewModels;

public class ProjectFormViewModel
{
    public int? Id { get; set; }

    [Required]
    public string ProjectName { get; set; } = string.Empty;

    [Required]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    public ProjectType ProjectType { get; set; }

    [Required]
    public decimal Budget { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

    [DataType(DataType.Date)]
    public DateTime Deadline { get; set; } = DateTime.UtcNow.Date.AddMonths(1);

    public ProjectStatus Status { get; set; } = ProjectStatus.Upcoming;
    public string? ManagerId { get; set; }
    public List<string> EngineerIds { get; set; } = [];

    public List<SelectListItem> Managers { get; set; } = [];
    public List<SelectListItem> Engineers { get; set; } = [];
}
