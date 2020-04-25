using AntProVue.Admin.EntityFramework.Shared.Entities.Identity;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AntProVue.STS.Identity.Services
{
    /// <summary>
    /// 映射用户自定义字段
    /// </summary>
    public class ProfileServices : IProfileService
    {
        private readonly UserManager<UserIdentity> _userManager;
        public ProfileServices(UserManager<UserIdentity> userManager)
        {
            _userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);
            context.IssuedClaims = await GetClaimsFromUserAsync(user);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);
            context.IsActive = user != null ? true : false;
           //context.IsActive = user?.; //该用户是否已经激活
        }
        public async Task<List<Claim>> GetClaimsFromUserAsync(UserIdentity user)
        {
            var userclaims = await _userManager.GetClaimsAsync(user);
            var claims = new List<Claim> {
                new Claim(JwtClaimTypes.Subject,user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName,user.UserName),
                new Claim("displayname",user.UserName),
                new Claim("email",user.Email),
               //new Claim("avator",user.Avatar??""),
                new Claim("userid",user.Id),
                new Claim("username",user.UserName)

            };

            claims.AddRange(userclaims);

            var role = await _userManager.GetRolesAsync(user);
            role.ToList().ForEach(f =>
            {
                claims.Add(new Claim(JwtClaimTypes.Role, f));
            });

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                claims.Add(new Claim("Phone", user.PhoneNumber ?? ""));
            }
            return claims;
        }
    }
}
