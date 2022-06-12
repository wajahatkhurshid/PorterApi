using System;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    public class GetWorkReviewUpdateInfoResponse
    {
        public string WorkReviewId { get; set; }

        public bool UpdateType { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
