using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ABP.WebApi.Application.Interceptors;
using ABP.WebApi.Authorization;

namespace ABP.WebApi
{
    [DependsOn(
        typeof(WebApiCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class WebApiApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.ReplaceService(typeof(IAuditingStore), () => {
                IocManager.RegisterIfNot<IAuditingStore, WebapiAuditStore>(DependencyLifeStyle.Transient);
            });
            //注册aop拦截
            //普通方法拦截
            ServiceInterceptorRegistrar.Initialize(IocManager);
            //授权拦截
            AuthorizationInterceptorRegistrar.Initialize(IocManager);
            //Configuration.Authorization.Providers.Add<WebApiAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(WebApiApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
