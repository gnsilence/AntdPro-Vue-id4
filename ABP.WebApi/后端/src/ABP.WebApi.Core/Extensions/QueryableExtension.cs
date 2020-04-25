using Abp.Application.Services.Dto;
using Abp.Dependency;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ABP.WebApi.Extensions
{
    /// <summary>
    /// 对IQueryable的扩展
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 分页方法
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TEntityDto">返回的Dto</typeparam>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static PagedResultDto<TEntityDto> GetPageSortList<TEntity, TEntityDto>(this IQueryable<TEntity> query, PagedSortedRequestInput input, Expression<Func<TEntity, TEntityDto>> selector = null)
        {
            //总行数
            var totalCount = query.Count();
            //排序
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting.Split(',')[0], (input.Sorting.Split(',').Length == 2 && input.Sorting.Split(',')[1].ToLower() == "desc") ? true : false);
            //分页
            int startIndex = (input.StartPage - 1) * input.PageCount;
            query = query.Skip(startIndex).Take(input.PageCount);

            List<TEntityDto> rlst = null;
            if (selector != null)
            {
                var query2 = query.Select(selector);
                rlst = query2.ToList();
            }
            else
            {
                //获取对象
                var entities = query.ToList();
                rlst = IocManager.Instance.Resolve<IMapper>().Map<List<TEntityDto>>(entities);
            }
            //组装分页返回对象
            var PagedResult = new PagedResultDto<TEntityDto>(totalCount, rlst);
            return PagedResult;
        }

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TEntityDto">返回的Dto</typeparam>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PagedResultDto<TEntityDto>> GetPageSortListAsync<TEntity, TEntityDto>(this IQueryable<TEntity> query, PagedSortedRequestInput input, Expression<Func<TEntity, TEntityDto>> selector = null)
        {
            //总行数
            var totalCount = await query.CountAsync();
            //排序
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting.Split(',')[0], (input.Sorting.Split(',').Length==2&&input.Sorting.Split(',')[1].ToLower() == "desc") ? true : false);
            //分页
            int startIndex = (input.StartPage - 1) * input.PageCount;
            query = query.Skip(startIndex).Take(input.PageCount);

            List<TEntityDto> rlst = null;
            if (selector != null)
            {
                var query2 = query.Select(selector);
                rlst = await query2.ToListAsync();
            }
            else
            {
                //获取对象
                var entities = await query.ToListAsync();
                rlst = IocManager.Instance.Resolve<IMapper>().Map<List<TEntityDto>>(entities);
            }
            //组装分页返回对象
            var PagedResult = new PagedResultDto<TEntityDto>(totalCount, rlst);
            return PagedResult;
        }
    }

    /// <summary>
    /// 分页入参
    /// </summary>
    public class PagedSortedRequestInput : ISortedResultRequest, IPagedAndSortedResultRequest
    {
        /// <summary>
        /// 排序字段 包含 ASC或DESC
        /// </summary>
        public string Sorting { get; set; }

        /// <summary>
        /// 当前页，从1开始
        /// </summary>
        public int StartPage { get; set; }

        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageCount { get; set; }

        [JsonIgnore]
        public int SkipCount
        {
            get => (StartPage - 1) * PageCount;
            set { }
        }

        [JsonIgnore]
        public int MaxResultCount
        {
            get => PageCount;
            set { }
        }
    }

    /// <summary>
    /// 分页出参
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedSortedRequestInput<T> : PagedSortedRequestInput
    {
        public T Query { get; set; }

    }
}
