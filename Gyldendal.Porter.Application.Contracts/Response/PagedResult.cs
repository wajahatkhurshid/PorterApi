using System.Collections.Generic;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    /// <summary>
    /// Generic object for returning the paged information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T>
    {
        public long TotalResults { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<T> Results { get; set; }
    }
}
