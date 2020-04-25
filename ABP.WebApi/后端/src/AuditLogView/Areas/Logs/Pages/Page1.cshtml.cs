using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Grpc.Client.Utility;
using AuditLogView.GrpcService;
using AuditLogView.GrpcService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditLogView.Logs.Pages
{
    public class Page1Model : PageModel
    {
        private readonly IGrpcConnectionUtility _connectionUtility;
        public Page1Model(IGrpcConnectionUtility connectionUtility)
        {
            _connectionUtility = connectionUtility;
        }
        /// <summary>
        /// get logs
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            var service = _connectionUtility.GetRemoteServiceForDirectConnection<ITestService>("TestServiceName");
            auditLogDtos = await service.GetAuditLogs();
        }

        [BindProperty]
        public List<AuditLogDto> auditLogDtos { get; set; }
    }
}