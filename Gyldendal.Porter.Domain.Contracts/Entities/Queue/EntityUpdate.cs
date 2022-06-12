using System;

namespace Gyldendal.Porter.Domain.Contracts.Entities.Queue
{
    public class EntityUpdate : DomainEntityBase
    {
        public EntityUpdate(EntityType entityType, string entityId)
        {
            Id = entityId;
            EntityType = entityType;
            EntityId = entityId;
            UpdateTimestamp = DateTime.UtcNow;
        }

        public EntityType EntityType { get; set; }
        public string EntityId { get; set; }
        public DateTime UpdateTimestamp { get; set; }

        public override string ToString()
        {
            return $"Type: {EntityType.ToString()}, EntityId: {EntityId}, UpdateTimestamp: {UpdateTimestamp:O}";
        }
    }

    public enum EntityType
    {
        Work = 0,
        Product = 1,
        Series = 2,
        Contributor = 3,
        WorkReview = 4,
        MerchandiseProduct = 5
    }
}
