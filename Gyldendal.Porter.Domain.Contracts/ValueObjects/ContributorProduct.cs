using System;
using System.Collections.Generic;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects
{
    public class ContributorProduct
    {
        public DateTime UpdatedTimestamp { get; set; }
        public List<List<GpmNode>> Websites { get; set; }
        public List<ProfileContributorAuthorContainer> ContributorAuthors { get; set; }
    }
}
