using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    public class GetProductAttachmentsResponse
    {
        public List<Attachment> Attachments { get; set; }
    }
}

