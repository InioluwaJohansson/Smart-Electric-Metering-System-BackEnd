using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Smart_Electric_Metering_System_BackEnd.Context
{
    public class SmartElectricMeteringContextFactory : IDesignTimeDbContextFactory<SmartElectricMeteringContext>
    {
        public SmartElectricMeteringContext CreateDbContext(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var connectionString = config.GetConnectionString("SmartElectricMeteringContext");

            var optionsBuilder = new DbContextOptionsBuilder<SmartElectricMeteringContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new SmartElectricMeteringContext(optionsBuilder.Options);
        }
    }
}
