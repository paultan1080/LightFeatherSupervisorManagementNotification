namespace LightFeatherSupervisorManagementNotificationApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

[Route("api")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly ISupervisorService _supervisorService;

    public ApiController(ISupervisorService supervisorService)
    {
        _supervisorService = supervisorService;
    }

    [HttpGet("supervisors")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var supervisors = await _supervisorService.GetSupervisorsAsync();

            return Ok(
            supervisors
                .Where(s => !String.IsNullOrWhiteSpace(s.Jurisdiction) && char.IsLetter(s.Jurisdiction.First()))
                .Select(s => $"{s.Jurisdiction} - {s.LastName}, {s.FirstName}")
                .OrderBy(name => name)
                .ToList()
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching supervisors.");
        }
    }
            
    [HttpPost("submit")]
    public IActionResult SubmitNotification([FromBody] NotificationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Console.WriteLine($"New notification request for Supervisor: {request.Supervisor}");
        Console.WriteLine($"Request Details: {request.FirstName} {request.LastName}, Email: {request.Email}, Phone: {request.PhoneNumber}");

        return Ok();
    }
}
