﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ABP.WebApi.Dtos;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 角色创建请求Dto
    /// </summary>
    public class RoleCreateRequestDto : IDto
    {
        [MaxLength(64)]
        public string RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required, MaxLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// 分配的权限
        /// </summary>
        public List<string> Permissions { get;  set; } = new List<string>();
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
