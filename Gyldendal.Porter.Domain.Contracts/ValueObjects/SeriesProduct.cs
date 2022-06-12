using System;
using System.Collections.Generic;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects
{
    public class SeriesProduct
    {
        public DateTime UpdatedTimestamp { get; set; }
        public List<ProductCollectionTitleContainer> Series { get; set; }
        public List<List<GpmNode>> EducationSubjectLevels { get; set; }
    }
}
