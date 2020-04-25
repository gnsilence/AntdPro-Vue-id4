using Grpc.Core.Logging;
using Hangfire;
using Hangfire.HttpJob;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using Hangfire.Dashboard;
using Hangfire.Console;
using Microsoft.Extensions.Configuration;
using Hangfire.Heartbeat;
using Hangfire.Heartbeat.Server;
using Hangfire.Dashboard.BasicAuthorization;

namespace ABP.WebApi.Web.Host.Startup
{
    public class ConfigHangfire
    {
        #region Properties
        /// <summary>
        /// 使用redis缓存
        /// </summary>
        static ConnectionMultiplexer Redis;
        /// <summary>
        /// 是否启用hangfire
        /// </summary>
        private static bool IsEnabled { get; set; }
        /// <summary>
        /// 是否只读面板
        /// </summary>
        private static bool IsReadOnly { get; set; }
        /// <summary>
        /// 返回跳转的链接
        /// </summary>
        private static string BackLink { get; set; }
        /// <summary>
        /// 队列名称集合
        /// </summary>
        private static string[] Queues { get; set; }
        /// <summary>
        /// 管理员账号
        /// </summary>
        private static string Account { get; set; }
        /// <summary>
        /// 管理员密码
        /// </summary>
        private static string Password { get; set; }
        /// <summary>
        /// hangfire存储库地址
        /// </summary>
        private static string ConnectionStrng { get; set; }
        #endregion

