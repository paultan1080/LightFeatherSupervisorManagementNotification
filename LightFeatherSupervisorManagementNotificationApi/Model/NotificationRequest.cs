namespace LightFeatherSupervisorManagementNotificationApi.Model;

using System.ComponentModel.DataAnnotations;

public class NotificationRequest
{
    [Required(ErrorMessage = "First name is required and can't be empty.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required and can't be empty.")]
    public string LastName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Supervisor is required and can't be empty.")]
    public string Supervisor { get; set; }
}
