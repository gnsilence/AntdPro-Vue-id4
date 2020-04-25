using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using ABP.WebApi;
using ABP.WebApi.Configuration;
using ABP.WebApi.EntityFrameworkCore;
using ABP.WebApi.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.EntityFrameworkCore
{
  public  class MyConnectionStringResolver: DefaultConnectionStringResolver
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        public MyConnectionStringResolver(IAbpStartupConfiguration configuration, IHostingEnvironment env)
            : base(configuration)
        {
            _env = env;
            _appConfiguration = env.GetConfiguration();
        }
        public override string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            // if SqlServer 
            if (args["DbContextConcreteType"] as Type == typeof(WebApiDbContext))
            {
                return _appConfiguration[$"ConnectionStrings:{WebApiConsts.ConnectionStringName}"];
            }
            // if mysql
            if (args["DbContextConcreteType"] as Type == typeof(JPGZServiceMysqlDbContext))
            {
                return _appConfiguration[$"ConnectionStrings:{WebApiConsts.MysqlConnectionStringName}"];
            }
            // if postgresql
            if (args["DbContextConcreteType"] as Type == typeof(JPGZServicePostgreSqlDbContext))
            {
                return _appConfiguration[$"ConnectionStrings:{WebApiConsts.MysqlConnectionStringName}"];
            }
            return base.GetNameOrConnectionString(args);
        }
    }
}
