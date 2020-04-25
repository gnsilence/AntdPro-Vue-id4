
using Abp.Dependency;
using ABP.WebApi.Core.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace ABP.WebApi.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthContextService
    {
        private static IHttpContextAccessor _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
        }
        /// <summary>
        /// 
        /// </summary>
        public static HttpContext Current => _context.HttpContext;
        /// <summary>
        /// 
        /// </summary>
        public static AuthContextUser CurrentUser
        {
            get
            {
                var user = new AuthContextUser
                {
                    DisplayName = Current.User.FindFirstValue("username"),
                    EmailAddress = Current.User.FindFirstValue("emailAddress"),
                    Avator = Current.User.FindFirstValue("avator"),
                    Guid =Current.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    //Id = Current.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Roles = Current.User.FindAll(p => p.Type == ClaimTypes.Role).Select(p=>p.Value).ToList() 
                };
                return user;
            }
        }

        /// <summary>
        /// 是否已授权
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return Current.User.Identity.IsAuthenticated;
            }
        }

        private static int GetUserid(string guid)
        {
            try
            {
                var id = IocManager.Instance.Resolve<IAccountRepository>().GetIdByUserGuidAsync(guid);
                return id;
            }
            catch (Exception ee)
            {

                return 0;
            }

        }
    }
}
