using Volo.Abp.Modularity;

namespace Volo.Blogging
{
    [DependsOn(typeof(BloggingApplicationContractsSharedModule))]
    public class BloggingApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
