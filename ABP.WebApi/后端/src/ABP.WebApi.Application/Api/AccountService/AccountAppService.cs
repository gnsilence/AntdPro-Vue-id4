using Abp.Domain.Repositories;
using ABP.WebApi.Authorization;
using ASF.Application.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABP.WebApi.Entities;
using ABP.WebApi.DTO;
using ABP.WebApi.Core.Repositories;
using AutoMapper;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ABP.WebApi.Auth;
using Abp.Dependency;
using ASF.Domain.Services;
using ASF;
using ABP.WebApi.Configuration;
using ABP.WebApi.Extensions.Values;
using ASF.Domain.Entities;

namespace ABP.WebApi.Api.AccountService
{
    [Authorize]
    public class AccountAppService : WebApiAppServiceBase, IAccountAppService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LogOperateRecordService _operateLog;
        public AccountAppService(IAccountRepository accountRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnitOfWork unitOfWork, LogOperateRecordService operateLog)
        {
            _accountRepository = accountRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _operateLog = operateLog;
        }
        [HttpGet]
        public virtual async Task<Result<AccountInfoBaseResponseDto>> Get()
        {
            var account = await _accountRepository.GetAsync(0);
            if (account == null)
                return Result<AccountInfoBaseResponseDto>.ReFailure(ResultCodes.AccountNotExist);

            var result = _mapper.Map<AccountInfoDetailsResponseDto>(account);
            return Result<AccountInfoBaseResponseDto>.ReSuccess(result);
        }
        [HttpGet]
        public virtual void Testuid()
        {
            var rlist = new List<List<string>>();
            var list = new List<string>() { "age", "name" };
            var list2 = new List<string>() { "22", "11111" };
            var list3 = new List<string>() { "82", "333" };
            var list4 = new List<string>() { "92", "5555" };
            rlist.Add(list2);
            rlist.Add(list3);
            rlist.Add(list4);
            var data = ConvertArrayToJson(list.ToList(), rlist);
        }
        private static String ConvertArrayToJson(List<string> h, List<List<string>> d)
        {
            return JsonConvert.SerializeObject(d.Select(x => ConvertTodic(h, x)));
        }

