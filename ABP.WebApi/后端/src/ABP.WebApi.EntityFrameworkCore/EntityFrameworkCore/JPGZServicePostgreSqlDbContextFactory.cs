using ABP.WebApi;
using ABP.WebApi.Configuration;
using ABP.WebApi.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.EntityFrameworkCore
{
    public class JPGZServicePostgreSqlDbContextFactory : IDesignTimeDbContextFactory<JPGZServicePostgreSqlDbContext>
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        public JPGZServicePostgreSqlDbContextFactory(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetConfiguration();
        }
        public JPGZServicePostgreSqlDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<JPGZServicePostgreSqlDbContext>();
           

            JPGZServicePostgreSqlDbContextConfigurer.Configure(builder, _appConfiguration[$"ConnectionStrings:{WebApiConsts.PostgreSqlConnectionStringName}"]);

            return new JPGZServicePostgreSqlDbContext(builder.Options);
        }
    }
}
