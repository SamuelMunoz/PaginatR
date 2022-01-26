using System.Linq.Expressions;
using Mapster;
using PaginatR.Process;
using PaginatR.Requests;
using PaginatR.Responses;

namespace PaginatR.Extensions;

public static class PaginationExtensions
{
    /// <summary>
    /// Paginates the queryable sent
    /// </summary>
    /// <param name="queryable">The queryable</param>
    /// <param name="request">The pagination request</param>
    /// <param name="orderBy">The orderBy expression</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <typeparam name="T">The type</typeparam>
    /// <typeparam name="TOrderBy">The order by type</typeparam>
    /// <typeparam name="TOut">The type to which the object will be converted to</typeparam>
    /// <returns>A pagination response with the data paginated</returns>
    public static async Task<PaginationResponse<TOut>> ToPaginatedAsync<T, TOrderBy, TOut>(this IQueryable<T> queryable, PaginationRequest request, 
        Expression<Func<T, TOrderBy>> orderBy, CancellationToken cancellationToken)
    {
        var (pageNumber, pageSize) = request;
        var (skip, totalPages) = PaginationProcessing.CalculatePagination(queryable, request);
        var data = await PaginationProcessing.QueryPaginatedAsync(queryable, skip, pageSize, orderBy, cancellationToken);
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