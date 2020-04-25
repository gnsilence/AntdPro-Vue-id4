using Abp.Dependency;
using ABP.WebApi.Authorization;
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

namespace ABP.WebApi.Api.Permission
{
    [Authorize]
   public class PermissionAppService: WebApiAppServiceBase
    {
        protected readonly LogOperateRecordService _operateLog;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IPermissionRepository _permissionRepository;
        protected readonly IMapper _mapper;

        public PermissionAppService(LogOperateRecordService operateLog, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository,IMapper mapper)
        {
            _operateLog = operateLog;
            _unitOfWork = unitOfWork;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// 修改权限排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> ModifySort(PermissionModifySortRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return result;


            //修改功能权限
            var modifyResult = await IocManager.Instance.Resolve<PermissionChangeService>().ModifySort(dto.Id, dto.Sort);
            if (!modifyResult.Success)
                return modifyResult;

            //数据持久化
            await _permissionRepository.ModifyAsync(modifyResult.Data);
            return Result.ReSuccess();
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id">权限标识</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Result> Delete(string id)
        {
            //删除权限
            var result = await IocManager.Instance.Resolve<PermissionDeleteService>().Delete(id);
            if (!result.Success)
                return result;

            _operateLog.Record(ASFPermissions.PermissionDelete, new { permissionId = id }, "Success");  //记录日志
            await _permissionRepository.RemoveAsync(id);
            return Result.ReSuccess();
        }
    }
}
