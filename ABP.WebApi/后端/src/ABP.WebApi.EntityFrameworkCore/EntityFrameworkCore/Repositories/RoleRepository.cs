using Abp.EntityFrameworkCore;
using ABP.WebApi.Core.Repositories;
using ABP.WebApi.EntityFrameworkCore;
using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.EntityFramework.Model;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASF.EntityFramework.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IDbContextProvider<WebApiDbContext> _dbContextProvider;
        private readonly IMapper _mapper;
        public RoleRepository(IDbContextProvider<WebApiDbContext> dbContextProvider, IMapper mapper)
        {
            _dbContextProvider = dbContextProvider;
            _mapper = mapper;
        }

        public async Task<Role> AddAsync(Role entity)
        {
            var model = _mapper.Map<RoleModel>(entity);
            await _dbContextProvider.GetDbContext().AddAsync(model);
            return _mapper.Map<Role>(model);
        }

        public bool ExistsRole(string name)
        {
            var model = _dbContextProvider.GetDbContext().Roles.FirstOrDefault(w => w.Name == name);
            if (model == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Role> GetAsync(int id)
        {
            var model = await _dbContextProvider.GetDbContext().Roles.FirstOrDefaultAsync(w => w.Id == id);
            return _mapper.Map<Role>(model);
        }

        public async Task<IList<Role>> GetList(IList<int> ids)
        {
            var list = await _dbContextProvider.GetDbContext().Roles.Where(w => ids.Contains(w.Id)).ToListAsync();
            list = list == null ? new List<RoleModel>() : list;
            return _mapper.Map<List<Role>>(list);
        }

        public async Task<IList<Role>> GetList()
        {
            var list = await _dbContextProvider.GetDbContext().Roles.ToListAsync();
            list = list == null ? new List<RoleModel>() : list;
            return _mapper.Map<List<Role>>(list);
        }

        public async Task<(IList<Role> Roles, int TotalCount)> GetList(RoleListPagedRequestDto requestDto)
        {
            var queryable = _dbContextProvider.GetDbContext().Roles
               .Where(w => w.Id > 0);

            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => w.Id.ToString() == requestDto.Vague
                    || EF.Functions.Like(w.Name, "%" + requestDto.Vague + "%"));
            }
            if (requestDto.Enable == 1)
                queryable = queryable.Where(w => w.Enable == true);
            if (requestDto.Enable == 0)
                queryable = queryable.Where(w => w.Enable == false);

            var result = queryable.OrderByDescending(p => p.CreateTime);
            var list = await result.Skip((requestDto.SkipPage - 1) * requestDto.PagedCount).Take(requestDto.PagedCount).ToListAsync();

            return (_mapper.Map<List<Role>>(list), result.Count());
        }
        public async Task ModifyAsync(Role role)
        {
            var model = await _dbContextProvider.GetDbContext().Roles.FirstOrDefaultAsync(w => w.Id == role.Id);
            if (model == null) return;
            _mapper.Map(role, model);
            _dbContextProvider.GetDbContext().Roles.Update(model);
        }
        public async Task ModifyAsync(int roleId, bool enable)
        {
            var model = await _dbContextProvider.GetDbContext().Roles.FirstOrDefaultAsync(w => w.Id == roleId);
            if (model == null) return;
            model.Enable = enable;
            _dbContextProvider.GetDbContext().Roles.Update(model);
        }
        public async Task RemoveAsync(int primaryKey)
        {
            var model = await _dbContextProvider.GetDbContext().Roles.FirstOrDefaultAsync(w => w.Id == primaryKey);
            if (model == null) return;
            _dbContextProvider.GetDbContext().Remove(model);
        }
    }
}
