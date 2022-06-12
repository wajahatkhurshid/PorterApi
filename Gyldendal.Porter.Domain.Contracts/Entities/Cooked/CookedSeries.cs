using System;
using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;

namespace Gyldendal.Porter.Domain.Contracts.Entities.Cooked
{
    public class CookedSeries: DomainEntityBase
    {
        public int? ParentSerieId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public List<Area> Areas { get; set; }

        public List<SubArea> SubAreas { get; set; }

        public List<EducationLevel> EducationLevels { get; set; }

        public List<Subject> Subjects { get; set; }

        public string ImageUrl { get; set; }

        public DateTime UpdatedTimestamp { get; set; }

        public bool IsSystemSeries { get; set; }

        public List<WebShop> WebShops { get; set; }

        public CookedSeries ParentSeries { get; set; }

        public List<CookedSeries> ChildSeries { get; set; }
    }
}
