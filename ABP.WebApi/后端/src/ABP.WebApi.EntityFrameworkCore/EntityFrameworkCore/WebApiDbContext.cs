using Microsoft.EntityFrameworkCore;
using Abp.EntityFrameworkCore;
using Abp.Auditing;
using ABP.WebApi.Entities;
using ASF.EntityFramework.Model;

namespace ABP.WebApi.EntityFrameworkCore
{
    public class WebApiDbContext : AbpDbContext
    {
        /* Define a DbSet for each entity of the application */

        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
        }
        //public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<LogInfoModel> LogInfos { get; set; }
        public DbSet<PermissionModel> Permissions { get; set; }
        public DbSet<RoleModel> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountModel>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<LogInfoModel>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<PermissionModel>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<RoleModel>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
