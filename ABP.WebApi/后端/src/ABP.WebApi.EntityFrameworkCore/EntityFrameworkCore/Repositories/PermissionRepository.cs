using Abp.EntityFrameworkCore;
using ABP.WebApi.Core.Repositories;
using ABP.WebApi.EntityFrameworkCore;
using ASF.Application.DTO;
using ASF.Domain.Entities;
using ABP.WebApi.Extensions.Values;
using ASF.EntityFramework.Model;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ABP.WebApi.Entities.Enums.CommonEnum;

namespace ASF.EntityFramework.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IDbContextProvider<WebApiDbContext> _dbContextProvider;
        private readonly IMapper _mapper;
        public PermissionRepository(IDbContextProvider<WebApiDbContext> dbContextProvider, IMapper mapper)
        {
            _dbContextProvider = dbContextProvider;
            _mapper = mapper;
        }
        public async Task<Permission> AddAsync(Permission entity)
        {
            var model = _mapper.Map<PermissionModel>(entity);
            await _dbContextProvider.GetDbContext().AddAsync(model);
            return _mapper.Map<Permission>(model);
        }

        public async Task<Permission> GetAsync(string id)
        {
            var model = await _dbContextProvider.GetDbContext().Permissions.FirstOrDefaultAsync(w => w.Id == id);
            return _mapper.Map<Permission>(model);
        }

        public async Task<IList<Permission>> GetList(IList<string> ids)
        {
            var list = await _dbContextProvider.GetDbContext().Permissions.Where(w => ids.Contains(w.Id)).ToListAsync();
            list = list == null ? new List<PermissionModel>() : list;
            return _mapper.Map<List<Permission>>(list);
        }

        public async Task<IList<Permission>> GetList(PermissionListRequestDto requestDto)
        {
            var queryable = _dbContextProvider.GetDbContext().Permissions
                .Where(w => w.Id != "");

            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                //queryable = queryable
                //    .Where(w => w.Id.ToString() == requestDto.Vague
                //    || EF.Functions.Like(w.Name, "%" + requestDto.Vague + "%"));
                queryable = queryable.Where(p => p.Id.Contains(requestDto.Vague) || p.Name.Contains(requestDto.Vague));
            }
            if (!string.IsNullOrEmpty(requestDto.ParamId))
                queryable = queryable.Where(w => w.ParentId == requestDto.ParamId);

            if (requestDto.Enable == 1)
                queryable = queryable.Where(w => w.Enable == true);
            if (requestDto.Enable == 0)
                queryable = queryable.Where(w => w.Enable == false);

            var list = await queryable.ToListAsync();

            return _mapper.Map<IList<Permission>>(list);
        }

        public async Task<IList<Permission>> GetList()
        {
            var list = await _dbContextProvider.GetDbContext().Permissions.ToListAsync();
            return _mapper.Map<IList<Permission>>(list);
        }

        public async Task<IList<Permission>> GetListByParentId(string parentId)
        {
            var list = await _dbContextProvider.GetDbContext().Permissions.Where(f => f.ParentId == parentId).ToListAsync();
            return _mapper.Map<IList<Permission>>(list);
        }

        public async Task<IList<Permission>> GetActionListByParentId(string parentId)
        {
            var list = await _dbContextProvider.GetDbContext().Permissions.Where(f => f.ParentId == parentId && f.Type== PermissionType.Action).ToListAsync();
            return _mapper.Map<IList<Permission>>(list);
        }

        public async Task<bool> HasById(string id)
        {
            var model = await _dbContextProvider.GetDbContext().Permissions.FirstOrDefaultAsync(w => w.Id == id);
            return model == null ? false : true;
        }

        public async Task ModifyAsync(Permission permission)
        {
            var model = await _dbContextProvider.GetDbContext().Permissions.FirstOrDefaultAsync(w => w.Id == permission.Id);
            if (model == null) return;
            _mapper.Map(permission, model);
            _dbContextProvider.GetDbContext().Permissions.Update(model);
        }

        public async Task RemoveAsync(string primaryKey)
        {
            var model = await _dbContextProvider.GetDbContext().Permissions.FirstOrDefaultAsync(w => w.Id == primaryKey);
            if (model == null) return;
            _dbContextProvider.GetDbContext().Remove(model);
        }
    }
}
