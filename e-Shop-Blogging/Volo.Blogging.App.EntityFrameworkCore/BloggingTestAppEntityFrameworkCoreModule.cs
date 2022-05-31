using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging.App.EntityFrameworkCore;

[DependsOn(
    typeof(BloggingEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(BlobStoringDatabaseEntityFrameworkCoreModule))]
public class BloggingTestAppEntityFrameworkCoreModule : AbpModule
{
}
