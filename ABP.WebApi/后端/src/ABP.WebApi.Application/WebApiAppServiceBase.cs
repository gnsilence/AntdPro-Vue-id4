using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using System.Collections.Concurrent;
using Abp.Dependency;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace ABP.WebApi
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class WebApiAppServiceBase : ApplicationService, ITransientDependency
    {

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        [NonAction]
        public virtual OkObjectResult Ok( object value)
            => new OkObjectResult(value);
    }
}
