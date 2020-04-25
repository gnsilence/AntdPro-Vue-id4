using Abp.Dependency;
using Castle.Core;
using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ABP.WebApi.Authorization
{
    internal static class AuthorizationInterceptorRegistrar
    {
        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private static void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            if (ShouldIntercept(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AuthorizationInterceptor)));
            }
        }

        /// <summary>
        /// 需要拦截的特性类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool ShouldIntercept(Type type)
        {
            if (SelfOrMethodsDefinesAttribute<AuthorizeAttribute>(type))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否使用某个特性
        /// </summary>
        /// <typeparam name="TAttr"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool SelfOrMethodsDefinesAttribute<TAttr>(Type type)
        {
            if (type.GetTypeInfo().IsDefined(typeof(TAttr), true))
            {
                return true;
            }

            return type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(m => m.IsDefined(typeof(TAttr), true));
        }
    }
}
