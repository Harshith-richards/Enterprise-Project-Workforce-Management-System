using System.ComponentModel.DataAnnotations;

namespace EnterpriseProjectWorkforceManagementSystem.ViewModels;

public class CreateUserViewModel
{
    [Required, MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Department { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "P@ssw0rd123!";

    [DataType(DataType.Date)]
    public DateTime DateJoined { get; set; } = DateTime.UtcNow.Date;
}