        /// config hangfire
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureHangFire(IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var listqueue = new List<string>();
                IsEnabled = bool.Parse(configuration["Hangfire:IsEnabled"]);
                IsReadOnly = bool.Parse(configuration["Hangfire:IsReadOnly"]);
                BackLink = configuration["Hangfire:BackLink"].ToString();
                configuration.GetSection("Hangfire:Queues").Bind(listqueue);
                Queues = listqueue.Any() ? listqueue.ToArray<string>() : default;
                Account = configuration["Hangfire:AdminAccount"].ToString();
                Password = configuration["Hangfire:AdminPassword"].ToString();
                ConnectionStrng = configuration["Hangfire:ConnectionStrng"].ToString();
                if (IsEnabled)
                {
                    Redis = ConnectionMultiplexer.Connect(ConnectionStrng);
                    services.AddHangfire(config =>
                    {
                        config.UseHeartbeatPage(checkInterval: TimeSpan.FromSeconds(1));
                        //使用redis
                        config.UseRedisStorage(Redis, new Hangfire.Redis.RedisStorageOptions()
                        {
                            FetchTimeout = TimeSpan.FromMinutes(5),
                            Prefix = "{hangfire}:",
                            //活动服务器超时时间
                            InvisibilityTimeout = TimeSpan.FromHours(1),
                            //任务过期检查频率
                            ExpiryCheckInterval = TimeSpan.FromHours(1),
                            DeletedListSize = 10000,
                            SucceededListSize = 10000
                        })

                        .UseHangfireHttpJob(new HangfireHttpJobOptions()
                        {
                            AddHttpJobButtonName = "添加计划任务",
                            AddRecurringJobHttpJobButtonName = "添加定时任务",
                            EditRecurringJobButtonName = "编辑定时任务",
                            PauseJobButtonName = "暂停或开始",
                            DashboardTitle = "计时服务管理面板",
                            DashboardName = "后台任务管理",
                            DashboardFooter = "计时服务管理面板",
                            SMTPPort = Convert.ToInt32(configuration["App:SMTP:Port"].ToString()),// 邮件端口
                            SMTPServerAddress = configuration["App:SMTP:Domain"].ToString(),// 邮件服务地址
                            SMTPPwd = configuration["App:SMTP:Password"].ToString(),// 发送者密码
                            SMTPSubject = configuration["App:SMTP:SMTPSubject"].ToString(),// 邮件标题
                            SendMailAddress = configuration["App:SMTP:UserName"].ToString(),// 发送着账号
                            SendToMailList = configuration["App:SMTP:SendToMailList"].ToString().Split(',').ToList(),// 接收人地址列表
                            Authority = configuration["Authentication:JwtBearer:Authority"].ToString(),// identitserver服务的地址,用来请求需要认证的服务
                            ClientId= configuration["Hangfire:ClientId"].ToString(),
                            ClientSecret= configuration["Hangfire:ClientSecret"].ToString()
                        })
                        .UseConsole(new ConsoleOptions()
                        {
                            BackgroundColor = "#000000"
                        })
                        .UseDashboardMetric(DashboardMetrics.AwaitingCount)
                        .UseDashboardMetric(DashboardMetrics.ProcessingCount)
                        .UseDashboardMetric(DashboardMetrics.RecurringJobCount)
                        .UseDashboardMetric(DashboardMetrics.RetriesCount)
                        .UseDashboardMetric(DashboardMetrics.FailedCount)
                        .UseDashboardMetric(DashboardMetrics.SucceededCount)
                        ;
                    });
                }

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// config hangfirez
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public static void UseHangfireSettings(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                if (IsEnabled)
                {

                    // 设置语言
                    var supportedCultures = new[]
                    {
                        new CultureInfo("zh-CN"),
                        new CultureInfo("en-US")
                    };
                    app.UseRequestLocalization(new RequestLocalizationOptions
                    {
                        DefaultRequestCulture = new RequestCulture("zh-CN"),
                        // Formatting numbers, dates, etc.
                        SupportedCultures = supportedCultures,
                        // UI strings that we have localized.
                        SupportedUICultures = supportedCultures
                    });
                    var process = Process.GetCurrentProcess();
                    var prcss = Process.GetProcesses(Environment.MachineName).OrderByDescending(p => p.PrivateMemorySize64).Take(5);
                    List<ProcessMonitor> processMonitors = new List<ProcessMonitor>();
                    processMonitors.Add(new ProcessMonitor(TimeSpan.FromSeconds(1), Process.GetCurrentProcess()));
                    prcss.ToList().ForEach(s =>
                    {
                        var moni = new ProcessMonitor(TimeSpan.FromSeconds(5), s);
                        processMonitors.Add(moni);
                    });
                    app.UseHangfireServer(new BackgroundJobServerOptions()
                    {
                        ServerTimeout = TimeSpan.FromMinutes(4),
                        SchedulePollingInterval = TimeSpan.FromSeconds(1),// 秒级任务需要配置短点，一般任务可以配置默认时间，默认15秒
                        ShutdownTimeout = TimeSpan.FromMinutes(30),// 超时时间
                        Queues = Queues,// 队列
                        WorkerCount = Math.Max(Environment.ProcessorCount, 40)// 工作线程数，当前允许的最大线程，默认20
                    },
                    additionalProcesses: processMonitors.ToArray());
                    app.UseHangfireDashboard("/job", new DashboardOptions
                    {
                        AppPath = BackLink,// 返回时跳转的地址
                        DisplayStorageConnectionString = false,// 是否显示数据库连接信息
                        IsReadOnlyFunc = Context =>
                        {
                            var isreadonly = IsReadOnly;
                            return isreadonly;
                        },
                        Authorization = new[]
                        {
                           new Id4AuthAuthorizationFilter()
                        }
                        //Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                        //{
                        //    RequireSsl = false,// 是否启用ssl验证，即https
                        //    SslRedirect = false,
                        //    LoginCaseSensitive = true,
                        //    Users = new []
                        //    {
                        //        new BasicAuthAuthorizationUser
                        //        {
                        //            Login =Account,// 登录账号
                        //            PasswordClear =  Password// 登录密码
                        //        }
                        //    }
                        //})
                        //}
                    });
                }
            }
            catch (Exception ex)
            {
                loggerFactory.CreateLogger("HangfireStartUpError").Log(LogLevel.Error, ex.ToString());
            }
        }
    }
}
