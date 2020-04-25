using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ABP.WebApi.Api.Auditservice.Dtos
{
    /// <summary>
    /// 查询
    /// </summary>
   public class SearchDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 只看异常
        /// </summary>
        public bool HasException { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }
    }
}
