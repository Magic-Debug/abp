using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.SignalR;

namespace Volo.Blogging.App;

/// <summary>
/// /signalr-hubs/push
/// </summary>
//[Authorize]

[HubRoute("/signalr-hubs/blogging")]
public class PushHub : AbpHub
{

    public PushHub(ILogger<PushHub> log)
    {
        log.LogInformation("Push Hub");
    }

    public async override Task OnConnectedAsync()
    {
        Logger.LogInformation("on connected");
        await base.OnConnectedAsync();

        await Clients.All.SendAsync("welcome", new { ConnectionId = this.Context.ConnectionId, Time = DateTime.Now });
    }

    public async override Task OnDisconnectedAsync(Exception exception)
    {
        await Clients.AllExcept(Context.ConnectionId).SendAsync("onDisconnected", new { ConnectionId = Context.ConnectionId, Time = DateTime.Now });
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string targetUserId, string message)
    {
        //CurrentUser.UserName
        message = $"{DateTime.Now}: {message}";
        await Clients
            .User(targetUserId.ToString())
            .SendAsync("ReceiveMessage", message);
    }
}
