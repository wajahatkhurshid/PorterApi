using System.Collections.Generic;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    public class ContributorResponse<T>
    {
        public int Count { get; set; }
        public List<T> Results { get; set; }
    }
}
