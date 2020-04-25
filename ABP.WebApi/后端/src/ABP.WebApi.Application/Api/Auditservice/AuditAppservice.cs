using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Domain.Repositories;
using ABP.WebApi.Api.Auditservice.Dtos;
using ABP.WebApi.Authorization;
using ABP.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABP.WebApi.Api.Auditervice
{

    /// <summary>
    /// 审计日志
    /// </summary>
    [Authorize]
    public class AuditAppService : WebApiAppServiceBase
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        public AuditAppService(IRepository<AuditLog, long> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }
        /// <summary>
        /// 获取审计日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<PagedResultDto<AuditLog>> GetAuditLogsPagelist(PagedSortedRequestInput<SearchDto> sortedRequestInput)
        {
            var auditlist = _auditLogRepository.GetAll();
            if (auditlist.Any()&&sortedRequestInput.Query!=null)
            {
                switch (sortedRequestInput.Query)
                {
                    case var tm when tm.BeginTime.HasValue && tm.EndTime.HasValue:
                        var begintime = sortedRequestInput.Query.BeginTime.Value;
                        var endtime = sortedRequestInput.Query.EndTime.Value.AddDays(1);
                        auditlist = auditlist.Where(p => p.ExecutionTime >= begintime && p.ExecutionTime < endtime);
                        break;
                    case var tm when tm.BeginTime.HasValue && !tm.EndTime.HasValue:
                        var time = sortedRequestInput.Query.BeginTime.Value.AddDays(-1);
                        auditlist = auditlist.Where(p => p.ExecutionTime > time);
                        break;
                    case var tm when !tm.BeginTime.HasValue && tm.EndTime.HasValue:
                        var btime = sortedRequestInput.Query.EndTime.Value.AddDays(1);
                        auditlist = auditlist.Where(p => p.ExecutionTime < btime);
                        break;
                    case var tm when tm.HasException:
                        auditlist = auditlist.Where(p => p.Exception!=null);
                        break;
                    case var tm when !string.IsNullOrEmpty(tm.IPAddress):
                        auditlist = auditlist.Where(p => p.ClientIpAddress==tm.IPAddress);
                        break;
                    case var tm when !string.IsNullOrEmpty(tm.Account):
                        auditlist = auditlist.Where(p => p.ReturnValue.Contains(tm.Account));
                        break;
                    case var tm when !string.IsNullOrEmpty(tm.MethodName):
                        auditlist = auditlist.Where(p => p.MethodName==tm.MethodName);
                        break;
                }
            }
            var reslut = await auditlist.GetPageSortListAsync(sortedRequestInput, t => new AuditLog()
            {
                BrowserInfo = t.BrowserInfo,
                ExecutionTime = t.ExecutionTime,
                Exception = t.Exception,
                ClientName = t.ClientName,
                ClientIpAddress = t.ClientIpAddress,
                ExecutionDuration = t.ExecutionDuration,
                ReturnValue = t.ReturnValue,
                MethodName = t.MethodName,
                ServiceName = t.ServiceName,
                Parameters = t.Parameters,
                CustomData = t.CustomData,
                Id=t.Id
            });
            return reslut;
        }
        /// <summary>
        /// 删除指定日期的日志
        /// </summary>
        /// <param name="begintime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task DeleteLogs(SearchDto searchDto)
        {
            await _auditLogRepository.DeleteAsync(p => p.ExecutionTime > searchDto.BeginTime.Value.AddDays(-1)&&p.ExecutionTime< searchDto.EndTime.Value.AddDays(1));
        }
        /// <summary>
        /// 获取日志详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<AuditLog> GetAuditLogAsync(long id)
        {
            return await _auditLogRepository.GetAsync(id);
        }
    }
}
