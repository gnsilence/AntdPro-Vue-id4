using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using ABP.WebApi.Api.AccountService;
using ABP.WebApi.Api.Auditervice;
using ABP.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABP.WebApi
{
    /// <summary>
    /// 可以处理审计日志,比如更改存储方式
    /// </summary>
    public class WebapiAuditStore : IAuditingStore, ITransientDependency
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        public WebapiAuditStore(IRepository<AuditLog, long> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public void Save(AuditInfo auditInfo)
        {

        }

        public async Task SaveAsync(AuditInfo auditInfo)
        {
            // 记录操作人id
            auditInfo.ReturnValue = AuthContextService.CurrentUser.DisplayName;
            await _auditLogRepository.InsertAsync(AuditLog.CreateFromAuditInfo(auditInfo));
        }
    }
}

