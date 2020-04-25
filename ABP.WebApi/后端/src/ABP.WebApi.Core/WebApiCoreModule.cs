using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using ABP.WebApi.Configuration;
using ABP.WebApi.Localization;
using ABP.WebApi.Timing;
using ASF.Domain.Services;
using System.Runtime.Serialization;
using Zop.AspNetCore.Authentication.JwtBearer;

namespace ABP.WebApi
{
    public class WebApiCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;
        }

        public override void Initialize()
        {
            
            IocManager.Register<AccessTokenOptions>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<IAccessTokenGenerate, AccessTokenGenerate>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<AccountCreateService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<AccountAuthorizationService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<AccountEmailChangeService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<AccountInfoChangeService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<IAccountLoginService, AccountLoginService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<AccountPasswordChangeService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<AccountRoleAssignationService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<AccountPermissionService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<AccountTelephoneChangeService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<AccountDeleteService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<LogLoginRecordService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<LogOperateRecordService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<PermissionCreateService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<PermissionChangeService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<PermissionDeleteService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<RoleCreateService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<RoleInfoChangeService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<RolePermissionAssignationService>(Abp.Dependency.DependencyLifeStyle.Transient);
            IocManager.RegisterAssemblyByConvention(typeof(WebApiCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
