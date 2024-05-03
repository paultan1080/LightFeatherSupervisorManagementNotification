namespace LightFeatherSupervisorManagementNotificationApi.Service.Impl.Api;

using System.Text.Json;
using Microsoft.Extensions.Options;
using Model;

public class ApiSupervisorService(HttpClient httpClient, IOptions<ApiSettings> options, ILogger<ApiSupervisorService> logger) : ISupervisorService {
    private readonly ApiSettings _apiSettings = options.Value;

    public async Task<IEnumerable<Supervisor>> GetSupervisorsAsync()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
                        
            var response = await httpClient.GetAsync(_apiSettings.SupervisorApiUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Supervisor>>(content, options);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Failed to fetch supervisors from {Url}", _apiSettings.SupervisorApiUrl);
            throw new InvalidOperationException("Failed to fetch supervisors", e);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An unexpected error occurred");
            throw;
        }
    }
}
