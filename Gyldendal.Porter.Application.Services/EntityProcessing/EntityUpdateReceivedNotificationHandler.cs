using System;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Domain.Contracts.Entities.Queue;
using Hangfire;
using MediatR;

namespace Gyldendal.Porter.Application.Services.EntityProcessing
{
    public class EntityUpdateReceivedNotificationHandler : INotificationHandler<EntityUpdateReceivedNotification>
    {
        private readonly ILogger _logger;
        private readonly IBackgroundJobClient _jobClient;

        public EntityUpdateReceivedNotificationHandler(ILogger logger, IBackgroundJobClient jobClient)
        {
            _logger = logger;
            _jobClient = jobClient;
        }

        public Task Handle(EntityUpdateReceivedNotification notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Info($"Enqueuing entity update. Id: {notification.EntityId}, type: {notification.EntityType}");
                switch (notification.EntityType)
                {
                    case EntityType.Product:
                        _jobClient.Enqueue<IProductCookingService>(m =>
                            m.Cook(notification.EntityId, cancellationToken));
                        break;
                    case EntityType.Series:
                        _jobClient.Enqueue<ISeriesCookingService>(m =>
                            m.Cook(int.Parse(notification.EntityId), cancellationToken));
                        break;
                    case EntityType.Contributor:
                        _jobClient.Enqueue<IContributorCookingService>(m =>
                            m.Cook(int.Parse(notification.EntityId), cancellationToken));
                        break;
                    case EntityType.WorkReview:
                        _jobClient.Enqueue<IWorkReviewCookingService>(m =>
                            m.Cook(int.Parse(notification.EntityId), cancellationToken));
                        break;
                    case EntityType.MerchandiseProduct:
                        _jobClient.Enqueue<IMerchandiseProductCookingService>(m =>
                            m.Cook(notification.EntityId, cancellationToken));
                        break;
                    case EntityType.Work:
                    default:
                        throw new ArgumentException($"Unsupported entity update type: {notification.EntityType}");
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.Error(
                    $"{Environment.MachineName} - Failed to enqueue task for notification: {notification.EntityId}, {notification.EntityType}",
                    ex);
                return Task.FromException(ex);
            }
        }
    }
}