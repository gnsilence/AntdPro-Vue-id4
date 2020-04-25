using Abp.Dependency;
using ABP.WebApi.Auth;
using ABP.WebApi.Authorization;
using ABP.WebApi.Configuration;
using ABP.WebApi.Core.Repositories;
using ABP.WebApi.Entities;
using ASF;
using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABP.WebApi.Api.RoleService
{
    [Authorize]
    public class RoleAppService : WebApiAppServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;
        private readonly LogOperateRecordService _operateLog;
        private readonly IMapper _mapper;

        public RoleAppService(IUnitOfWork unitOfWork, IRoleRepository roleRepository, LogOperateRecordService operateLog, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _operateLog = operateLog;
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> Create(RoleCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //调用领域服务
            var createResult = await IocManager.Instance.Resolve<RoleCreateService>()
                .Create(dto.Name,dto.RoleId, dto.Description,AuthContextService.CurrentUser.Guid, dto.Permissions);
            if (!createResult.Success)
                return createResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.RoleCreate, dto, "Success");  //记录日志
            await _roleRepository.AddAsync(createResult.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色标识</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> Delete(int id)
        {
            _operateLog.Record(ASFPermissions.RoleDelete, new { roleId = id }, "Success");  //记录日志
            await this._roleRepository.RemoveAsync(id);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> Modify([FromBody] RoleModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //调用领域服务修改
            var modifyResult = IocManager.Instance.Resolve<RoleInfoChangeService>()
                    .Modify(dto.RoleId, dto.Name, dto.Enable, dto.Description, dto.Permissions);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.RoleModify, dto, "Success");  //记录日志
            await _roleRepository.ModifyAsync(modifyResult.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改角色状态
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> ModifyStatus([FromBody] RoleModifyStatusRequestDto dto)
        {
            if (dto.RoleId <= 0)
                return Result.ReFailure(ResultCodes.RoleNotExist);

            //数据持久化
            _operateLog.Record(ASFPermissions.RoleModifyStatus, dto, "Success");  //记录日志
            await _roleRepository.ModifyAsync(dto.RoleId, dto.Enable);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 获取所有角色基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<ResultList<RoleInfoSimpleResponseDto>> GetListAll()
        {
            //获取角色数据
            var roelList = await this._roleRepository.GetList();
            if (roelList.Count == 0)
                return ResultList<RoleInfoSimpleResponseDto>.ReSuccess();

            var roles = _mapper.Map<IList<RoleInfoSimpleResponseDto>>(roelList.Where(f => f.Enable).ToList());
            return ResultList<RoleInfoSimpleResponseDto>.ReSuccess(roles);
        }
        /// <summary>
        /// 获取角色集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ResultPagedList<RoleInfoBaseResponseDto>> GetList([FromBody]RoleListPagedRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultPagedList<RoleInfoBaseResponseDto>.ReFailure(result);

            //获取角色
            var roelResult = await this._roleRepository.GetList(dto);
            var roles = _mapper.Map<IList<RoleInfoBaseResponseDto>>(roelResult.Roles);
            return ResultPagedList<RoleInfoBaseResponseDto>.ReSuccess(roles, roelResult.TotalCount);
        }
        /// <summary>
        /// 检查是否已经添加角色
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual bool IfExistsRole(string name)
        {
            return _roleRepository.ExistsRole(name);
        }
        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<Result<RoleInfoDetailsResponseDto>> GetDetails(int id)
        {
            var role = await this._roleRepository.GetAsync(id);
            if (role == null)
                return Result<RoleInfoDetailsResponseDto>.ReFailure(ResultCodes.RoleNotExist);

            //获取所有权限
            var permissions = await IocManager.Instance.Resolve<IPermissionRepository>()
                .GetList(role.Permissions.ToList());

            var result = _mapper.Map<RoleInfoDetailsResponseDto>(role);
            result.Permissions = permissions.OrderBy(f => f.Sort).ToDictionary(k => k.Id, v => v.Name);
            return Result<RoleInfoDetailsResponseDto>.ReSuccess(result);
        }
    }
}
