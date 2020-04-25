﻿using Abp.EntityFrameworkCore;
using Abp.Localization;
using ABP.WebApi.TestModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.EntityFrameworkCore
{
  public  class JPGZServicePostgreSqlDbContext: AbpDbContext
    {
        /* Define a DbSet for each entity of the application */

        public JPGZServicePostgreSqlDbContext(DbContextOptions<JPGZServicePostgreSqlDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Animal> StudentBase { get; set; }
    }
}
