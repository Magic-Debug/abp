using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class HelloDbContextFactory : IDesignTimeDbContextFactory<BloggingDbContext>
    {
        public BloggingDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = BuildConfiguration();
            string connectionString = configuration.GetConnectionString("MySql");

            DbContextOptionsBuilder<BloggingDbContext> builder = new DbContextOptionsBuilder<BloggingDbContext>()
                .UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion);

            return new BloggingDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), @"E:\source\code\abp\e-Shop-Blogging\Volo.Blogging.App.EntityFrameworkCore\"))
                .AddJsonFile("appsettings.json", optional: false);
            return builder.Build();
        }
    }
}
