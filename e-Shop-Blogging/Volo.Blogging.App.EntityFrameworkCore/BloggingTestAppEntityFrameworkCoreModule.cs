using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.BloggingTestApp.EntityFrameworkCore
{
    [DependsOn(
        typeof(BloggingEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(BlobStoringDatabaseEntityFrameworkCoreModule))]
    public class BloggingTestAppEntityFrameworkCoreModule : AbpModule
    {
    }
}
