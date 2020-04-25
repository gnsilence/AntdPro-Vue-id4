
using System;
using System.Collections.Generic;

namespace ABP.WebApi.Auth
{
    /// <summary>
    /// 登录用户上下文
    /// </summary>
    public class AuthContextUser
    {
        /// <summary>
        /// 用户GUID
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avator { get; set; }

        //public int Id { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public IReadOnlyList<string> Roles { get; set; }
    }
}