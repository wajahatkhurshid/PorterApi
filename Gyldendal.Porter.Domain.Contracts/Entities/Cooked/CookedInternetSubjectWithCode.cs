using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;

namespace Gyldendal.Porter.Domain.Contracts.Entities.Cooked
{
    public class CookedInternetSubjectWithCode
    {
        public List<WebShop> WebShops { get; set; }

        public List<Area> Areas { get; set; }

        public List<Subject> Subjects { get; set; }

        public List<SubArea> SubAreas { get; set; }

        public List<SubjectCode> SubjectCodes { get; set; }
    }
}
