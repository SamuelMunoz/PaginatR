namespace PaginatR.Requests;

/// <summary>
/// The pagination request object
/// </summary>
public class PaginationRequest
{
    /// <summary>
    /// The page number to request
    /// </summary>
    public int? PageNumber { get; }
    
    /// <summary>
    /// The total number of items per page
    /// </summary>
    public int? PageSize { get; }

    /// <summary>
    /// The pagination request object
    /// </summary>
    public PaginationRequest() : this(1, 15)
    {
        
    }

    /// <summary>
    /// The pagination request object
    /// </summary>
    /// <param name="pageNumber">The page number to request</param>
    /// <param name="pageSize">The total number of items per page</param>
    public PaginationRequest(int? pageNumber, int? pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    /// <summary>
    /// The request object
    /// </summary>
    /// <param name="pageNumber">The page number to request</param>
    /// <param name="pageSize">The total number of items per page</param>
    public void Deconstruct(out int? pageNumber, out int? pageSize)
    {
        pageNumber = PageNumber;
        pageSize = PageSize;
    }
}