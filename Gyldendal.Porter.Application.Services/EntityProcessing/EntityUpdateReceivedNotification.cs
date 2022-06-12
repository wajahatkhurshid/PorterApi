using Gyldendal.Porter.Domain.Contracts.Entities.Queue;
using MediatR;

namespace Gyldendal.Porter.Application.Services.EntityProcessing
{
    public class EntityUpdateReceivedNotification : INotification
    {
        public EntityUpdateReceivedNotification(EntityType entityType, string entityId)
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        public EntityType EntityType { get; set; }
        public string EntityId { get; set; }
    }
}