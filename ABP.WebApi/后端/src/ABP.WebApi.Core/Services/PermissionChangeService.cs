﻿using ABP.WebApi.Core.Repositories;
using ABP.WebApi.Entities;
using ABP.WebApi.Extensions.Values;
using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using static ABP.WebApi.Entities.Enums.CommonEnum;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 权限修改服务
    /// </summary>
    public class PermissionChangeService
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionChangeService(IPermissionRepository permissionRepository)
        {
            this._permissionRepository = permissionRepository;
        }
        /// <summary>
        /// 修改功能权限
        /// </summary>
        /// <param name="pid">权限标识</param>
        /// <param name="name">权限名称</param>
        /// <param name="description">描述</param>
        /// <param name="enable">是否可用</param>
        /// <param name="apiTemplate">api 地址</param>
        /// <param name="isLogger">是否记录日志</param>
        /// <param name="httpMethods">api 支持 Http 请求方法</param>
        /// <returns></returns>
        public async Task<Result<Permission>> ModifyAction(string pid, string name, string description, bool enable, string apiTemplate, bool isLogger, List<HttpMethod> httpMethods)
        {
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(pid);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);
            if (permission.Type != PermissionType.Action)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            //如果是系统权限不能修改
            if (permission.IsSystem)
            {
                return Result<Permission>.ReFailure(ResultCodes.PermissionSystemNotModify.ToFormat(permission.Name));
            }

            permission.Name = name;
            permission.HttpMethods = httpMethods;
            permission.IsLogger = isLogger;
            permission.Enable = enable;
            permission.Description = description;
            permission.SetApiTemplate(apiTemplate);

            //验证权限聚合的数据合法性
            return permission.Valid<Permission>();
        }
        /// <summary>
        /// 修改菜单权限
        /// </summary>
        /// <param name="pid">权限标识</param>
        /// <param name="name">权限名称</param>
        /// <param name="parentId">分类</param>
        /// <param name="description">描述</param>
        /// <param name="enable">是否可用</param>
        /// <param name="icon">菜单图标</param>
        /// <param name="redirect">菜单重定向</param>
        /// <param name="hidden">菜单是否隐藏</param>
        /// <returns></returns>
        public async Task<Result<Permission>> ModifyMenu(string pid, string name, string parentId, string description, bool enable, string icon, string redirect, bool hidden)
        {
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(pid);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);
            if (permission.Type != PermissionType.Menu)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            //如果是系统权限不能修改
            if (permission.IsSystem)
            {
                return Result<Permission>.ReFailure(ResultCodes.PermissionSystemNotModify.ToFormat(permission.Name));
            }

            //如果配置上级权限，需要验证上级权限必须为菜单权限
            if (permission.ParentId != parentId && !string.IsNullOrEmpty(parentId) && permission.Id != parentId)
            {
                //判断上级权限,上级权限不能为菜单权限
                var paremt = await this._permissionRepository.GetAsync(parentId);
                if (paremt == null)
                {
                    return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);
                }
                if (paremt.Type == PermissionType.Action)
                {
                    return Result<Permission>.ReFailure(ResultCodes.PermissionParemtNotAction);
                }
            }

            permission.Name = name;
            permission.ParentId = parentId;
            permission.Enable = enable;
            permission.Description = description;
            permission.MenuIcon = icon;
            permission.MenuRedirect = redirect;
            permission.MenuHidden = hidden;
            //验证权限聚合的数据合法性
            return permission.Valid<Permission>();
        }

        /// <summary>
        /// 修改开放API权限
        /// </summary>
        /// <param name="pid">权限标识</param>
        /// <param name="name">权限名称</param>
        /// <param name="description">描述</param>
        /// <param name="enable">是否可用</param>
        /// <param name="apiTemplate">api 地址</param>
        /// <param name="httpMethods">api 支持 Http 请求方法</param>
        /// <returns></returns>
        public async Task<Result<Permission>> ModifyOpenApi(string pid, string name, string description, bool enable, string apiTemplate, List<HttpMethod> httpMethods)
        {
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(pid);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);
            if (permission.Type != PermissionType.OpenApi)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            //如果是系统权限不能修改
            if (permission.IsSystem)
            {
                return Result<Permission>.ReFailure(ResultCodes.PermissionSystemNotModify.ToFormat(permission.Name));
            }
            permission.Name = name;
            permission.HttpMethods = httpMethods;
            permission.Enable = enable;
            permission.Description = description;
            permission.SetApiTemplate(apiTemplate);

            //验证权限聚合的数据合法性
            return permission.Valid<Permission>();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<Result<Permission>> ModifySort(string pid, int sort)
        {
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(pid);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            permission.Sort = sort;
            return Result<Permission>.ReSuccess(permission);
        }
    }
}
