using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using ABP.WebApi.Authentication.JwtBearer;
using ABP.WebApi.Configuration;
using ABP.WebApi.EntityFrameworkCore;
using Abp.Grpc.Server;
using Abp.Grpc.Server.Extensions;
using Abp.Grpc.Client;
using Abp.Grpc.Client.Extensions;
using Abp.Grpc.Client.Configuration;
using Abp.Runtime.Caching.Redis;
using Abp.MailKit;
using Abp.Web.Api.ProxyScripting.Generators;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Abp.Web;

namespace ABP.WebApi
{
    [DependsOn(
         typeof(WebApiApplicationModule),
         typeof(WebApiEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
        , typeof(AbpAspNetCoreSignalRModule),
        typeof(AbpGrpcServerModule),
        typeof(AbpGrpcClientModule),
          typeof(AbpAspNetCoreModule),
        typeof(AbpRedisCacheModule),
        typeof(AbpMailKitModule)
     )]
    public class WebApiWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public WebApiWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                WebApiConsts.ConnectionStringName
            );
            //配置grpc
            Configuration.Modules.UseGrpcService(option =>
            {
                option.GrpcBindAddress = _appConfiguration["Grpc:GrpcBindAddress"];
                option.GrpcBindPort = int.Parse(_appConfiguration["Grpc:GrpcBindPort"]);
            }).AddRpcServiceAssembly(typeof(WebApiApplicationModule).Assembly);
            //禁用redis缓存会自动使用内存缓存
            if (bool.Parse(_appConfiguration["App:RedisCache:IsEnabled"]))
            {
                //使用redis作为缓存
                Configuration.Caching.UseRedis(options =>
                {
                    options.ConnectionString = _appConfiguration["App:RedisCache:ConnectionString"];
                    options.DatabaseId = _appConfiguration.GetValue<int>("App:RedisCache:DatabaseId");
                });
                //配置redis的Cache过期时间
                Configuration.Caching.Configure("redis", cache =>
                {
                    //缓存滑动过期时间,时长应当根据数据的更新频率来设置
                    cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(15);
                });
            }
            //其他缓存时间配置
            Configuration.Caching.ConfigureAll(options =>
            {
                options.DefaultSlidingExpireTime = TimeSpan.FromMinutes(15);
            });
            //使用配置管理器
            Configuration.Settings.Providers.Add<ConfigSettingProvider>();
            // Use database for language management
            //Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(WebApiApplicationModule).GetAssembly()
                 )/* 自定义路由格式，
                 默认为/api/services/app/Controller.ControllerName/action.ActionName/*/
                 .ConfigureControllerModel(model =>
                 {
                     //判断是否已经默认添加了httpverb,添加了则保留已经添加的,abp的方法不添加http动词,默认是post请求
                     static bool IsDefcultVerb(object o) => o switch
                     {
                         HttpGetAttribute v when v is HttpGetAttribute => true,
                         HttpDeleteAttribute d when d is HttpDeleteAttribute => true,
                         HttpPatchAttribute p when p is HttpPatchAttribute => true,
                         HttpPostAttribute p when p is HttpPostAttribute => true,
                         HttpPutAttribute p when p is HttpPutAttribute => true,
                         _ => false
                     };
                     foreach (var action in model.Actions)
                     {
                         var verb = ProxyScriptingHelper.GetConventionalVerbForMethodName(action.ActionName);
                         var constraint = new HttpMethodActionConstraint(new List<string> { verb.ToString().ToUpper() });
                         foreach (var selector in action.Selectors)
                         {
                             bool ifhasverb = false;
                             foreach (var item in selector.EndpointMetadata)
                             {
                                 if (IsDefcultVerb(item))
                                 {
                                     ifhasverb = true;
                                 }
                             }
                             if (!ifhasverb)//如果不包含http动词
                             {
                                 selector.ActionConstraints.Add(constraint);
                             }
                             //selector.ActionConstraints.Add(constraint);
                             // 更改默认路由格式
                             selector.AttributeRouteModel = new AttributeRouteModel(
                                 new RouteAttribute(
                                     $"api/{action.Controller.ControllerName}/{action.ActionName}"
                                 )
                             );
                         }
                     }
                 });

            ConfigureTokenAuth();
        }
        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }
        public override void PostInitialize()
        {
            // 配置grpc，直连模式
            Configuration.Modules.UseGrpcClientForDirectConnection(new[]
            {
                new GrpcServerNode
                {
                    GrpcServiceIp = "127.0.0.1",
                    GrpcServiceName = "TestServiceName",
                    GrpcServicePort = int.Parse(_appConfiguration["Grpc:GrpcBindPort"])
        }
            });
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WebApiWebCoreModule).GetAssembly());
        }
    }
}
