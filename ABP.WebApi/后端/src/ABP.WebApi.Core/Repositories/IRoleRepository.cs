using ABP.WebApi.Entities;
using ASF.Application.DTO;
using ASF.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABP.WebApi.Core.Repositories
{
    public interface IRoleRepository : IRepository<Role, int>
    {
        /// <summary>
        ///根据角色标识获取角色集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IList<Role>> GetList(IList<int> ids);
        /// <summary>
        ///获取角色集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IList<Role>> GetList();
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="account">角色信息</param>
        /// <returns></returns>
        Task ModifyAsync(Role role);
        /// <summary>
        /// 是否存在权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ExistsRole(string name);

        /// <summary>
        /// 修改角色状态
        /// </summary>
        /// <param name="enable">是否可用</param>
        /// <returns></returns>
        Task ModifyAsync(int roleId,bool enable);

        /// <summary>
        /// 分页获取角色集合
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<(IList<Role> Roles, int TotalCount)> GetList(RoleListPagedRequestDto requestDto);
    }
}
