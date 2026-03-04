using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Payments.Application.Contracts;
using CompanyName.MyMeetings.Modules.Payments.Application.Subscriptions.MarkSubscriptionPaymentAsPaid;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Payments.Subscriptions
{
    /// <summary>
    /// Controller for managing subscription payment registrations.
    /// Provides an endpoint to mark subscription payments as paid.
    /// </summary>
    [Route("api/payments/subscriptionPayments")]
    [ApiController]
    public class SubscriptionPaymentsController : ControllerBase
    {
        private readonly IPaymentsModule _meetingsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionPaymentsController"/> class.
        /// </summary>
        /// <param name="meetingsModule">The payments module used to execute commands.</param>
        public SubscriptionPaymentsController(IPaymentsModule meetingsModule)
        {
            _meetingsModule = meetingsModule;
        }

        /// <summary>
        /// Marks a subscription payment as paid.
        /// </summary>
        /// <param name="request">The request containing the payment identifier.</param>
        /// <returns>An OK result if the subscription payment was successfully marked as paid.</returns>
        /// <response code="200">The subscription payment was successfully marked as paid.</response>
        [HttpPost]
        [HasPermission(PaymentsPermissions.RegisterPayment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterSubscriptionPayment(RegisterSubscriptionPaymentRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(new MarkSubscriptionPaymentAsPaidCommand(request.PaymentId));

            return Ok();
        }
    }
}
