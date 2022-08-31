using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Volo.Abp.AspNetCore.WebClientInfo;

namespace Hello.IdentityServer;

public class NginxWebClientInfoProvider: HttpContextWebClientInfoProvider
{
    public NginxWebClientInfoProvider(ILogger<HttpContextWebClientInfoProvider> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {

    }
    protected override string GetClientIpAddress()
    {
        //从nginx转发获取，需要在nginx配置 X-Forwarded-For $proxy_add_x_forwarded_for;
        StringValues? xForwardedfor = HttpContextAccessor.HttpContext?.Request?.Headers?["X-Forwarded-For"];
        string clientIpAddress = xForwardedfor.HasValue ? xForwardedfor.Value.ToString() : base.GetClientIpAddress();
        return clientIpAddress;
    }
}
