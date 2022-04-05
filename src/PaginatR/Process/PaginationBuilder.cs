using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Mapster;
using PaginatR.Contracts;
using PaginatR.Enums;

namespace PaginatR.Process
{
    /// <summary>
    /// Paginates any queryable
    /// </summary>
    /// <typeparam name="TIn">The in type</typeparam>
    /// <typeparam name="TOrderBy">The orderBy type</typeparam>
    /// <typeparam name="TOut">The out type</typeparam>
    public class PaginationBuilder<TIn, TOrderBy, TOut> :
        IAddQueryableStage<TIn, TOrderBy, TOut>,
        IAddRequestStage<TIn, TOrderBy, TOut>,
        IAddOrderByStage<TIn, TOrderBy, TOut>,
        IAddDirectionStage<TOut>,
        IGeneratePagination<TOut>
    {
        private IQueryable<TIn>? _queryable;
        private PaginationRequest? _request;
        private Expression<Func<TIn, TOrderBy>>? _orderBy;
        private OrderDirection? _direction;
        
        private PaginationBuilder() {}

        /// <summary>
        /// Creates a pagination object
        /// </summary>
        /// <returns>The next step</returns>
        public static IAddQueryableStage<TIn, TOrderBy, TOut> CreatePagination()
        {
            return new PaginationBuilder<TIn, TOrderBy, TOut>();
        }

        /// <summary>
        /// The queryable I'll paginate.
        /// </summary>
        /// <param name="queryable">The queryable</param>
        /// <returns>The next step</returns>
        public IAddRequestStage<TIn, TOrderBy, TOut> ForQueryable(IQueryable<TIn> queryable)
        {
            _queryable = queryable;
            return this;
        }

        /// <summary>
        /// The <see cref="PaginationRequest"/> I will use to generate pages.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The next step</returns>
        public IAddOrderByStage<TIn, TOrderBy, TOut> WithRequest(PaginationRequest request)
        {
            _request = request;
            return this;
        }

        /// <summary>
        /// The Expression I will use to order the data.
        /// </summary>
        /// <param name="orderedBy">The orderBy expression</param>
        /// <returns>The next step</returns>
        public IAddDirectionStage<TOut> OrderedBy(Expression<Func<TIn, TOrderBy>> orderedBy)
        {
            _orderBy = orderedBy;
            return this;
        }

        /// <summary>
        /// The direction I'll order by the data.
        /// </summary>
        /// <param name="direction">The order by direction</param>
        /// <returns>The next step</returns>
        public IGeneratePagination<TOut> WithDirection(OrderDirection direction)
        {
            _direction = direction;
            return this;
        }

        /// <summary>
        /// Generates the finished <see cref="PaginationResponse{T}"/>
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The pagination response</returns>
        public async Task<PaginationResponse<TOut>> Generate(CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(_queryable, nameof(_queryable));
            Guard.Against.Null(_orderBy, nameof(_orderBy));
            var (pageNumber, pageSize) = _request!;
            var (skip, totalPages) = PaginationProcessing.CalculatePagination(_queryable, _request);
            var data = await PaginationProcessing.QueryPaginatedAsync(_queryable, skip, pageSize, _orderBy, _direction, cancellationToken);
            var result = new PaginationResponse<TOut>(
                data.Adapt<List<TOut>>(),
                pageNumber,
                pageSize,
                totalPages,
                _queryable.Count(),
                pageNumber > 1,
                pageNumber < totalPages
            );
            return await Task.FromResult(result);
        }
    }
}