using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.Authorization
{
    public interface IAuthorizeAttribute
    {
        /// <summary>
        /// 权限
        /// </summary>
        string[] Permissions { get; }

        bool RequireAllPermissions { get; set; }
    }
}
