using System;
using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.Entities
{
    public class Contributor : DomainEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

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
