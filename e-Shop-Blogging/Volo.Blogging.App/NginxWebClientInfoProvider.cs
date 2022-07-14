using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Volo.Abp.AspNetCore.WebClientInfo;

namespace Volo.Blogging.App;

public class NginxWebClientInfoProvider : HttpContextWebClientInfoProvider, IWebClientInfoProvider
{
    public NginxWebClientInfoProvider(ILogger<HttpContextWebClientInfoProvider> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {

    }
    protected override string GetClientIpAddress()
    {
        StringValues? xForwardedfor = HttpContextAccessor.HttpContext?.Request?.Headers?["X-Forwarded-For"];//从nginx转发获取，需要在nginx配置
        string clientIpAddress = xForwardedfor.HasValue ? xForwardedfor.Value.ToString() : base.GetClientIpAddress();
        return clientIpAddress;
    }
}

