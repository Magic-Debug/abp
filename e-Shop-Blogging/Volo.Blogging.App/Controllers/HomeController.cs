using System;
using System.Collections;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging;

namespace Volo.BloggingTestApp.Controllers;

public class HomeController : AbpController
{
    private readonly BloggingUrlOptions _blogOptions;

    public HomeController(IOptions<BloggingUrlOptions> blogOptions)
    {
        _blogOptions = blogOptions.Value;
    }
    public ActionResult Index()
    {
        string urlPrefix = _blogOptions.RoutePrefix;
        return Redirect(urlPrefix);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/env")]
    public ActionResult Env()
    {
        IDictionary envVariables = System.Environment.GetEnvironmentVariables();
        return Json(envVariables);
    }

    [HttpGet("api/info")]
    public ActionResult Info()
    {
        var env = new {
            Environment.MachineName,
            Environment.ProcessId,
            Environment.OSVersion,
            Environment.ProcessorCount,
            Environment.SystemDirectory,
            Environment.CurrentDirectory,
            Environment.CommandLine,
        };
        return Json(env);
    }

    [HttpGet("api/host")]
    public ActionResult Host()
    {
        string hostName = Dns.GetHostName();
        IPHostEntry entry = Dns.GetHostEntry(hostName);
        return Json(new {
            HostName = entry.HostName,
            Address = entry.AddressList.Select(address => address.ToString())
        });
    }
}
