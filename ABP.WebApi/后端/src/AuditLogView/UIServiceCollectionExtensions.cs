using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuditLogView
{
  public static class UIServiceCollectionExtensions
    {
        public static void AddLogUI(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(LogConfigureOptions));
        }
    }
}
