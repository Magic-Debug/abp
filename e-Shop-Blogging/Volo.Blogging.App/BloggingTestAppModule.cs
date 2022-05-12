using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI;
using Volo.Abp.VirtualFileSystem;
using Volo.Blogging.Admin;
using Volo.Blogging.App.EntityFrameworkCore;

namespace Volo.Blogging.App;

[DependsOn(
    typeof(BloggingWebModule),
    typeof(BloggingApplicationModule),
    typeof(BloggingHttpApiModule),
    typeof(BloggingAdminWebModule),
    typeof(BloggingAdminHttpApiModule),
    typeof(BloggingAdminApplicationModule),
    typeof(AbpAspNetCoreSignalRModule),

    typeof(BloggingTestAppEntityFrameworkCoreModule),
    typeof(BlobStoringDatabaseDomainModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule)
)]
public class BloggingTestAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IWebHostEnvironment hostingEnvironment = context.Services.GetHostingEnvironment();
        IConfiguration configuration = context.Services.GetConfiguration();
        ConfigureCache();
        Configure<BloggingUrlOptions>(options =>
        {
            options.RoutePrefix = null;
        });

        Configure<AbpDbConnectionOptions>(options =>
        {
            const string connStringName = "SqlServer";
            options.ConnectionStrings.Default = configuration.GetConnectionString(connStringName);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseSqlServer();
        });
        if (!hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<AbpUiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}framework{0}src{0}Volo.Abp.UI", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiBootstrapModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Bootstrap", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiThemeSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiBasicThemeModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}modules{0}basic-theme{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<BloggingDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}src{0}Volo.Blogging.Domain", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<BloggingWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}src{0}Volo.Blogging.Web", Path.DirectorySeparatorChar)));
            });
        }

        context.Services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Blogging API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });

        List<CultureInfo> cultures = new List<CultureInfo>
        {
            new CultureInfo("cs"),
            new CultureInfo("en"),
            new CultureInfo("zh-Hans")
        };

        Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("cs");
            options.SupportedCultures = cultures;
            options.SupportedUICultures = cultures;
        });

        Configure<AbpThemingOptions>(options =>
        {
            options.DefaultThemeName = BasicTheme.Name;
        });

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseDatabase();
            });
        });
        AddCustomAuthentication(context.Services, configuration);
        AddCustomAuthorization(context.Services);
    }
    private void AddCustomAuthentication(IServiceCollection builder, IConfiguration configuration)
    {
        // Prevent mapping "sub" claim to nameidentifier.
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        builder.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Audience = "blogging-api";
                options.Authority = configuration.GetValue<string>("AuthorityUrl");
                options.RequireHttpsMetadata = false;
            });
    }

    private void AddCustomAuthorization(IServiceCollection builder)
    {
        builder.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "blogging");
            });
        });
    }

    private void ConfigureCache()
    {
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "Blogging:";
        });
    }
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        IApplicationBuilder app = context.GetApplicationBuilder();

        if (context.GetEnvironment().IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseErrorPage();
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAbpRequestLocalization();

        app.UseConfiguredEndpoints();

        using IServiceScope scope = context.ServiceProvider.CreateScope();
        AsyncHelper.RunSync(async () =>
        {
            await scope.ServiceProvider
                .GetRequiredService<IDataSeeder>()
                .SeedAsync();
        });
    }
}
