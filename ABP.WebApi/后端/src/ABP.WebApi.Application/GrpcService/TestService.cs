using Abp.Auditing;
using Abp.Domain.Repositories;
using ABP.WebApi.GrpcService.Dtos;
using MagicOnion;
using MagicOnion.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ABP.WebApi.GrpcService
{
    public class TestService : ServiceBase<ITestService>, ITestService
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        public TestService(IRepository<AuditLog, long> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }
        public UnaryResult<List<AuditLogDto>> GetAuditLogs()
        {
            var datalist = _auditLogRepository.GetAllList(p=>p.ExecutionTime.ToShortDateString().Equals(DateTime.Now.ToShortDateString()));
            var query = (from t in datalist
                         select new AuditLogDto()
                         {
                             BrowserInfo = t.BrowserInfo,
                             ExecutionTime = t.ExecutionTime,
                             Exception = t.Exception,
                             ClientName = t.ClientName,
                             ClientIpAddress = t.ClientIpAddress,
                             ExecutionDuration=t.ExecutionDuration,
                             ReturnValue=t.ReturnValue,
                             MethodName=t.MethodName,
                             ServiceName=t.ServiceName,
                             Parameters=t.Parameters,
                             CustomData=t.CustomData
                         }).ToList();
            return new UnaryResult<List<AuditLogDto>>(query);
        }

        public UnaryResult<string> GetTestData()
        {
            return new UnaryResult<string>("测试");
        }
    }
}
