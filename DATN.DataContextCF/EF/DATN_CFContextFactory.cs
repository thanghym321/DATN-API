using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.DataContextCF.EF
{
    public class DATN_CFContextFactory : IDesignTimeDbContextFactory<DATN_CFContext>
    {
        public DATN_CFContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DATN_CF");

            var optionsBuilder = new DbContextOptionsBuilder<DATN_CFContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DATN_CFContext(optionsBuilder.Options);
        }
    }
}
