using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    public class GetSubjectsResponse
    {
        public List<Subject> Subjects { get; set; }
    }
}

