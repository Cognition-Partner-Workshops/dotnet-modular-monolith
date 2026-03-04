using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Payments.Application.Contracts;
using CompanyName.MyMeetings.Modules.Payments.Application.Subscriptions.MarkSubscriptionRenewalPaymentAsPaid;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Payments
{
    /// <summary>
    /// Controller for managing subscription renewal payment registrations.
    /// Provides an endpoint to mark subscription renewal payments as paid.
    /// </summary>
    [Route("api/payments/subscriptionRenewals")]
    [ApiController]
    public class SubscriptionRenewalsController : ControllerBase
    {
        private readonly IPaymentsModule _paymentsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionRenewalsController"/> class.
        /// </summary>
        /// <param name="paymentsModule">The payments module used to execute commands.</param>
        public SubscriptionRenewalsController(IPaymentsModule paymentsModule)
        {
            _paymentsModule = paymentsModule;
        }

        /// <summary>
        /// Marks a subscription renewal payment as paid.
        /// </summary>
        /// <param name="request">The request containing the payment identifier.</param>
        /// <returns>An OK result if the subscription renewal payment was successfully marked as paid.</returns>
        /// <response code="200">The subscription renewal payment was successfully marked as paid.</response>
        [HttpPost]
        [HasPermission(PaymentsPermissions.RegisterPayment)]
        public async Task<IActionResult> RegisterSubscriptionPayment(RegisterSubscriptionRenewalPaymentRequest request)
        {
            await _paymentsModule.ExecuteCommandAsync(
                new MarkSubscriptionRenewalPaymentAsPaidCommand(request.PaymentId));

            return Ok();
        }
    }
}
