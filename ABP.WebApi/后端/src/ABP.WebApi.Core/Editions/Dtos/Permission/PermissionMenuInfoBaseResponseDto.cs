﻿using ABP.WebApi;
using System.Collections.Generic;
using ABP.WebApi.Dtos;

namespace ASF.Application.DTO
{
    public class PermissionMenuInfoBaseResponseDto:IDto
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 上级权限编号
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 功能集合
        /// </summary>
        public Dictionary<string, string> Actions { get; set; } = new Dictionary<string, string>();
    }
}
