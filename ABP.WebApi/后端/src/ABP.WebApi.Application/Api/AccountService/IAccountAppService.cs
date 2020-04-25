using ABP.WebApi.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABP.WebApi.Api.AccountService
{
    public interface IAccountAppService
    {
        Task<Result<AccountInfoBaseResponseDto>> Get();
    }
}
