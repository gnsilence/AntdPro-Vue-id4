using Abp.EntityFrameworkCore;
using ABP.WebApi.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ASF.EntityFramework.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextProvider<WebApiDbContext> _dbContextProvider;
        public UnitOfWork(IDbContextProvider<WebApiDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }
        public bool Committed => throw new NotImplementedException();

        public bool Commit(bool autoRollback = false)
        {
            using (var transaction = _dbContextProvider.GetDbContext().Database.BeginTransaction())
            {
                try
                {
                    _dbContextProvider.GetDbContext().SaveChanges();
                    //如果未执行到Commit()就执行失败遇到异常了，EF Core会自动进行数据回滚（前提是使用Using） 
                    transaction.Commit();
                }
                catch (Exception ex)
                { 
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }

        public Task<bool> CommitAsync(bool autoRollback = false)
        {
            using (var transaction = _dbContextProvider.GetDbContext().Database.BeginTransaction())
            {
                try
                {
                    _dbContextProvider.GetDbContext().SaveChanges();
                    //如果未执行到Commit()就执行失败遇到异常了，EF Core会自动进行数据回滚（前提是使用Using） 
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Task.FromResult(false);
                }
            }
            return Task.FromResult(true);
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
