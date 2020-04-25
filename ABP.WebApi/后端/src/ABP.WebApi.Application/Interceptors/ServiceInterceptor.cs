using Abp;
using Abp.Authorization;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ABP.WebApi.Application.Interceptors
{
    /// <summary>
    /// 拦截普通方法(由于通过接口调用所以被拦截的方法必须为虚方法)
    /// </summary>
    public class ServiceInterceptor : IInterceptor
    {
        public ILogger Logger { get; set; }
        public ServiceInterceptor()
        {
            Logger = NullLogger.Instance;
        }

        public void Intercept(IInvocation invocation)
        {
            //方法调用之前,在这里甚至可以更改方法的返回值类型,参数等信息
            var type = invocation.TargetType;
            var parametersdic = new Dictionary<string, object>();
            var stopwatch = Stopwatch.StartNew();
            //调用方法
            invocation.Proceed();
            //调用方法后,
            stopwatch.Stop();
            Logger.InfoFormat(
                "Interceptor: {0} executed in {1} milliseconds; Arguments is {2}",
                invocation.MethodInvocationTarget.Name,
                stopwatch.Elapsed.TotalMilliseconds.ToString("0.000"),
                JsonConvert.SerializeObject(parametersdic)
                );
        }
    }
}
