using Microsoft.AspNetCore.SignalR;

namespace Talos.Server.Application.RealTime;

public class NotificationService
{
    private readonly IHubContext<NotificationsHub> _hub;
    
    public NotificationService(IHubContext<NotificationsHub> hub)
    {
        _hub = hub;
    }

    public async Task NotifyTemplateCreated(string user, string template)
    {
        await _hub.Clients.All.SendAsync("templateCreated", new
        {
            user,
            template,
            timestamp = DateTime.UtcNow
        });
    }

    public async Task NotifyCompatibilityUpdated(string package, string version)
    {
        await _hub.Clients.All.SendAsync("compatibilityUpdated", new
        {
            package,
            version,
            timestamp = DateTime.UtcNow
        });
    }
}