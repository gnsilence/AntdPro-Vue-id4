using Abp.Dependency;
using ABP.WebApi.Authorization;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ABP.WebApi
{
    [Authorize]
    public class Id4AuthAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public virtual bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
