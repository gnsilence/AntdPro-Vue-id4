using Hangfire.HttpJob.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.Api.Jobs.Dtos
{
    public class jobdto : RecurringJobItem
    {
        public DateTime? LastExecution { get; set; }
        public DateTime? NextExecution { get; set; }
        public string LastJobId { get; set; }
        public string LastJobState { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool Removed { get; set; }
        public string TimeZoneId { get; set; }
    }
}
