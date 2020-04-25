using Abp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.GrpcService
{
   public interface IDemoAppService: IApplicationService
    {
        void Dowork();
    }
}
