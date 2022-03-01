using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using PaginatR.Contracts;
using PaginatR.Enums;

namespace PaginatR.Process
{
    internal static class PaginationProcessing
    {
        /// <summary>
        /// Calculates the pagination values
        /// </summary>
        /// <param name="queryable">The queryable object</param>
        /// <param name="request">The request object</param>
        /// <typeparam name="T">The type to be used</typeparam>
        /// <returns>Tuple with skip and take to be used with LINQ</returns>
        internal static (int?, int?) CalculatePagination<T>(IQueryable<T> queryable, PaginationRequest request)
        {
            var (pageNumber, pageSize) = request;
            var skip = (pageNumber - 1) * pageSize;
            var totalPages = (int) Math.Ceiling(queryable.Count() / (double) (pageSize ?? 15));
            return (skip, totalPages);
        }

        /// <summary>
        /// Queries asynchronously the pagination queryable object
        /// </summary>
        /// <param name="queryable">The queryable object</param>
        /// <param name="skip">How many records will be skipped</param>
        /// <param name="pageSize">And how many record will be retrieved</param>
        /// <param name="orderBy">The orderBy column</param>
        /// <param name="direction">The orderBy direction</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <typeparam name="T">The type</typeparam>
        /// <typeparam name="TOrderBy">The order by type</typeparam>
        /// <returns>A task-based list of results</returns>
        internal static Task<List<T>> QueryPaginatedAsync<T, TOrderBy>(IQueryable<T> queryable, int? skip, int? pageSize,
            Expression<Func<T, TOrderBy>> orderBy, OrderDirection direction, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<List<T>>(cancellationToken);
            }

            Guard.Against.Null(queryable, nameof(queryable));
            Guard.Against.Null(skip, nameof(skip));
            Guard.Against.Null(pageSize, nameof(pageSize));

            var data = queryable
                .OrderBy(orderBy)
                .Skip((int) skip)
                .Take((int) pageSize)
                .ToList();

            if (direction == OrderDirection.Descending)
            {
                data = queryable
                    .OrderByDescending(orderBy)
                    .Skip((int) skip)
                    .Take((int) pageSize)
                    .ToList();
            }

            return Task.FromResult(data);
        }
    }
}