using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ABP.WebApi.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ////如果是控制台下使用，后面加上console参数即可，默认是服务方式
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            if (isService)
            {
                //获取当前程序所在目录
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var builder = BuildWebHost(
                args.Where(arg => arg != "--console").ToArray());

            if (isService)
            {
                //使用服务启动
                builder.RunAsService();
            }
            else
            {
                builder.Run();
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStaticWebAssets()
                .UseUrls("http://*:21021")
                .UseStartup<Startup>()
                .Build();
        }
    }
}
