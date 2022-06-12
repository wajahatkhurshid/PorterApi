using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Application.Services.GpmSubscription;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gyldendal.Porter.Api.Controllers
{
    /// <summary>
    ///  Gpm Subscription Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class GpmSubscriptionController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly IContainerService _containerService;
        private readonly IMediator _mediator;
        private readonly IReceiveGpmMessageService _receiveGpmMessageService;

        /// <summary>
        /// Responsible for anything related to the GPM subscriptions
        /// </summary>
        public GpmSubscriptionController(ILogger logger, IContainerService containerService, IMediator mediator, IReceiveGpmMessageService receiveGpmMessageService)
        {
            _logger = logger;
            _containerService = containerService;
            _mediator = mediator;
            _receiveGpmMessageService = receiveGpmMessageService;
        }

        /// <summary>
        /// Used by GPM to push new container messages
        /// </summary>
        /// <param name="containerPayload">JSON payload with container data</param>
        /// <returns>200 if message acknowledged</returns>
        [HttpPut]
        public async Task<ActionResult> AddContainerMessage([FromBody] string containerPayload)
        {
            _logger.Info($"Container message - {containerPayload}", isGdprSafe: true);
            //Request.Headers.Add("businessobjectname", "ProductBusinessObject");
            var containerTypeExists = Request.Headers.TryGetValue("businessobjectname", out var containerId);

            if (!containerTypeExists)
            {
                _logger.Info("BAD REQUEST: Invalid Container received", isGdprSafe: true);
                return BadRequest("Container Type doesn't exists.");
            }

            await _containerService.AddContainer(containerId, containerPayload);

            return Ok();
        }

        /// <summary>
        /// Create subscription and scopes in GPM. All subscription scopes are created as disabled, so remember to go and enable them in the UI one by one to avoid overloading GPM.
        /// </summary>
        /// <param name="porterEnvironment">Postfix added to the subscription name</param>
        /// <param name="subscriptionName">Not recommended to use, but you can change your subscription name here. Recommended to leave empty.</param>
        [HttpPost]
        [ProducesResponseType(typeof(GpmSubscriptionResponse), StatusCodes.Status200OK)]
        [Route("api/v1/CreateSubscription")]
        public async Task<ActionResult> CreateSubscription()
        {
            var result = await _mediator.Send(new CreateGpmSubscriptionCommand(), CancellationToken.None);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<Subscription>), StatusCodes.Status200OK)]
        [Route("api/v1/GetSubscription")]
        public async Task<ActionResult> GetSubscription()
        {
            var result = await _mediator.Send(new GetGpmSubscriptionCommand(), CancellationToken.None);
            return Ok(result);
        }

        /// <summary>
        /// Triggers a reload of GPM data
        /// </summary>
        /// <param name="subscriptionId">Subscription to trigger reload for</param>
        /// <param name="shouldWipeCollections">Clear collection if set</param>
        /// <returns>True if successful</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("api/v1/TriggerReplay")]
        public async Task<ActionResult> TriggerReplay(int subscriptionId, bool shouldWipeCollections)
        {
            var isSuccessful = await _mediator.Send(new TriggerReplayCommand { SubscriptionId = subscriptionId, ShouldWipeCollections = shouldWipeCollections }, CancellationToken.None);
            return Ok(isSuccessful);
        }

        /// <summary>
        /// Test method to trigger receiving of GPM azure bus messages. It will be removed and possibly replaced by a hangfire job
        /// </summary>
        /// <returns>True if successful</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("api/v1/ReceiveGpmServiceBusMessage")]
        public async Task<ActionResult> ReceiveGpmServiceBusMessage()
        {
            await _receiveGpmMessageService.Receive(CancellationToken.None);
            return Ok();
        }
    }
}