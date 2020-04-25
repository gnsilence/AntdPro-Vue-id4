using ABP.WebApi.Entities.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ABP.WebApi.DTO
{
    /// <summary>
    /// 修改账户状态
    /// </summary>
    public class AccountModifyStatusRequestDto
    {
        /// <summary>
        /// 账户标识
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 账户状态
        /// </summary>
        public AccountStatus Status { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
