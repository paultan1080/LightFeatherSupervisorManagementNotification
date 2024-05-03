namespace LightFeatherSupervisorManagementNotificationApi;

using System.ComponentModel.DataAnnotations;

public class ApiSettings
{
    [Required]
    [Url]
    public string SupervisorApiUrl { get; set; }
}
