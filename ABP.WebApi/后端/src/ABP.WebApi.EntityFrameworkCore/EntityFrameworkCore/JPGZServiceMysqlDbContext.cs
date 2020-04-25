using Abp.Auditing;
using Abp.EntityFrameworkCore;
using ABP.WebApi.Entities;
using ABP.WebApi.TestModel;
using ASF.EntityFramework.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.EntityFrameworkCore
{
    public class JPGZServiceMysqlDbContext : AbpDbContext
    {
        public JPGZServiceMysqlDbContext(DbContextOptions<JPGZServiceMysqlDbContext> options)
           : base(options)
        {
         
        }
        //public virtual DbSet<AuditLog> AuditLogs { get; set; }
        //public DbSet<AccountModel> Accounts { get; set; }
        //public DbSet<LogInfoModel> LogInfos { get; set; }
        //public DbSet<PermissionModel> Permissions { get; set; }
        //public DbSet<RoleModel> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AccountModel>(e =>
            //{
            //    e.HasKey(x => x.Id);
            //    e.Property(x => x.Id).ValueGeneratedOnAdd();
            //});
            //modelBuilder.Entity<LogInfoModel>(e =>
            //{
            //    e.HasKey(x => x.Id);
            //    e.Property(x => x.Id).ValueGeneratedOnAdd();
            //});
            //modelBuilder.Entity<PermissionModel>(e =>
            //{
            //    e.HasKey(x => x.Id);
            //    e.Property(x => x.Id).ValueGeneratedOnAdd();
            //});
            //modelBuilder.Entity<RoleModel>(e =>
            //{
            //    e.HasKey(x => x.Id);
            //    e.Property(x => x.Id).ValueGeneratedOnAdd();
            //});
            base.OnModelCreating(modelBuilder);
        }
    }
}
