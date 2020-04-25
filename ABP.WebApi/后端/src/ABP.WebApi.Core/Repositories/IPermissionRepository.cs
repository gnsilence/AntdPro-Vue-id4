﻿using ABP.WebApi.Entities;
using ASF.Application.DTO;
using ASF.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABP.WebApi.Core.Repositories
{
    public interface IPermissionRepository : IRepository<Permission, string>
    {
        /// <summary>
        /// 这标识是否被使用
        /// </summary>
        /// <param name="id">权限标识</param>
        /// <returns></returns>
        Task<bool> HasById(string id);
        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <returns></returns>
        Task ModifyAsync(Permission permission);
        /// <summary>
        /// 根据ID获取权限集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IList<Permission>> GetList(IList<string> ids);
        /// <summary>
        /// 根据父类获取子级权限集合
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<IList<Permission>> GetListByParentId(string parentId);

        /// <summary>
        /// 获取action集合，根据父类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<IList<Permission>> GetActionListByParentId(string parentId);

        /// <summary>
        /// 获取所有权限集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IList<Permission>> GetList();
        /// <summary>
        /// 分页获取角色集合
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<IList<Permission>> GetList(PermissionListRequestDto requestDto);
    }
}
