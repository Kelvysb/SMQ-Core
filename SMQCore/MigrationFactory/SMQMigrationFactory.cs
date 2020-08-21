using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SMQCore.DataAccess.Contexts;

namespace SMQCore.MigrationFactory
{
    public class SMQMigrationFactory : IDesignTimeDbContextFactory<SMQContext>
    {
        public SMQContext CreateDbContext(string[] args)
        {
            var resourceName = "appsettings.json";
            var config = new ConfigurationBuilder().AddJsonFile(resourceName).Build();
            return new SMQContext(config);
        }
    }
}