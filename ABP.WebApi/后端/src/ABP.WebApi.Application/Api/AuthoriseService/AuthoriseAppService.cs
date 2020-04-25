using Abp.Dependency;
using ABP.WebApi.Core.Repositories;
using ABP.WebApi.DTO;
using ASF;
using ASF.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zop.AspNetCore.Authentication.JwtBearer;

namespace ABP.WebApi.Api.AuthoriseService
{
    public class AuthoriseAppService : WebApiAppServiceBase
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountLoginService _accountLoginService;
        public AuthoriseAppService(IHttpContextAccessor httpContext, IAccountRepository accountRepository, IUnitOfWork unitOfWork, IAccountLoginService accountLoginService)
        {
            _httpContext = httpContext;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _accountLoginService = accountLoginService;
        }
        /// <summary>
        /// 账户登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<AccessToken>> Login(AuthoriseByUsernameRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
            {
                _httpContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Result<AccessToken>.ReFailure(result);
            }

            //账户登录验证
            //var service = IocManager.Instance.Resolve<IAccountLoginService>();
            var ip = _httpContext.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ip))
                ip = "127.0.0.1";
            var logResult = _accountLoginService.LoginByUsername(dto.Username, dto.Password, ip);
            if (!logResult.Success)
            {
                _httpContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Result<AccessToken>.ReFailure(logResult);
            }

            //数据持久化
            await _accountRepository.ModifyAsync(logResult.Data);
            //await _unitOfWork.CommitAsync(autoRollback: true);
            return Result<AccessToken>.ReSuccess(logResult.Data.LoginInfo.AccessToken);
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result Logout()
        {
            return Result.ReSuccess();
        }
    }
}
