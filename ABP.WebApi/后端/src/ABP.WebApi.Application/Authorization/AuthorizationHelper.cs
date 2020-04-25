using Abp.Auditing;
using Abp.Authorization;
using Abp.Dependency;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ABP.WebApi.Authorization
{
    public class AuthorizationHelper : IAuthorizationHelper, ITransientDependency
    {
        private static IHttpContextAccessor _context;

        IPermissionChecker permissionChecker { get; set; }
        public AuthorizationHelper(IHttpContextAccessor contextAccessor)
        {
            _context = contextAccessor;
            permissionChecker = NullPermissionChecker.Instance;
        }
        [DisableAuditing]
        public virtual async Task AuthorizeAsync(IEnumerable<IAuthorizeAttribute> authorizeAttributes)
        {

            if (!_context.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new AbpAuthorizationException("用户未授权");
            }

            foreach (var authorizeAttribute in authorizeAttributes)
            {
                await permissionChecker.AuthorizeAsync(authorizeAttribute.RequireAllPermissions, authorizeAttribute.Permissions);
            }
        }
        public virtual async Task AuthorizeAsync(MethodInfo methodInfo, Type type)
        {
            await CheckPermissions(methodInfo,type);
        }
        private static bool AllowAnonymous(MemberInfo memberInfo, Type type)
        {
            return ReflectionHelper
                .GetAttributesOfMemberAndType(memberInfo, type)
                .OfType<IAbpAllowAnonymousAttribute>()
                .Any();
        }
        protected virtual async Task CheckPermissions(MethodInfo methodInfo, Type type)
        {

            if (AllowAnonymous(methodInfo, type))
            {
                return;
            }

            if (ReflectionHelper.IsPropertyGetterSetterMethod(methodInfo, type))
            {
                return;
            }

            if (!methodInfo.IsPublic && !methodInfo.GetCustomAttributes().OfType<IAuthorizeAttribute>().Any())
            {
                return;
            }

            var authorizeAttributes =
                ReflectionHelper
                    .GetAttributesOfMemberAndType(methodInfo, type)
                    .OfType<IAuthorizeAttribute>()
                    .ToArray();

            if (!authorizeAttributes.Any())
            {
                return;
            }

            await AuthorizeAsync(authorizeAttributes);
        }
    }
}
