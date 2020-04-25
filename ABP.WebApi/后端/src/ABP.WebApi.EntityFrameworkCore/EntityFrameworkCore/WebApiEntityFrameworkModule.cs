using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.FreeSqlExtensions.FreeSqlExt;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ABP.WebApi.Core.Repositories;
using ABP.WebApi.EntityFrameworkCore.Repositories;
using ABP.WebApi.EntityFrameworkCore.Repositories.SqlService;
using ASF.EntityFramework.Repository;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ABP.WebApi.EntityFrameworkCore
{
    [DependsOn(
        typeof(WebApiCoreModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpFreeSqlExtensionsModule))]
    public class WebApiEntityFrameworkModule : AbpModule
    {
        public bool SkipDbContextRegistration { get; set; }

        // SkipRegister SqlserverContext
        public bool SkipSqlserverDbContextRegistration { get; set; } = false;
        /// <summary>   
        /// SkipRegister MysqlContext
        /// </summary>
        public bool SkipMysqlDbContextRegistration { get; set; } = true;
        /// <summary>
        /// Skip postSqlContext
        /// </summary>
        public bool SkipPostgreSqlDbContextRegistration { get; set; } = true;
        /// <summary>
        /// skip seed initdata
        /// </summary>
        public bool SkipDbSeed { get; set; } = true;

        public override void PreInitialize()
        {
            //Configuration.ReplaceService(typeof(IConnectionStringResolver), () => {
            //    IocManager.RegisterIfNot<IConnectionStringResolver, MyConnectionStringResolver>();
            //});
            Configuration.ReplaceService<IConnectionStringResolver, MyConnectionStringResolver>();

            if (!SkipDbContextRegistration)
            {
                if (!SkipSqlserverDbContextRegistration)
                {
                    Configuration.Modules.AbpEfCore().AddDbContext<WebApiDbContext>(options =>
                    {
                        if (options.ExistingConnection != null)
                        {
                            WebApiDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                        }
                        else
                        {
                            WebApiDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                        }
                    });
                }
                if (!SkipMysqlDbContextRegistration)
                {
                    //配置mysql数据库
                    Configuration.Modules.AbpEfCore().AddDbContext<JPGZServiceMysqlDbContext>(options =>
                    {
                        if (options.ExistingConnection != null)
                        {
                            JPGZServiceMysqlDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                        }
                        else
                        {
                            JPGZServiceMysqlDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                        }
                    });
                }

                if (!SkipPostgreSqlDbContextRegistration)
                {
                    //配置PostgreSql数据库
                    Configuration.Modules.AbpEfCore().AddDbContext<JPGZServicePostgreSqlDbContext>(options =>
                    {
                        if (options.ExistingConnection != null)
                        {
                            JPGZServicePostgreSqlDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                        }
                        else
                        {
                            JPGZServicePostgreSqlDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                        }
                    });
                }
            }
        }

        public override void Initialize()
        {
            IocManager.IocContainer.Register(Component.For<IAccountRepository, AccountRepository>()
    .LifestyleCustom<MsScopedLifestyleManager>());
            IocManager.IocContainer.Register(Component.For<ASF.IUnitOfWork, UnitOfWork>()
    .LifestyleCustom<MsScopedLifestyleManager>());
            IocManager.IocContainer.Register(Component.For<ILoggingRepository, LogInfoRepository>()
   .LifestyleCustom<MsScopedLifestyleManager>());
            IocManager.IocContainer.Register(Component.For<IPermissionRepository, PermissionRepository>()
   .LifestyleCustom<MsScopedLifestyleManager>());
            IocManager.IocContainer.Register(Component.For<IRoleRepository, RoleRepository>()
 .LifestyleCustom<MsScopedLifestyleManager>());
            IocManager.RegisterAssemblyByConvention(typeof(WebApiEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                //SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
