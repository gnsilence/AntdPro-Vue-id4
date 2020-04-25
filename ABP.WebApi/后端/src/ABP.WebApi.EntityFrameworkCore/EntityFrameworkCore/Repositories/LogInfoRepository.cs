using Abp.EntityFrameworkCore;
using ABP.WebApi.Core.Repositories;
using ABP.WebApi.EntityFrameworkCore;
using ABP.WebApi.Extensions.Values;
using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Domain.Values;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ABP.WebApi.Entities.Enums.CommonEnum;

namespace ASF.EntityFramework.Repository
{
    public class LogInfoRepository : ILoggingRepository
    {
        private readonly IMapper _mapper;
        private readonly IDbContextProvider<WebApiDbContext> _dbContextProvider;
        public LogInfoRepository(IMapper mapper, IDbContextProvider<WebApiDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _mapper = mapper;
        }

        public async Task<Logging> AddAsync(Logging entity)
        {
            var model = _mapper.Map<Model.LogInfoModel>(entity);
            await _dbContextProvider.GetDbContext().AddAsync(model);
            return _mapper.Map<Logging>(model);
        }

        public async Task<Logging> GetAsync(long id)
        {
            var model = await _dbContextProvider.GetDbContext().LogInfos.FirstOrDefaultAsync(w => w.Id == id);
            return _mapper.Map<Logging>(model);
        }

        public Task RemoveAsync(long primaryKey)
        {
            return Task.FromResult(0);
        }

        public async Task<IList<Logging>> GetList()
        {
            var list = await _dbContextProvider.GetDbContext().LogInfos.ToListAsync();
            return _mapper.Map<IList<Logging>>(list);
        }

        public async Task<(IList<Logging> Loggings, int TotalCount)> GetList(LoggerListPagedRequestDto dto)
        {
            var queryable = _dbContextProvider.GetDbContext().LogInfos
                .Where(w => w.Id > 0);

            if (!string.IsNullOrEmpty(dto.Subject))
            {
                queryable = queryable.Where(w => EF.Functions.Like(w.Subject, "%" + dto.Subject + "%") 
                || w.Id.ToString()==dto.Subject);
            }
            if (!string.IsNullOrEmpty(dto.Account))
            {
                queryable = queryable
                    .Where(w => EF.Functions.Like(w.AccountName, "%" + dto.Account + "%")
                    || w.AccountId.ToString() == dto.Account
                    );
            }
            if (dto.Type == 1)
                queryable = queryable.Where(w => w.Type == LoggingType.Login);
            if (dto.Type == 2)
                queryable = queryable.Where(w => w.Type == LoggingType.Operate);
            if (dto.BeginTime != null && dto.BeginTime != default(DateTime))
                queryable = queryable.Where(w => w.AddTime >= dto.BeginTime);
            if (dto.EndTime != null && dto.EndTime != default(DateTime))
                queryable = queryable.Where(w => w.AddTime <= dto.EndTime);

            if (!string.IsNullOrEmpty(dto.PermissionId))
                queryable = queryable.Where(w => w.PermissionId == dto.PermissionId);
            if (!string.IsNullOrEmpty(dto.ClientIp))
                queryable = queryable.Where(w => w.ClientIp == dto.ClientIp);

            var result = queryable.OrderByDescending(p => p.AddTime);
            var list = await result.Skip((dto.SkipPage - 1) * dto.PagedCount).Take(dto.PagedCount).ToListAsync();

            return (_mapper.Map<List<Logging>>(list), result.Count());
        }

        public Task Delete(LoggerDeleteRequestDto dto)
        {
            var queryable = _dbContextProvider.GetDbContext().LogInfos.Where(w => w.Id > 0);
            if (dto.BeginTime != default(DateTime) && dto.BeginTime != null)
                queryable = queryable.Where(w => w.AddTime >= dto.BeginTime);
            if (dto.EndTime != default(DateTime) && dto.EndTime != null)
                queryable = queryable.Where(w => w.AddTime <= dto.EndTime);

            _dbContextProvider.GetDbContext().LogInfos.RemoveRange(queryable);
            return Task.FromResult(0);
        }

     
    }
}
