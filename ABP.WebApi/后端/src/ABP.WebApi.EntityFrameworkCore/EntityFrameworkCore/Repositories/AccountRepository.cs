using ABP.WebApi.Core.Repositories;
using ABP.WebApi.Entities.Enums;
using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.EntityFramework.Model;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABP.WebApi.Extensions.Values;
using Abp.EntityFrameworkCore;
using ABP.WebApi.EntityFrameworkCore;
using ABP.WebApi.EntityFrameworkCore.Repositories;
using Abp.Dependency;

namespace ASF.EntityFramework.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbContextProvider<WebApiDbContext> _dbContextProvider;
        // public readonly RepositoryContext _dbContext;
        private readonly IMapper _mapper;
        public AccountRepository(IMapper mapper, IDbContextProvider<WebApiDbContext> dbContextProvider)
        {
            //_dbContext = dbContext;
            _dbContextProvider = dbContextProvider;
            _mapper = mapper;
        }
        public async Task<Account> AddAsync(Account entity)
        {
            var model = _mapper.Map<AccountModel>(entity);
            var acct = new AccountModel()
            {
                Name = entity.Name,
                Password = entity.Password,
                Username = entity.Username,
                Roles = model.Roles,
                Avatar = model.Avatar,
                UserGuid = "",
                Status = model.Status
            };
            await _dbContextProvider.GetDbContext().AddAsync(acct);
            return _mapper.Map<Account>(model);
        }

        public async Task<Account> GetAsync(PhoneNumber telephone)
        {
            var model = await _dbContextProvider.GetDbContext().Accounts.FirstOrDefaultAsync(w => w.Telephone == telephone.ToString());
            return _mapper.Map<Account>(model);
        }

        public async Task<Account> GetAsync(int id)
        {
            var model = await _dbContextProvider.GetDbContext().Accounts.FirstOrDefaultAsync(w => w.Id == id);
            return _mapper.Map<Account>(model);
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            var model = await _dbContextProvider.GetDbContext().Accounts.FirstOrDefaultAsync(w => w.Email == email);
            return _mapper.Map<Account>(model);
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            var model = await _dbContextProvider.GetDbContext().Accounts.FirstOrDefaultAsync(w => w.Username == username);
            return _mapper.Map<Account>(model);
        }

        public async Task<(IList<Account> Accounts, int TotalCount)> GetList(AccountListPagedRequestDto requestDto)
        {
            var queryable = _dbContextProvider.GetDbContext().Accounts
                .Where(w => w.IsDeleted == requestDto.IsDeleted);

            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => w.Id.ToString() == requestDto.Vague
                    || EF.Functions.Like(w.Name, "%" + requestDto.Vague + "%")
                    || EF.Functions.Like(w.Username, "%" + requestDto.Vague + "%")
                    );
            }
            if (requestDto.Status == 1)
                queryable = queryable.Where(w => w.Status == AccountStatus.Normal);
            if (requestDto.Status == 2)
                queryable = queryable.Where(w => w.Status == AccountStatus.NotAllowedLogin);

            var result = queryable.OrderByDescending(p => p.CreateTime);
            var list = await result.Skip((requestDto.SkipPage - 1) * requestDto.PagedCount).Take(requestDto.PagedCount).ToListAsync();

            return (_mapper.Map<List<Account>>(list), result.Count());
        }

        public async Task<bool> HasByEmail(string email)
        {
            var model = await _dbContextProvider.GetDbContext().Accounts.FirstOrDefaultAsync(w => w.Email == email);
            return model == null ? false : true;
        }

        public async Task<bool> HasByTelephone(PhoneNumber telephone)
        {
            var model = await _dbContextProvider.GetDbContext().Accounts.FirstOrDefaultAsync(w => w.Telephone == telephone.ToString());
            return model == null ? false : true;
        }

        public async Task<bool> HasByUsername(string username)
        {
            var model = await _dbContextProvider.GetDbContext().Accounts.FirstOrDefaultAsync(w => w.Username == username);
            return model == null ? false : true;
        }

        public async Task ModifyAsync(Account account)
        {
            var model = await _dbContextProvider.GetDbContext().Accounts.FirstOrDefaultAsync(w => w.Id == account.Id);
            if (model == null) return;
            _mapper.Map(account, model);
            _dbContextProvider.GetDbContext().Update<AccountModel>(model);
        }

        public async Task RemoveAsync(int primaryKey)
        {
            var model = await _dbContextProvider.GetDbContext().Accounts.FirstOrDefaultAsync(w => w.Id == primaryKey);
            if (model == null) return;
            model.Delete();
            _dbContextProvider.GetDbContext().Accounts.Update(model);
        }

        public async Task<IList<Account>> GetList()
        {
            var list = await _dbContextProvider.GetDbContext().Accounts.ToListAsync();
            return _mapper.Map<IList<Account>>(list);
        }

        public int GetIdByUserGuidAsync(string userid)
        {
            return _dbContextProvider.GetDbContext().Accounts.Where(p => p.UserGuid == userid).FirstOrDefault().Id;
        }
    }
}
