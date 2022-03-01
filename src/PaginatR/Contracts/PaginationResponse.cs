using System.Collections.Generic;

namespace PaginatR.Contracts
{
    /// <summary>
    /// The response object
    /// </summary>
    /// <param name="Data">The enumerable data to be paginated</param>
    /// <param name="PageNumber">The page number to be displayed</param>
    /// <param name="PageSize">The per-page items size</param>
    /// <param name="TotalPages">The total pages available to paginate</param>
    /// <param name="HasPrevious">True if the list has previous pages available</param>
    /// <param name="HasNext">True if list has next pages available</param>
    /// <typeparam name="T">The type of the final response object</typeparam>
    public record PaginationResponse<T>(
        IEnumerable<T>? Data,
        int? PageNumber,
        int? PageSize,
        int? TotalPages,
        bool HasPrevious,
        bool HasNext
    );
}