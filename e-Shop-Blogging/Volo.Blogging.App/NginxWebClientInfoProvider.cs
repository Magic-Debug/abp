using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.WebClientInfo;

namespace Volo.Blogging.App;

public class NginxWebClientInfoProvider : HttpContextWebClientInfoProvider, IWebClientInfoProvider
{
    public NginxWebClientInfoProvider(ILogger<HttpContextWebClientInfoProvider> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {

    }
    protected override string GetClientIpAddress()
    {
        string xForwardedfor = HttpContextAccessor.HttpContext?.Request?.Headers?["X-Forwarded-For"].ToString();//从nginx转发获取，需要在nginx配置
        string clientIpAddress = string.IsNullOrEmpty(xForwardedfor) ? base.GetClientIpAddress() : xForwardedfor.ToString();
        return clientIpAddress;
    }
}

