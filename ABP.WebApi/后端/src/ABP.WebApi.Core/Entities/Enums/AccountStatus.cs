using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.Entities.Enums
{
    /// <summary>
    /// 账户状态
    /// </summary>
    public enum AccountStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 不允许登录
        /// </summary>
        NotAllowedLogin = 2

    }
}
