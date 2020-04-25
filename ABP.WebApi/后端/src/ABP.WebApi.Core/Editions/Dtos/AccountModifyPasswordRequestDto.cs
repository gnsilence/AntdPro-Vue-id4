using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ABP.WebApi.DTO
{
    /// <summary>
    /// 账户修改登录密码
    /// </summary>
    public class AccountModifyPasswordRequestDto
    {
        /// <summary>
        /// 旧登录密码
        /// </summary>
        [Required, StringLength(32, MinimumLength = 6)]
        public string OldPassword { get;  set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required, StringLength(32, MinimumLength = 6)]
        public string Password { get;  set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
