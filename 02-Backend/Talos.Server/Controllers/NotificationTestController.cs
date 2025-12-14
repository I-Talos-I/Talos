using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Talos.Server.Application.RealTime;

namespace Talos.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationTestController : ControllerBase
{
    private readonly IHubContext<NotificationsHub> _hubContext;

    public NotificationTestController(IHubContext<NotificationsHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    //POST: /api/NotificationTest/send
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        return Ok(new { success = true, sent = message});
    }
}