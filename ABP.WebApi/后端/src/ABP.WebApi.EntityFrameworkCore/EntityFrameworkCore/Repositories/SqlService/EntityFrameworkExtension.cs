﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Abp.EntityFrameworkCore;

namespace ABP.WebApi.EntityFrameworkCore.Repositories.SqlService
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityFrameworkExtension
    {
        /// <summary>
        /// 调用ADO.NET执行SQL语句查询泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">数据库连接上下文</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL语句需要的参数</param>
        /// <returns></returns>
        public static List<T> FromSqlEt<T>(this IDbContextProvider<JPGZServiceMysqlDbContext> database, string sql, object parameters = null)
        {
            var result = new List<T>();
            using (var command = database.GetDbContext().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                {
                    var _parameters = parameters.ToSqlParamsArray();
                    command.Parameters.AddRange(_parameters);
                }

                database.GetDbContext().Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    result = dt.ToList<T>();
                    return result;
                }
            }
        }
    }
}
