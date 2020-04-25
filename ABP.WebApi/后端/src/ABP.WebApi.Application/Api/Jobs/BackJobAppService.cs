using ABP.WebApi.Api.Jobs.Dtos;
using ABP.WebApi.Authorization;
using Hangfire;
using Hangfire.HttpJob.Server;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Hangfire.Common;

namespace ABP.WebApi.Api.Jobs
{
    [Authorize]
    public class BackJobAppService : WebApiAppServiceBase
    {
        /// <summary>
        /// 添加一个队列任务,立即执行
        /// </summary>
        /// <param name="httpJob">httpjob</param>
        /// <returns></returns>
        [HttpPost]
        public virtual ReturnMsg AddBackGroundJob(Hangfire.HttpJob.Server.HttpJobItem httpJob)
        {
            var addreslut = string.Empty;
            try
            {
                addreslut = BackgroundJob.Enqueue(() => Hangfire.HttpJob.Server.HttpJob.Excute(httpJob, httpJob.JobName, httpJob.QueueName, httpJob.IsRetry, null));
            }
            catch (Exception ec)
            {
                return new ReturnMsg() { Errorcode = "0", Message = $"添加队列任务失败,{ec.Message}" };
            }
            return new ReturnMsg() { Errorcode = "1", Message = "添加队列任务成功!!" };
        }
        /// <summary>
        /// 添加一个周期任务
        /// </summary>
        /// <param name="httpJob"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ReturnMsg AddOrUpdateRecurringJob(Hangfire.HttpJob.Server.HttpJobItem httpJob)
        {
            try
            {
                RecurringJob.AddOrUpdate(httpJob.JobName, () => Hangfire.HttpJob.Server.HttpJob.Excute(httpJob, httpJob.JobName, httpJob.QueueName, httpJob.IsRetry, null), httpJob.Corn, TimeZoneInfo.Local);
            }
            catch (Exception ec)
            {
                return new ReturnMsg() { Errorcode = "0", Message = $"添加周期任务失败,{ec.Message}" };
            }
            return new ReturnMsg() { Errorcode = "1", Message = "添加周期任务成功!!" };
        }
        /// <summary>
        /// 删除周期任务
        /// </summary>
        /// <param name="jobname">任务名称</param>
        /// <returns></returns>
        [HttpGet]
        public virtual ReturnMsg DeleteJob(string jobname)
        {
            try
            {
                RecurringJob.RemoveIfExists(jobname);
            }
            catch (Exception ec)
            {
                return new ReturnMsg() { Errorcode = "0", Message = $"删除周期任务失败,{ec.Message}" };
            }
            return new ReturnMsg() { Errorcode = "1", Message = "删除周期任务成功!!" };
        }
        /// <summary>
        /// 手动触发一个定时任务,立即执行
        /// </summary>
        /// <param name="jobname">任务名称</param>
        /// <returns></returns>
        [HttpGet]
        public virtual ReturnMsg TriggerRecurringJob(string jobname)
        {
            try
            {
                RecurringJob.Trigger(jobname);
            }
            catch (Exception ec)
            {
                return new ReturnMsg() { Errorcode = "0", Message = $"触发任务失败,{ec.Message}" };
            }
            return new ReturnMsg() { Errorcode = "1", Message = "触发任务成功!!" };
        }
        /// <summary>
        /// 添加一个延迟任务
        /// </summary>
        /// <param name="httpJob"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ReturnMsg AddScheduleJob(Hangfire.HttpJob.Server.HttpJobItem httpJob)
        {
            var reslut = string.Empty;
            try
            {
                reslut = BackgroundJob.Schedule(() => Hangfire.HttpJob.Server.HttpJob.Excute(httpJob, httpJob.JobName, httpJob.QueueName, httpJob.IsRetry, null), TimeSpan.FromMinutes(httpJob.DelayFromMinutes));
            }
            catch (Exception ec)
            {
                return new ReturnMsg() { Errorcode = "0", Message = $"添加延迟任务失败,{ec.Message}" };
            }
            return new ReturnMsg() { Errorcode = "1", Message = "添加延迟任务成功!!" };
        }
        /// <summary>
        /// 获取所有定时任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ResultPagedList<jobdto>> GetJobItems(JobListPagedRequestDto listPagedRequestDto)
        {
            var joblist = new List<jobdto>();
            var totalcount = 0;
            using (var connection = JobStorage.Current.GetConnection())
            {
                var RecurringJob = StorageConnectionExtensions.GetRecurringJobs(connection);
                if (RecurringJob != null)
                {
                    RecurringJob.ForEach(job =>
                    {
                        var jobitem = JsonConvert.DeserializeObject<RecurringJobItem>(job.Job.Args.FirstOrDefault().ToString());
                        var jobmodel = new jobdto();
                        jobmodel.BasicPassword = jobitem.BasicPassword;
                        jobmodel.BasicUserName = jobitem.BasicUserName;
                        jobmodel.ContentType = jobitem.ContentType;
                        jobmodel.Corn = jobitem.Corn;
                        jobmodel.CreatedAt = job.CreatedAt;
                        jobmodel.Data = jobitem.Data;
                        jobmodel.IsRetry = jobitem.IsRetry;
                        jobmodel.JobName = jobitem.JobName;
                        jobmodel.LastExecution = job.LastExecution;
                        jobmodel.LastJobState = job.LastJobState;
                        jobmodel.NextExecution = job.NextExecution;
                        jobmodel.QueueName = jobitem.QueueName;
                        jobmodel.Removed = job.Removed;
                        jobmodel.Timeout = jobitem.Timeout;
                        jobmodel.LastJobId = job.LastJobId;
                        jobmodel.Method = jobitem.Method;
                        jobmodel.Url = jobitem.Url;
                        joblist.Add(jobmodel);
                    });
                    totalcount = joblist.Count();
                    if (!string.IsNullOrEmpty(listPagedRequestDto.Vague))
                    {
                        joblist = joblist.Where(p => p.JobName.Contains(listPagedRequestDto.Vague) || p.QueueName.Contains(listPagedRequestDto.Vague) || (p.Method == listPagedRequestDto.Vague)).ToList();
                        totalcount = joblist.Count();
                    }
                    var jobreslut = joblist.OrderByDescending(p => p.QueueName);
                    joblist = jobreslut.Skip((listPagedRequestDto.SkipPage - 1) * listPagedRequestDto.PagedCount).Take(listPagedRequestDto.PagedCount).ToList();
                    return ResultPagedList<jobdto>.ReSuccess(joblist, totalcount);
                }
            }
            return new ResultPagedList<jobdto>();
        }
        /// <summary>
        /// 获取任务状态信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual MonitoringApi GetMonitoringApi()
        {
            var morapi = JobStorage.Current.GetMonitoringApi().GetStatistics();
            var mormodel = new MonitoringApi()
            {
                DeletedJobs = morapi.Deleted.ToString(),
                SucceededListCount = morapi.Succeeded.ToString(),
                FailedCount = morapi.Failed.ToString(),
                ProcessingCount = morapi.Processing.ToString(),
                ScheduledCount = morapi.Scheduled.ToString(),
                EnqueuedCount=morapi.Enqueued.ToString(),
                Queues=morapi.Queues.ToString()

            };
            return mormodel;
        }
        /// <summary>
        /// 根据名称获取任务
        /// </summary>
        /// <param name="jobname">任务名称</param>
        /// <returns></returns>
        [HttpGet]
        public virtual RecurringJobItem GetHttpJobItem(string jobname)
        {
            var RecurringJob = JobStorage.Current.GetConnection().GetRecurringJobs().
                    Where(p => p.Id == jobname).FirstOrDefault();
            if (RecurringJob != null)
            {
                return JsonConvert.DeserializeObject<RecurringJobItem>(RecurringJob.Job.Args.FirstOrDefault().ToString());
            }
            return new RecurringJobItem();
        }
    }
}
