using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Payments.Application.Contracts;
using CompanyName.MyMeetings.Modules.Payments.Application.Subscriptions.BuySubscription;
using CompanyName.MyMeetings.Modules.Payments.Application.Subscriptions.BuySubscriptionRenewal;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Payments.Subscriptions
{
    /// <summary>
    /// Controller for managing subscriptions.
    /// Provides endpoints to buy new subscriptions and renew existing ones.
    /// </summary>
    [Route("api/payments/subscriptions")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IPaymentsModule _meetingsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionsController"/> class.
        /// </summary>
        /// <param name="meetingsModule">The payments module used to execute commands.</param>
        public SubscriptionsController(IPaymentsModule meetingsModule)
        {
            _meetingsModule = meetingsModule;
        }

        /// <summary>
        /// Purchases a new subscription for the authenticated payer.
        /// </summary>
        /// <param name="request">The request containing the subscription type, country, and payment details.</param>
        /// <returns>The unique identifier of the created payment.</returns>
        /// <response code="200">Returns the payment identifier for the purchased subscription.</response>
        [HttpPost("")]
        [HasPermission(PaymentsPermissions.BuySubscription)]
        public async Task<IActionResult> BuySubscription(BuySubscriptionRequest request)
        {
            var paymentId = await _meetingsModule.ExecuteCommandAsync(
                new BuySubscriptionCommand(
                    request.SubscriptionTypeCode,
                    request.CountryCode,
                    request.Value,
                    request.Currency));

            return Ok(paymentId);
        }

        /// <summary>
        /// Renews an existing subscription.
        /// </summary>
        /// <param name="subscriptionId">The unique identifier of the subscription to renew.</param>
        /// <param name="request">The request containing the renewal subscription type, country, and payment details.</param>
        /// <returns>An Accepted result if the renewal was successfully initiated.</returns>
        /// <response code="202">The subscription renewal was successfully initiated.</response>
        [HttpPost("{subscriptionId}/renewals")]
        [HasPermission(PaymentsPermissions.RenewSubscription)]
        public async Task<IActionResult> RenewSubscription(
            Guid subscriptionId,
            RenewSubscriptionRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(
                new BuySubscriptionRenewalCommand(
                    subscriptionId,
                    request.SubscriptionTypeCode,
                    request.CountryCode,
                    request.Value,
                    request.Currency));

            return Accepted();
        }
    }
}
