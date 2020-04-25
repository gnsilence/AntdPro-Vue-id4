using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABP.WebApi.Authorization
{
  public  interface IPermissionChecker
    {
        Task<bool> IsAuthenticatedAsync();

        Task<bool> IsGrantedAsync(string permissionName);
    }
}
