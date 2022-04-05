using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using PaginatR.Contracts;
using PaginatR.Enums;

namespace PaginatR.Process
{
    public interface IAddQueryableStage<T, TOrderBy, TOut>
    {
        public IAddRequestStage<T, TOrderBy, TOut> ForQueryable(IQueryable<T> queryable);
    }

    public interface IAddRequestStage<T, TOrderBy, TOut>
    {
        public IAddOrderByStage<T, TOrderBy, TOut> WithRequest(PaginationRequest request);
    }

    public interface IAddOrderByStage<T, TOrderBy, TOut>
    {
        public IAddDirectionStage<TOut> OrderedBy(Expression<Func<T, TOrderBy>> orderedBy);
    }

    public interface IAddDirectionStage<TOut>
    {
        public IGeneratePagination<TOut> WithDirection(OrderDirection direction);
    }

    public interface IGeneratePagination<TOut>
    {
        public Task<PaginationResponse<TOut>> Generate(CancellationToken cancellationToken);
    }
}