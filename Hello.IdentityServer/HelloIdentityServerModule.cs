using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.Emailing;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Identity.Web;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.BackgroundJobs;
using IdentityServer4.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.RabbitMQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Extensions;
using Volo.Abp.AspNetCore.WebClientInfo;
using Microsoft.AspNetCore.HttpOverrides;
using Volo.Abp.IdentityServer;

namespace Hello.IdentityServer;

[DependsOn(
    typeof(AbpPermissionManagementWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpFeatureManagementWebModule),
    typeof(AbpTenantManagementWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpAccountWebIdentityServerModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityHttpApiModule),
    // typeof(AbpBackgroundJobsRabbitMqModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpIdentityServerEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
    )]
public class HelloIdentityServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AbpIdentityServerDbProperties.DbTablePrefix = string.Empty;

        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();
        context.Services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
        ConfigureIdentityServerOptions(configuration);
        context.Services.Replace(ServiceDescriptor.Transient<IWebClientInfoProvider, NginxWebClientInfoProvider>());
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL(x =>
            {

            });
        });
        Configure<AbpDbConnectionOptions>(options =>
        {
        });
        context.Services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Hello.IdentityServer API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
        Configure<AbpAuditingOptions>(options =>
        {
            //options.IsEnabledForGetRequests = true;
            options.ApplicationName = "ABP Hello.IdentityServer";
        });
        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = true;
        });
        context.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        Configure<AbpBackgroundJobWorkerOptions>(options =>
        {
            options.JobPollPeriod = 1000 * 10;
            options.DefaultFirstWaitDuration = 1;
            options.DefaultWaitFactor = 1;
        });

        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "ids4:";
        });
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

        });

        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });

        context.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
                options.Events = new JwtBearerEvents();
                options.Events.OnAuthenticationFailed = (AuthenticationFailedContext authFailedCtx) =>
                {
                    authFailedCtx.Response.OnStarting(async () =>
                    {
                        var result = new { Message = "Token验证失败", Code = 401 };
                        authFailedCtx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        authFailedCtx.Response.ContentType = "application/json";
                        string json = JsonConvert.SerializeObject(result);
                        await authFailedCtx.Response.WriteAsync(json);
                    });
                    return Task.CompletedTask;
                };
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                //options.Audience = configuration["AuthServer:ApiName"];
            });
        //context.Services.ForwardIdentityAuthenticationForBearer();

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });

        var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("Hello.IdentityServer");
        if (!hostingEnvironment.IsDevelopment())
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "Hello.IdentityServer-Protection-Keys");
        }

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
    }
    private void ConfigureIdentityServerOptions(IConfiguration configuration)
    {
        Configure<IdentityServerOptions>(options =>
        {
            options.Events.RaiseSuccessEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.IssuerUri = configuration["App:IssuerUri"];
            //options.PublicOrigin = configuration["App:PublicOrigin"];
            options.LowerCaseIssuerUri = true;
            options.MutualTls.Enabled = true;
            options.MutualTls.ClientCertificateAuthenticationScheme = "x509";

            System.Diagnostics.Debug.WriteLine(options);
        });
    }
    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        app.Use(async (ctx, next) =>
        {
            ctx.SetIdentityServerOrigin("http://82.157.167.144:30571/");
            await next();
        });

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseErrorPage();
            app.UseHsts();
        }

        // app.UseHttpsRedirection();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();

        app.UseMultiTenancy();

        app.UseAbpRequestLocalization();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
            IEndpointConventionBuilder endpointConventionBuilder = endpoints.MapHealthChecks($"api/hc", options: new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            endpoints.MapHealthChecks($"api/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self"),
                AllowCachingResponses = true
            });
        });

        await SeedData(context);
    }

    private async Task SeedData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            await scope.ServiceProvider
                .GetRequiredService<IDataSeeder>()
                .SeedAsync();
        }
    }
}
