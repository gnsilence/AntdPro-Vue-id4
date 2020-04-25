using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuditLogView.GrpcService.Dtos
{
    [MessagePackObject(true)]
    public class AuditLogDto
    {
        public long Id { get; set; }
        //
        // 摘要:
        //     Start time of the method execution.
        public virtual DateTime ExecutionTime { get; set; }
        //
        // 摘要:
        //     Exception object, if an exception occured during execution of the method.
        public virtual string Exception { get; set; }
        //
        // 摘要:
        //     Browser information if this method is called in a web request.
        public virtual string BrowserInfo { get; set; }
        //
        // 摘要:
        //     Name (generally computer name) of the client.
        public virtual string ClientName { get; set; }
        //
        // 摘要:
        //     IP address of the client.
        public virtual string ClientIpAddress { get; set; }
        //
        // 摘要:
        //     Total duration of the method call as milliseconds.
        public virtual int ExecutionDuration { get; set; }
        //
        // 摘要:
        //     Return values.
        public virtual string ReturnValue { get; set; }
        //
        // 摘要:
        //     Executed method name.
        public virtual string MethodName { get; set; }
        //
        // 摘要:
        //     Service (class/interface) name.
        public virtual string ServiceName { get; set; }
        //
        // 摘要:
        //     Calling parameters.
        public virtual string Parameters { get; set; }
        //
        // 摘要:
        //     Abp.Auditing.AuditInfo.CustomData.
        public virtual string CustomData { get; set; }
    }
}
