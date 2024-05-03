namespace LightFeatherSupervisorManagementNotificationApi.Service;

using Model;

public interface ISupervisorService
{
    Task<IEnumerable<Supervisor>> GetSupervisorsAsync();
}
