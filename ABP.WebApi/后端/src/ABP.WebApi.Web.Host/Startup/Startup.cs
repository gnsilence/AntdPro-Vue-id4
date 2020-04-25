using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using ABP.WebApi.Configuration;
using Abp.AspNetCore.SignalR.Hubs;
using AuditLogView;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.Json;
using Abp.Dependency;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;
using ABP.WebApi.Auth;
using System.IO;

namespace ABP.WebApi.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //MVC
            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
                }
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });
            services.AddRazorPages();
            AuthConfigurer.Configure(services, _appConfiguration);
            services.AddLogUI();
            services.AddSignalR();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.Configure<WebEncoderOptions>(options =>
               options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs)
           );
            services.AddAutoMapperProfile();
            ConfigHangfire.ConfigureHangFire(services, _appConfiguration);
            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ABPWebApi APIService",
                    Version = "v1",
                    Description = "Use For Create Api Simple and Faster",
                    Contact = new OpenApiContact
                    {
                        Name = "gnsilence",
                        Email = "592254126@qq.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License"
                    }
                });
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                // Define the oauth2 scheme that's in use
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {

                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new System.Uri($"{_appConfiguration["Authentication:JwtBearer:Authority"]}/connect/authorize"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "apitest", "api权限" },//指定客户端请求的api作用域。 如果为空，则客户端无法访问
                            },

                        }
                    }
                });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "ABP.WebApi.Application.xml");
                options.IncludeXmlComments(xmlPath);
                options.OperationFilter<SecurityRequirementsOperationFilter>();

            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<WebApiWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; options.UseSecurityHeaders = false; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();

            app.UseAbpRequestLocalization();
            var serviceProvider = app.ApplicationServices;
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            AuthContextService.Configure(httpContextAccessor);
            ConfigHangfire.UseHangfireSettings(app, env, loggerFactory);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.OAuthClientId("swagger");
                options.OAuthScopeSeparator("openid profile email apitest");
                options.SwaggerEndpoint(_appConfiguration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json", "ABPService API V1");
                //options.IndexStream = () => Assembly.GetExecutingAssembly()
                //    .GetManifestResourceStream("ABP.WebApi.Web.Host.wwwroot.swagger.ui.index.html");
            });
        }
    }
}
