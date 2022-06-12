using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    public class GetLevelsResponse
    {
        public List<Level> Levels { get; set; }
    }
}

