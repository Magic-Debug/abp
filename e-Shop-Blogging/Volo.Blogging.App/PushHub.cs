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
public class PushHub : AbpHub
{

    public PushHub(ILogger<PushHub>  log)
    {
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
