using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Account.Web;


public class Startup
{
    public void ConfigureDevelopment0()
    {
        System.Console.WriteLine("ConfigureDevelopment");
    }

    public void Configure0()
    {
        System.Console.WriteLine("ConfigureDevelopment");
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<AbpAccountWebModule>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
