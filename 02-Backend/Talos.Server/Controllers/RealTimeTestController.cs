using Microsoft.AspNetCore.Mvc;
using Talos.Server.Application.RealTime;

namespace Talos.Server.Controllers;

[ApiController]
[Route("api/realtime")]
public class RealTimeTestController : ControllerBase
{
    private readonly NotificationService _notifications;
    
    public RealTimeTestController(NotificationService notifications)
    {
        _notifications = notifications;
    }

    [HttpPost("template-created")]
    public async Task<IActionResult> NotifyTemplateCreated(string user, string template)
    {
        await _notifications.NotifyTemplateCreated(user, template);
        return Ok(new { message = "Template notification sent" });
    }

    [HttpPost("compatibility-update")]
    public async Task<IActionResult> NotifyCompatibilityUpdate(string package, string version)
    {
        await _notifications.NotifyCompatibilityUpdated(package, version);
        return Ok(new { message = "Compatibility notification sent" });
    }
}