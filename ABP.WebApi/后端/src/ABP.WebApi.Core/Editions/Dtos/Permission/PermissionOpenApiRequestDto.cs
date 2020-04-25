using ABP.WebApi;
using System;
using System.Collections.Generic;
using System.Text;
using ABP.WebApi.Dtos;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 公共api请求对象
    /// </summary>
    public class PermissionOpenApiRequestDto:IDto
    {
        /// <summary>
        /// 权限List
        /// </summary>
        public List<PermissionOpenApiInfoDetailsResponseDto> List { get; set; }
    }
}
