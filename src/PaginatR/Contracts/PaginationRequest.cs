namespace PaginatR.Contracts
{
    /// <summary>
    /// The pagination request object
    /// </summary>
    /// <param name="PageNumber">The page number to be requested</param>
    /// <param name="PageSize">The size of the pages</param>
    public record PaginationRequest(int? PageNumber = 1, int? PageSize = 15);
}