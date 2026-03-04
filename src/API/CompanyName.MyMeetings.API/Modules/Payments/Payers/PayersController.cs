using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Payments.Application.Contracts;
using CompanyName.MyMeetings.Modules.Payments.Application.Subscriptions.GetPayerSubscription;
using CompanyName.MyMeetings.Modules.Payments.Application.Subscriptions.GetSubscriptionDetails;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Payments.Payers
{
    /// <summary>
    /// Controller for managing payer information and their subscriptions.
    /// </summary>
    [Route("api/payments/payers")]
    [ApiController]
    public class PayersController : ControllerBase
    {
        private readonly IPaymentsModule _paymentsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="PayersController"/> class.
        /// </summary>
        /// <param name="paymentsModule">The payments module used to execute queries.</param>
        public PayersController(IPaymentsModule paymentsModule)
        {
            _paymentsModule = paymentsModule;
        }

        /// <summary>
        /// Retrieves the subscription details for the authenticated payer.
        /// </summary>
        /// <returns>The subscription details of the authenticated payer.</returns>
        /// <response code="200">Returns the authenticated payer's subscription details.</response>
        [HttpGet("authenticated/subscription")]
        [HasPermission(PaymentsPermissions.GetAuthenticatedPayerSubscription)]
        [ProducesResponseType(typeof(SubscriptionDetailsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedPayerSubscription()
        {
            var subscription = await _paymentsModule.ExecuteQueryAsync(new GetAuthenticatedPayerSubscriptionQuery());

            return Ok(subscription);
        }
    }
}
