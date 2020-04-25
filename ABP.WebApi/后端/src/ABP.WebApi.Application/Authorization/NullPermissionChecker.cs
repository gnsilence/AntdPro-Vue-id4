using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABP.WebApi.Authorization
{
    public sealed class NullPermissionChecker : IPermissionChecker
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullPermissionChecker Instance { get; } = new NullPermissionChecker();

        public Task<bool> IsAuthenticatedAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsGrantedAsync(string permissionName)
        {
            return Task.FromResult(true);
        }
    }
}
