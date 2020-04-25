using Abp.Domain.Repositories;
using ABP.WebApi.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.GrpcService
{
    [Authorize]
    public class DemoAppService : WebApiAppServiceBase, IDemoAppService
    {
        public void Dowork()
        {
            throw new NotImplementedException();
        }
    }
}
