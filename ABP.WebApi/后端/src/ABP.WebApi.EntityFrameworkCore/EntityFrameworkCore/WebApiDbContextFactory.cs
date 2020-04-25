using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ABP.WebApi.Configuration;
using ABP.WebApi.Web;
using Microsoft.AspNetCore.Hosting;

namespace ABP.WebApi.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class WebApiDbContextFactory : IDesignTimeDbContextFactory<WebApiDbContext>
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        public WebApiDbContextFactory(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetConfiguration();
        }
        public WebApiDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<WebApiDbContext>();
            

            WebApiDbContextConfigurer.Configure(builder, _appConfiguration[$"ConnectionStrings:{WebApiConsts.ConnectionStringName}"]);

            return new WebApiDbContext(builder.Options);
        }
    }
}