        private static Dictionary<string, string> ConvertTodic(List<string> h, List<string> d)
        {
            return h.ToDictionary(x => x, x => d[h.IndexOf(x)]);
        }
        /// <summary>
        /// 记录id的日志
        /// </summary>
        /// <param name="optype"></param>
        /// <param name="logtxt"></param>
        [HttpPost]
        public void AddIdsLog(string optype,string logtxt)
        {
            _operateLog.Record(ASFPermissions.AddId4Log, optype, logtxt);
        }
        /// <summary>
        /// 个人修改登录密码
        /// </summary>
        /// <param name="dto">修改登录密码</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> ModifyPassword(AccountModifyPasswordRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            int id = 0;
            var service = IocManager.Instance.Resolve<AccountPasswordChangeService>();
            var modifyResult = await service.Modify(id, dto.Password, dto.OldPassword);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyPassword, new { uid = id, data = dto }, "Success");  //记录日志
            await _accountRepository.ModifyAsync(modifyResult.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 个人修改邮箱地址
        /// </summary>
        /// <param name="dto">邮箱地址</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> ModifyEmail(AccountModifyEmailRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            int id = 0;
            var service = IocManager.Instance.Resolve<AccountEmailChangeService>();
            var modifyResult = service.Modify(id, dto.Email);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyEmail, new { uid = id, data = dto }, "Success");  //记录日志
            await _accountRepository.ModifyAsync(modifyResult.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 个人修改电话号码
        /// </summary>
        /// <param name="dto">电话号码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> ModifyTelephone(AccountModifyTelephoneRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //调用服务修改电话号码
            int id = 0;
            var service = IocManager.Instance.Resolve<AccountTelephoneChangeService>();
            var modifyResult = service.Modify(id, new PhoneNumber(dto.Number, dto.AreaCode));
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyTelephone, new { uid = id, data = dto }, "Success");  //记录日志
            await _accountRepository.ModifyAsync(modifyResult.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 个人修改昵称或者头像
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> ModifyNameOrAvatar(AccountModifyNameOrAvatarRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改昵称和头像
            int id = 0;
            var modifyResult = await IocManager.Instance.Resolve<AccountInfoChangeService>()
                .ModifyNameOrAvatar(id, dto.Name, dto.Avatar);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyInfo, new { uid = id, data = dto }, "Success");  //记录日志
            await _accountRepository.ModifyAsync(modifyResult.Data);
            return Result.ReSuccess();

        }


        /// <summary>
        /// 登录获取用户信息和权限信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<Result<AccountInfoByLoginResponseDto>> Info()
        {
            //int uid = AuthContextService.CurrentUser.Id;
            if (AuthContextService.CurrentUser.Roles.Count() == 0)
                return Result<AccountInfoByLoginResponseDto>.ReFailure(ResultCodes.AccountNotExist);
            var result = await IocManager.Instance.Resolve<AccountPermissionService>()
              .GetPermissions(AuthContextService.CurrentUser.Roles);
            if (result.Account == null)
                return Result<AccountInfoByLoginResponseDto>.ReFailure(ResultCodes.AccountNotExist);

            AccountInfoByLoginResponseDto responseDto = new AccountInfoByLoginResponseDto(result.Account);
            if (result.Permissions.Count == 0)
                return Result<AccountInfoByLoginResponseDto>.ReSuccess(responseDto);

            //组装响应数据
            responseDto.SetMenus(result.Permissions);
            return Result<AccountInfoByLoginResponseDto>.ReSuccess(responseDto);
        }

        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="dto">账号对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create(AccountCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建服务
            var account = _mapper.Map<Account>(dto);
            var service = IocManager.Instance.Resolve<AccountCreateService>();
            result = await service.Create(account);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountCreate, dto, "Success");  //记录日志
            await _accountRepository.AddAsync(account);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 删除账号
        /// </summary>
        /// <param name="id">账号标识</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Delete(int id)
        {
            //删除账户
            var result = await IocManager.Instance.Resolve<AccountDeleteService>().Delete(id);
            if (!result.Success)
                return result;

            _operateLog.Record(ASFPermissions.AccountDelete, new { uid = id }, "Success");  //记录日志
            await _accountRepository.ModifyAsync(result.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="dto">账户信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Midify(AccountModifyInfoRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //调用服务修改账户数据
            var service = IocManager.Instance.Resolve<AccountInfoChangeService>();
            var modifyResult = await service.Modify(dto.AccountId, dto.Name, dto.Status, dto.Roles);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyStatus, dto, "Success");  //记录日志
            await _accountRepository.ModifyAsync(modifyResult.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改账号状态
        /// </summary>
        /// <param name="dto">修改状态请求</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> MidifyStatus(AccountModifyStatusRequestDto dto)
        {
            var service = IocManager.Instance.Resolve<AccountInfoChangeService>();
            var result = await service.ModifyStatus(dto.Id, dto.Status);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountModifyInfo, dto, "Success");  //记录日志
            await _accountRepository.ModifyAsync(result.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 设置登录密码
        /// </summary>
        /// <param name="dto">设置登录密码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> ResetPassword(AccountResetPasswordRequestDto dto)
        {
            var service = IocManager.Instance.Resolve<AccountPasswordChangeService>();
            var result = await service.Reset(dto.Id, dto.Password, 0, dto.AdminPassword);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.AccountSetPassword, dto, "Success");  //记录日志
            await _accountRepository.ModifyAsync(result.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 获取账户集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultPagedList<AccountInfoBaseResponseDto>> GetList(AccountListPagedRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultPagedList<AccountInfoBaseResponseDto>.ReFailure(result);

            //获取用户数据
            var accountsResult = await _accountRepository.GetList(dto);
            var accounts = accountsResult.Accounts;
            if (accounts.Count == 0)
                return ResultPagedList<AccountInfoBaseResponseDto>.ReSuccess();

            //获取角色数据
            var rids = new List<int>();
            accounts
                .Select(f => f.Roles.ToList()).ToList()
                .ForEach(p =>
                {
                    rids.AddRange(p);
                });
            var roles = await IocManager.Instance.Resolve<IRoleRepository>().GetList(rids);

            //组装响应数据
            var accountInfos = _mapper.Map<List<AccountInfoBaseResponseDto>>(accounts);
            accountInfos.ForEach(ainfo =>
            {
                var account = accounts.FirstOrDefault(a => a.Id == ainfo.Id);
                ainfo.Roles = roles
                    .Where(r => r.IsNormal() && account.Roles.Contains(r.Id))
                    .Select(r => r.Name)
                    .ToList();
            });
            return ResultPagedList<AccountInfoBaseResponseDto>.ReSuccess(accountInfos, accountsResult.TotalCount);
        }
        /// <summary>
        /// 获取账号详细信息
        /// </summary>
        /// <param name="id">账号ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<AccountInfoDetailsResponseDto>> GetDetails(int id)
        {
            var account = await _accountRepository.GetAsync(id);
            if (account == null)
                return Result<AccountInfoDetailsResponseDto>.ReFailure(ResultCodes.AccountNotExist);

            //获取创建用户
            var createAccount = await this._accountRepository.GetAsync(0);

            //组装响应数据
            var result = _mapper.Map<AccountInfoDetailsResponseDto>(account);
            if (account.IsSuperAdministrator())
            {
                result.Roles.Clear();
            }
            if (createAccount != null)
            {
                result.CreateUser = createAccount.Name;
            }
            return Result<AccountInfoDetailsResponseDto>.ReSuccess(result);
        }
    }
}
