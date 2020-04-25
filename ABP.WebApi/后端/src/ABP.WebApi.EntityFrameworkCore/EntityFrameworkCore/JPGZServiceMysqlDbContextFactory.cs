
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
   public class JPGZServiceMysqlDbContextFactory: IDesignTimeDbContextFactory<JPGZServiceMysqlDbContext>
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        public JPGZServiceMysqlDbContextFactory(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetConfiguration();
        }
        public JPGZServiceMysqlDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<JPGZServiceMysqlDbContext>();

            JPGZServiceMysqlDbContextConfigurer.Configure(
                builder,
                _appConfiguration[$"ConnectionStrings:{WebApiConsts.MysqlConnectionStringName}"]
            );

            return new JPGZServiceMysqlDbContext(builder.Options);
        }

    }
}
