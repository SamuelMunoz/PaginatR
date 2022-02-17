using System.Collections.Generic;

namespace PaginatR.Responses
{
    public class PaginationResponse<T>
    {
        /// <summary>
        /// The enumerable data to be paginated
        /// </summary>
        public IEnumerable<T>? Data { get; }
    
        /// <summary>
        /// The page number to be displayed
        /// </summary>
        public int? PageNumber { get; set; }
    
        /// <summary>
        /// The per page items size
        /// </summary>
        public int? PageSize { get; set; }
    
        /// <summary>
        /// The total pages available to paginate
        /// </summary>
        public int? TotalPages { get; set; }
    
        /// <summary>
        /// If the list has previous pages available
        /// </summary>
        public bool HasPrevious { get; set; }
    
        /// <summary>
        /// If the list has next pages available 
        /// </summary>
        public bool HasNext { get; set; }

        /// <summary>
        /// The response object
        /// </summary>
        /// <param name="data">The enumerable data to be paginated</param>
        public PaginationResponse(IEnumerable<T>? data)
        {
            Data = data;
        }
    }
}