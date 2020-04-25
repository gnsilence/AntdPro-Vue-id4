using Abp.Dependency;
using ABP.WebApi.Authorization;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABP.WebApi.Api.Permission
{
    [Authorize]
  public  class MenuAppService: PermissionAppService
    {
        public MenuAppService(LogOperateRecordService operateLog, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository, IMapper mapper) : base(operateLog, unitOfWork, permissionRepository, mapper)
        {

        }
        /// <summary>
        /// 创建菜单权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> Create(PermissionMenuCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建菜单权限
            var permission = dto.To();
            result = await IocManager.Instance.Resolve<PermissionCreateService>().CreateMenu(permission);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionCreateMenu, dto, "Success");  //记录日志
            await _permissionRepository.AddAsync(permission);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改菜单权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> Modify(PermissionMenuModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改功能权限
            var modifyResult = await IocManager.Instance.Resolve<PermissionChangeService>()
                .ModifyMenu(dto.Id, dto.Name, dto.ParentId, dto.Description, dto.Enable, dto.Icon, dto.Redirect, dto.Hidden);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionModifyMenu, dto, "Success");  //记录日志
            await _permissionRepository.ModifyAsync(modifyResult.Data);
            return Result.ReSuccess();
        }

        /// <summary>
        /// 获取菜单权限集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ResultList<PermissionMenuInfoDetailsResponseDto>> GetList(PermissionListRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultList<PermissionMenuInfoDetailsResponseDto>.ReFailure(result);

            //获取权限数据
            var permissionList = await this._permissionRepository.GetList(dto);

            //筛选所有的菜单权限
            var menuList = permissionList.Where(f => f.Type == PermissionType.Menu).OrderBy(f => f.Sort).ToList();
            var menus = _mapper.Map<List<PermissionMenuInfoDetailsResponseDto>>(menuList);
            //筛选菜单对应的功能权限
            var actionList = await this._permissionRepository.GetList();
            menus.ForEach(m =>
            {
                m.Actions = actionList
                     .Where(f => f.Type == PermissionType.Action && f.ParentId == m.Id)
                     .OrderBy(f => f.Sort).ToList()
                     .ToDictionary(k => k.Id, v => v.Name);
            });
            return ResultList<PermissionMenuInfoDetailsResponseDto>.ReSuccess(menus);
        }
        /// <summary>
        /// 根据ID获取菜单权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<Result<PermissionMenuInfoDetailsResponseDto>> GetDetails(string id)
        {
            //获取菜单权限数据
            var menu = await this._permissionRepository.GetAsync(id);
            if (menu == null || menu.Type != PermissionType.Menu)
                return Result<PermissionMenuInfoDetailsResponseDto>.ReFailure(ResultCodes.PermissionNotExist);
            //获取子级权限
            var permissions = await this._permissionRepository.GetListByParentId(menu.Id);

            //组装响应对象
            var result = _mapper.Map<PermissionMenuInfoDetailsResponseDto>(menu);
            result.Actions = permissions
                     .Where(f => f.Type == PermissionType.Action)
                     .ToList()
                     .ToDictionary(k => k.Id, v => v.Name);
            return Result<PermissionMenuInfoDetailsResponseDto>.ReSuccess(result);
        }

        /// <summary>
        /// 获取所有菜单权限集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<ResultList<PermissionMenuInfoBaseResponseDto>> GetAllList()
        {
            //获取权限数据
            var permissionList = await this._permissionRepository.GetList();

            //筛选所有的菜单权限
            var menuList = permissionList.Where(p => p.Type == PermissionType.Menu).OrderBy(f => f.Sort).ToList();
            var menus = _mapper.Map<List<PermissionMenuInfoBaseResponseDto>>(menuList);

            //筛选菜单对应的功能权限
            menus.ForEach(m =>
            {
                m.Actions = permissionList
                     .Where(f => f.Type == PermissionType.Action && f.ParentId == m.Id)
                     .OrderBy(f => f.Sort).ToList()
                     .ToDictionary(k => k.Id, v => v.Name);
            });
            return ResultList<PermissionMenuInfoBaseResponseDto>.ReSuccess(menus);
        }

        /// <summary>
        /// 导出权限菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ResultList<PermissionMenuInfoDetailsResponseDto>> Export( PermissionMenuExportRequestDto dto)
        {
            // 验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultList<PermissionMenuInfoDetailsResponseDto>.ReFailure(result);
            if (dto.List != null && dto.List.Count > 0)
            {
                List<PermissionMenuInfoDetailsResponseDto> res = new List<PermissionMenuInfoDetailsResponseDto>();

                foreach (var item in dto.List)
                {
                    res.Add(item);
                    var children = await this._permissionRepository.GetActionListByParentId(item.Id);
                    var child = _mapper.Map<List<PermissionMenuInfoDetailsResponseDto>>(children);
                    if (child != null && child.Count > 0)
                    {
                        foreach (var chi in child)
                        {
                            res.Add(chi);
                        }
                    }
                }
                return ResultList<PermissionMenuInfoDetailsResponseDto>.ReSuccess(res);
            }
            else
            {
                return ResultList<PermissionMenuInfoDetailsResponseDto>.ReFailure("没有菜单", 200);
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> Import(PermissionMenuExportRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            if (dto.List != null && dto.List.Count > 0)
            {
                foreach (var item in dto.List)
                {
                    var model = await this._permissionRepository.GetAsync(item.Id);
                    if (model != null)
                    {
                        //修改
                        var entity = item.To();
                        await _permissionRepository.ModifyAsync(entity);
                    }
                    else
                    {
                        //添加
                        var entity = item.To();
                        await _permissionRepository.AddAsync(entity);
                    }
                }
            }
            return Result.ReSuccess();
        }
    }
}
