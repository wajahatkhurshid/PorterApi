using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    public class GetSeriesPaginatedResponse
    {
        public List<Series> Series { get; set; }

        public int Count { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
