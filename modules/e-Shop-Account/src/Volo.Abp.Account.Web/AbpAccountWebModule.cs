using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Builder;

using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Autofac;
using Volo.Abp.Identity;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AuthenticationFailedContext = Microsoft.AspNetCore.Authentication.JwtBearer.AuthenticationFailedContext;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp.Caching;
using Volo.Abp.AspNetCore.Authentication.OpenIdConnect;
using Volo.Abp.AspNetCore.Authentication.OAuth;

namespace Volo.Abp.Account.Web;

[DependsOn(
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreAuthenticationOpenIdConnectModule),
    typeof(AbpAspNetCoreAuthenticationOAuthModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpExceptionHandlingModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule)
    )]
public class AbpAccountWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AccountResource), typeof(AbpAccountWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IWebHostEnvironment hostingEnvironment = context.Services.GetHostingEnvironment();
        IConfiguration configuration = context.Services.GetConfiguration();
        ConfigureProfileManagementPage();
        ConfigureCache();
        ConfigureAuthentication(context, configuration);
        ConfigureSwaggerServices(context.Services);
        context.Services.AddAutoMapperObjectMapper<AbpAccountWebModule>();

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountWebModule>();
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpAccountUserMenuContributor());
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new AccountModuleToolbarContributor());
        });
       
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpAccountWebAutoMapperProfile>(validate: true);
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(AccountRemoteServiceConsts.ModuleName);
        });
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default =configuration["ConnectionStrings:Default"];
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseSqlServer();
        });
    }
    private void ConfigureCache()
    {
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "Account:";
        });
    }
    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Account API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }
    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication()//AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters()
             {
                 ValidateAudience = false,
                 ValidateIssuer = false
                 // IssuerSigningKey = ConstFuncs.SecurityKey()
             };
             options.Events = new JwtBearerEvents();
             options.Events.OnAuthenticationFailed = (AuthenticationFailedContext authFailedCtx) =>
             {
                 authFailedCtx.Response.OnStarting(async () =>
                 {
                     object result = new { Message = "Token验证失败", Code = 401 };
                     authFailedCtx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                     authFailedCtx.Response.ContentType = "application/json";
                     string json = JsonConvert.SerializeObject(result);
                     await authFailedCtx.Response.WriteAsync(json);
                 });
                 return Task.CompletedTask;

             };
         }) .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));

        context.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Authority = "https://login.microsoftonline.com/" + configuration["AzureAd:TenantId"] + "/v2.0/";
            options.ClientId = configuration["AzureAd:ClientId"];
            options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            options.CallbackPath = configuration["AzureAd:CallbackPath"];
            options.ClientSecret = configuration["AzureAd:ClientSecret"];
            options.RequireHttpsMetadata = false;
            options.SaveTokens = false;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.Scope.Add("email");

            options.SignInScheme = IdentityConstants.ExternalScheme;

            //options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        base.OnApplicationInitialization(context);

        IApplicationBuilder app = context.GetApplicationBuilder();
        IWebHostEnvironment env = context.GetEnvironment();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();
        app.UseAuthorization();
        app.UseAbpRequestLocalization();
        app.UseAbpSecurityHeaders();
        app.UseStatusCodePagesWithReExecute("/error/{0}");
        app.UseSwagger();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API"); });
        app.UseAuditing();
        app.UseConfiguredEndpoints();
    }

    private void ConfigureProfileManagementPage()
    {
        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Account/Manage");
        });

        Configure<ProfileManagementPageOptions>(options =>
        {
            options.Contributors.Add(new AccountProfileManagementPageContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Configure(typeof(ManageModel).FullName,
                    configuration =>
                    {
                        configuration.AddFiles("/client-proxies/account-proxy.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/Password/Default.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/PersonalInfo/Default.js");
                    });
        });

    }
}
