using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Taxonomy
{
    public class SortAndPaginate
    {
        public SortBy SortBy { get; set; }

        public string OrderBy { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
