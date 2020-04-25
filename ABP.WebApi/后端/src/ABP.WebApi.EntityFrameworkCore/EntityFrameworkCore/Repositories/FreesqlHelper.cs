using ABP.WebApi.Configuration;
using ABP.WebApi.Web;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.EntityFrameworkCore.Repositories
{
    public static class FreesqlHelper
    {
        public static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
       .UseConnectionString(FreeSql.DataType.MySql,
           GetConnectionString())
       .UseAutoSyncStructure(false) //自动同步实体结构到数据库
       .Build();
       
        private static string GetConnectionString()
        {
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
            return configuration.GetConnectionString(WebApiConsts.MysqlConnectionStringName);
        }
    }
}
