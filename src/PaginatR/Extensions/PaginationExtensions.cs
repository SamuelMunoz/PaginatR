using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using PaginatR.Enums;
using PaginatR.Process;
using PaginatR.Requests;
using PaginatR.Responses;

namespace PaginatR.Extensions
{
    public static class PaginationExtensions
    {
        /// <summary>
        /// Paginates the queryable sent
        /// </summary>
        /// <param name="queryable">The queryable to be paginated</param>
        /// <param name="request">The pagination request</param>
        /// <param name="orderBy">The orderBy expression</param>
        /// <param name="direction">The orderBy direction</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <typeparam name="TIn">The type of the queryable source</typeparam>
        /// <typeparam name="TOrderBy">The orderBy type</typeparam>
        /// <typeparam name="TOut">The type to which the object will be mapped to</typeparam>
        /// <returns>A pagination response with the results paginated</returns>
        public static async Task<PaginationResponse<TOut>> ToPaginatedAsync<TIn, TOrderBy, TOut>(this IQueryable<TIn> queryable, PaginationRequest request, 
            Expression<Func<TIn, TOrderBy>> orderBy, OrderDirection direction, CancellationToken cancellationToken)
        {
            var (pageNumber, pageSize) = request;
            var (skip, totalPages) = PaginationProcessing.CalculatePagination(queryable, request);
            var data = await PaginationProcessing.QueryPaginatedAsync(queryable, skip, pageSize, orderBy, direction, cancellationToken);
            var result = new PaginationResponse<TOut>(data.Adapt<List<TOut>>())
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasPrevious = pageNumber > 1,
                HasNext = pageNumber < totalPages
            };
            return await Task.FromResult(result);
        }
    }
}