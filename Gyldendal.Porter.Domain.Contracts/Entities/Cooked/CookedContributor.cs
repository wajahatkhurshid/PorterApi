using System;
using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Domain.Contracts.Entities.Cooked
{
    public class CookedContributor : DomainEntityBase
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Biographical text about the author
        /// </summary>
        public string BiographyText { get; set; }

        /// <summary>
        /// Bibliography text about the author
        /// </summary>
        public string Bibliography { get; set; }

        /// <summary>
        /// Link to an image of the author
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// GPM is implementing this type in their contributor register. Type can be author, designer of the cover etc.
        /// </summary>
        public int ContributorTypeId { get; set; }

        public List<WebShop> WebShops { get; set; }

        /// <summary>
        /// When an update was last received on the contributor
        /// </summary>
        public DateTime UpdatedTimestamp { get; set; }

        /// <summary>
        /// Marks if the contributor's data should be deleted
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
