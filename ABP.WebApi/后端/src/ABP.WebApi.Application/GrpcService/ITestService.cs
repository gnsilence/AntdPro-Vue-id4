using Abp.Auditing;
using ABP.WebApi.GrpcService.Dtos;
using MagicOnion;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.GrpcService
{
   public interface ITestService: IService<ITestService>
    {
        /// <summary>
        /// grpc服务
        /// </summary>
        /// <returns></returns>
        UnaryResult<string> GetTestData();

        UnaryResult<List<AuditLogDto>> GetAuditLogs();
    }
}
