using System;

namespace Gyldendal.Porter.Domain.Contracts.Entities
{
    public class ProductStock : DomainEntityBase
    {
        public string Isbn { get; set; }
        public int AvailableStock { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
