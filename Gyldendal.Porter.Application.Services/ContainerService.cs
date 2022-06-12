using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts;
using Gyldendal.Porter.Application.Services.EntityProcessing;
using Gyldendal.Porter.Application.Services.GpmSubscription;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Common.Exceptions;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Domain.Contracts.Entities.Queue;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using MediatR;
using Newtonsoft.Json;

namespace Gyldendal.Porter.Application.Services
{
    /// <summary>
    /// Responsible to add container specific data from GPM to Mongo collection
    /// </summary>
    public class ContainerService : IContainerService
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ICommonProductDataService _commonProductDataService;

        public ContainerService(IMediator mediator, ILogger logger, ICommonProductDataService commonProductDataService)
        {
            _mediator = mediator;
            _logger = logger;
            _commonProductDataService = commonProductDataService;
        }

        /// <summary>
        /// Saves the received data to a base collection and emits events to schedule background processing tasks to cook the data for the API to serve
        /// </summary>
        public async Task AddContainer(string containerId, string payload)
        {
            var containerType = GetContainerTypeByContainerTypeId(containerId);
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(5));

            switch (containerType)
            {
                case ContainerType.Product:
                    await HandleProductContainer(payload, containerType, cancellationSource);
                    break;

                case ContainerType.Work:
                    await HandleWorkContainer(payload, cancellationSource);
                    break;

                case ContainerType.Merchandise:
                    await HandleMerchandiseContainer(payload, cancellationSource);
                    break;

                case ContainerType.Contributor:
                    await HandleContributorContainer(payload, cancellationSource);
                    break;

                case ContainerType.Series:
                    await HandleSeriesContainer(payload, cancellationSource);
                    break;

                default:
                    throw new ApiException(
                        (ulong)ErrorCodes.UnsupportedContainerType,
                        $"{string.Format(ErrorCodes.UnsupportedContainerType.GetDescription(), containerType)}");
            }
        }

        private async Task HandleSeriesContainer(string payload, CancellationTokenSource cancellationSource)
        {
            var container = JsonConvert.DeserializeObject<ProductCollectionTitleContainer>(payload);
            if (container == null) throw DeserializationFailure(ContainerType.Series, payload);

            await _mediator.Send(new UpsertSeriesCommand { SeriesContainer = container }, cancellationSource.Token);
            var seriesProductIsbns = await _commonProductDataService.GetCommonSeriesProductIsbnsAsync(container.ContainerInstanceId);
            var seriesEanProductIsbns = await _commonProductDataService.GetCommonSeriesEanProductIsbnsAsync(container.ContainerInstanceId);

            await PublishUpdateEvents(EntityType.Series, new List<string> { container.ContainerInstanceId.ToString() }, cancellationSource.Token);
            await PublishUpdateEvents(EntityType.Product, seriesProductIsbns, cancellationSource.Token);
            await PublishUpdateEvents(EntityType.MerchandiseProduct, seriesEanProductIsbns, cancellationSource.Token);
        }

        private async Task HandleContributorContainer(string payload, CancellationTokenSource cancellationSource)
        {
            var container = JsonConvert.DeserializeObject<ProfileContributorAuthorContainer>(payload);
            if (container == null) throw DeserializationFailure(ContainerType.Contributor, payload);

            await _mediator.Send(new UpsertContributorCommand { ContributorContainer = container }, cancellationSource.Token);
            var contributorProductIsbns = await _commonProductDataService.GetCommonContributorProductIsbnsAsync(container.ContainerInstanceId);
            var contributorEanProductIsbns = await _commonProductDataService.GetCommonContributorEanProductIsbnsAsync(container.ContainerInstanceId);

            await PublishUpdateEvents(EntityType.Contributor, new List<string> { container.ContainerInstanceId.ToString() }, cancellationSource.Token);
            await PublishUpdateEvents(EntityType.Product, contributorProductIsbns, cancellationSource.Token);
            await PublishUpdateEvents(EntityType.MerchandiseProduct, contributorEanProductIsbns, cancellationSource.Token);
        }

        private async Task HandleMerchandiseContainer(string payload, CancellationTokenSource cancellationSource)
        {
            var container = JsonConvert.DeserializeObject<MerchandiseProductContainer>(payload);
            if (container == null) throw DeserializationFailure(ContainerType.Merchandise, payload);

            var seriesIds = container.MerchandiseCollectionTitle.Select(c => c.ContainerInstanceId.ToString());
            var contributorIds = container.MerchandiseContributorAuthor.Select(c => c.ContainerInstanceId.ToString());

            await _mediator.Send(new UpsertMerchandiseProductCommand { MerchandiseProductContainer = container }, cancellationSource.Token);

            await PublishUpdateEvents(EntityType.MerchandiseProduct, new List<string> { container.MerchandiseEan }, cancellationSource.Token);
            await PublishUpdateEvents(EntityType.Series, seriesIds, cancellationSource.Token);
            await PublishUpdateEvents(EntityType.Contributor, contributorIds, cancellationSource.Token);
        }

        private async Task HandleWorkContainer(string payload, CancellationTokenSource cancellationSource)
        {
            var container = JsonConvert.DeserializeObject<WorkContainer>(payload);
            if (container == null) throw DeserializationFailure(ContainerType.Work, payload);

            var reviewIds = container.WorkReviews.Select(c => c.ContainerInstanceId.ToString());

            await _mediator.Send(new UpsertWorkCommand { WorkContainer = container }, cancellationSource.Token);

            await PublishUpdateEvents(EntityType.Product, container.ProductIds?.Select(x => x.ToString()), cancellationSource.Token);
            await PublishUpdateEvents(EntityType.MerchandiseProduct, container.MerchandiseIds?.Select(x => x.ToString()), cancellationSource.Token);
            await PublishUpdateEvents(EntityType.WorkReview, reviewIds, cancellationSource.Token);
        }

        private async Task HandleProductContainer(string payload, ContainerType containerType, CancellationTokenSource cancellationSource)
        {
            var container = JsonConvert.DeserializeObject<ProductContainer>(payload);
            if (container == null) throw DeserializationFailure(containerType, payload);

            var seriesIds = container.ProductCollectionTitleIds?.Select(c => c.ToString()) ?? new List<string>(); // TODO: Need to ensure in future implementation that the IDs are not null
            var contributorIds = container.ProductContributorAuthorIds?.Select(c => c.ToString()) ?? new List<string>();

            await _mediator.Send(new UpsertProductCommand { ProductContainer = container }, cancellationSource.Token);

            await PublishUpdateEvents(EntityType.Product, new List<string> { container.ProductISBN13 }, cancellationSource.Token);
            await PublishUpdateEvents(EntityType.Series, seriesIds, cancellationSource.Token);
            await PublishUpdateEvents(EntityType.Contributor, contributorIds, cancellationSource.Token);
        }

        private ApiException DeserializationFailure(ContainerType type, string payload)
        {
            var message = $"Failed to deserialize the container type {type} with payload: {payload}";
            _logger.Warning(message);
            return new ApiException((ulong)ErrorCodes.UnsupportedContainerType, message);
        }

        private async Task PublishUpdateEvents(EntityType entityType, IEnumerable<string> entityIds, CancellationToken cancellationToken)
        {
            if (entityIds == null)
                return;

            foreach (var entityId in entityIds)
            {
                await _mediator.Publish(new EntityUpdateReceivedNotification(entityType, entityId), cancellationToken);
            }
        }

        private ContainerType GetContainerTypeByContainerTypeId(string containerTypeId)
        {
            return containerTypeId switch
            {
                "2" => ContainerType.Work,
                "3" => ContainerType.Product,
                //"4" => ContainerType.WorkReview,
                //"5" => ContainerType.Contributor, GPM contributor concept. We only use Profile and we refer to Profile as Contributor
                //"6" => ContainerType.ProfileMember,
                "7" => ContainerType.Contributor,
                "8" => ContainerType.Series,
                //"9" => ContainerType.PrintRun,
                //"17" => ContainerType.Attachment
                "1000" => ContainerType.Merchandise,
                //"1018" => ContainerType.MerchandiseBatch
                _ => throw new ApiException((ulong)ErrorCodes.UnsupportedContainerType,
                    $"{string.Format(ErrorCodes.UnsupportedContainerType.GetDescription(), containerTypeId)}")
            };
        }

        private ContainerType GetContainerType(string containerId)
        {
            var containerType = containerId.ToLower() switch
            {
                "workbusinessobject" => ContainerType.Work,
                "productbusinessobject" => ContainerType.Product,
                "merchandisebusinessobject" => ContainerType.Merchandise,
                _ => throw new ApiException((ulong)ErrorCodes.UnsupportedContainerId,
                    $"{string.Format(ErrorCodes.UnsupportedContainerId.GetDescription(), containerId)}")
            };

            return containerType;
        }
    }
}