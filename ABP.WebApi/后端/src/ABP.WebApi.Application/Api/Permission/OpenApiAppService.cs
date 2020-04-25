using Abp.Dependency;
using ABP.WebApi.Configuration;
using ABP.WebApi.Core.Repositories;
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
using ABP.WebApi.Extensions.Values;
using ABP.WebApi.Entities;

namespace ABP.WebApi.Api.Permission
{
   public class OpenApiAppService: PermissionAppService
    {
        public OpenApiAppService(LogOperateRecordService operateLog, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository, IMapper mapper) : base(operateLog, unitOfWork, permissionRepository, mapper)
        {

        }
        /// <summary>
        /// 创建开放API权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create(PermissionOpenApiCreateRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //创建菜单权限
            var permission = dto.To();
            result = IocManager.Instance.Resolve<PermissionCreateService>().CreateOpenApi(permission);
            if (!result.Success)
                return result;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionCreateOpenApi, dto, "Success");  //记录日志
            await _permissionRepository.AddAsync(permission);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 修改开放API权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Modify(PermissionOpenApiModifyRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;

            //修改功能权限
            var modifyResult = await IocManager.Instance.Resolve<PermissionChangeService>().ModifyOpenApi(dto.Id, dto.Name, dto.Description, dto.Enable, dto.ApiTemplate, dto.HttpMethods.Select(f => new HttpMethod(f)).ToList());
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            _operateLog.Record(ASFPermissions.PermissionModifyOpenApi, dto, "Success");  //记录日志
            await _permissionRepository.ModifyAsync(modifyResult.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 根据ID获取开放API权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<PermissionOpenApiInfoDetailsResponseDto>> GetDetails(string id)
        {
            //获取菜单权限数据
            var menu = await this._permissionRepository.GetAsync(id);
            if (menu == null || menu.Type != PermissionType.OpenApi)
                return Result<PermissionOpenApiInfoDetailsResponseDto>.ReFailure(ResultCodes.PermissionNotExist);

            //组装响应对象
            var result = _mapper.Map<PermissionOpenApiInfoDetailsResponseDto>(menu);
            return Result<PermissionOpenApiInfoDetailsResponseDto>.ReSuccess(result);
        }
        /// <summary>
        /// 获取开放API权限集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultList<PermissionOpenApiInfoDetailsResponseDto>> GetList(PermissionListRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return ResultList<PermissionOpenApiInfoDetailsResponseDto>.ReFailure(result);

            //获取权限数据
            var permissionList = await this._permissionRepository.GetList(dto);

            //筛选所有的菜单权限
            var menuList = permissionList.Where(f => f.Type == PermissionType.OpenApi).OrderBy(f => f.Sort).ToList();
            var menus = _mapper.Map<List<PermissionOpenApiInfoDetailsResponseDto>>(menuList);
            return ResultList<PermissionOpenApiInfoDetailsResponseDto>.ReSuccess(menus);
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Import(PermissionOpenApiRequestDto dto)
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
