using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace Volo.Blogging.App;

public class LogConnectionHandler : ConnectionHandler
{
    ILogger<LogConnectionHandler> Logger { get; }
    public LogConnectionHandler(ILogger<LogConnectionHandler> logger)
    {
        Logger = logger;
    }
    public override Task OnConnectedAsync(ConnectionContext connection)
    {
        connection.ConnectionClosed.Register((state) =>
        {
            Logger.LogInformation($"{connection.ConnectionId}\t{connection.RemoteEndPoint} 关闭连接[ConnectionClosed]");
        }, connection);
        return Task.Run(() => Logger.LogInformation($"{connection.ConnectionId}\t{connection.RemoteEndPoint}  建立连接[OnConnected]"));
    }
}
