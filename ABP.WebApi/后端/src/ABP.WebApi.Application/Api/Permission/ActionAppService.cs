using Abp.Dependency;
using ABP.WebApi.Configuration;
using ABP.WebApi.Core.Repositories;
using ABP.WebApi.Entities;
using ABP.WebApi.Extensions.Values;
using ASF;
using ASF.Application.DTO;
using ASF.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using ABP.WebApi.Authorization;

namespace ABP.WebApi.Api.Permission
{
    [Authorize]
    public class ActionAppService : PermissionAppService
    {
        public ActionAppService(LogOperateRecordService operateLog, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository, IMapper mapper) :base(operateLog,unitOfWork,permissionRepository,mapper)
        {

        }
        /// <summary>
        /// 创建功能权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> Create(PermissionActionCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建功能权限
            var permission = dto.To();
            result = await IocManager.Instance.Resolve<PermissionCreateService>().CreateAction(permission);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionCreateAction, dto, "Success");  //记录日志
            await _permissionRepository.AddAsync(permission);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改功能权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> Modify(PermissionActionModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改功能权限
            var modifyResult = await IocManager.Instance.Resolve<PermissionChangeService>()
                .ModifyAction(dto.Id, dto.Name, dto.Description, dto.Enable, dto.ApiTemplate, dto.IsLogger, dto.HttpMethods.Select(f => new HttpMethod(f)).ToList());
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionModifyAction, dto, "Success");  //记录日志
            await _permissionRepository.ModifyAsync(modifyResult.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 获取功能权限集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ResultList<PermissionActionInfoDetailsResponseDto>> GetList(PermissionListRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultList<PermissionActionInfoDetailsResponseDto>.ReFailure(result);

            //获取权限数据
            var permissionList = await this._permissionRepository.GetList(dto);

            //组合响应数据
            var actionList = permissionList.Where(f => f.Type == PermissionType.Action).OrderBy(f => f.Sort).ToList();
            var actions = _mapper.Map<List<PermissionActionInfoDetailsResponseDto>>(actionList);
            return ResultList<PermissionActionInfoDetailsResponseDto>.ReSuccess(actions);

        }
        /// <summary>
        /// 根据ID获取功能权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<Result<PermissionActionInfoDetailsResponseDto>> GetDetails(string id)
        {
            // 获取权限数据
            var action = await this._permissionRepository.GetAsync(id);
            if (action == null || action.Type != PermissionType.Action)
                return Result<PermissionActionInfoDetailsResponseDto>.ReFailure(ResultCodes.PermissionNotExist);

            var result = _mapper.Map<PermissionActionInfoDetailsResponseDto>(action);
            return Result<PermissionActionInfoDetailsResponseDto>.ReSuccess(result);
        }
    }
}
