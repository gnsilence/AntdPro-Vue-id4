using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute, IAuthorizeAttribute
    {
        public string[] Permissions { get; }

        public bool RequireAllPermissions { get; set; }

        public AuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }
    }
}
