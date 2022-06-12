using System;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    public class GetContributorsUpdateInfoResponse
    {
        public string Id { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
