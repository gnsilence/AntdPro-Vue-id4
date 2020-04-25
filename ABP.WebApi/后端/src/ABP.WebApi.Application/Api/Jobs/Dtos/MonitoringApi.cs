using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.Api.Jobs.Dtos
{
    public class MonitoringApi
    {
        /// <summary>
        /// 已经删除的
        /// </summary>
        public string DeletedJobs { get; set; }
        /// <summary>
        /// 入队的作业
        /// </summary>
        public string EnqueuedCount { get; set; }
        /// <summary>
        /// 失败数
        /// </summary>
        public string FailedCount { get; set; }
        /// <summary>
        /// 执行中的作业
        /// </summary>
        public string ProcessingCount { get; set; }
        /// <summary>
        /// 等待中的作业
        /// </summary>
        public string ScheduledCount { get; set; }
        /// <summary>
        /// 执行成功的作业
        /// </summary>
        public string SucceededListCount { get; set; }
        /// <summary>
        /// 队列数
        /// </summary>
        public string Queues { get; set; }
    }
}
