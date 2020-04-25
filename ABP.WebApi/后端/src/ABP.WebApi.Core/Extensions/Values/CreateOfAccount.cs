using ABP.WebApi.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ABP.WebApi.Extensions.Values
{
    /// <summary>
    /// 创建的账户信息
    /// </summary>
    public class CreateOfAccount : IValueObject
    {
        private CreateOfAccount()
        {

        }
        public CreateOfAccount(ASF.Domain.Entities.Account account)
        {
     
        }

        public CreateOfAccount(string createOfAccountId)
        {
            this.CreateId = createOfAccountId;
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        [Range(1,int.MaxValue)]
        public string CreateId { get; private set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; } = DateTime.Now;
    }
}
