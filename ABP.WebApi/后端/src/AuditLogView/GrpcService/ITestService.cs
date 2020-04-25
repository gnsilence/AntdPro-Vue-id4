using AuditLogView.GrpcService.Dtos;
using MagicOnion;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuditLogView.GrpcService
{
  public interface ITestService : IService<ITestService>
    {
        UnaryResult<string> GetTestData();
        UnaryResult<List<AuditLogDto>> GetAuditLogs();
    }
}
