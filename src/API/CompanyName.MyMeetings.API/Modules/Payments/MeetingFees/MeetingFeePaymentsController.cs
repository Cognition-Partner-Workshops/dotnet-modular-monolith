using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Payments.Application.Contracts;
using CompanyName.MyMeetings.Modules.Payments.Application.MeetingFees.CreateMeetingFeePayment;
using CompanyName.MyMeetings.Modules.Payments.Application.MeetingFees.MarkMeetingFeePaymentAsPaid;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Payments.MeetingFees
{
    /// <summary>
    /// Controller for managing meeting fee payments.
    /// Provides endpoints to create and register payments for meeting attendance fees.
    /// </summary>
    [Route("api/payments/meetingFeePayments")]
    [ApiController]
    public class MeetingFeePaymentsController : ControllerBase
    {
        private readonly IPaymentsModule _meetingsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingFeePaymentsController"/> class.
        /// </summary>
        /// <param name="meetingsModule">The payments module used to execute commands.</param>
        public MeetingFeePaymentsController(IPaymentsModule meetingsModule)
        {
            _meetingsModule = meetingsModule;
        }

        /// <summary>
        /// Creates a new meeting fee payment for a specified meeting fee.
        /// </summary>
        /// <param name="request">The request containing the meeting fee identifier.</param>
        /// <returns>An OK result if the payment was successfully created.</returns>
        /// <response code="200">The meeting fee payment was successfully created.</response>
        [HttpPost]
        [HasPermission(PaymentsPermissions.RegisterPayment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateMeetingFeePayment(CreateMeetingFeePaymentRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(new CreateMeetingFeePaymentCommand(request.MeetingFeeId));

            return Ok();
        }

        /// <summary>
        /// Marks a meeting fee payment as paid (purchased).
        /// </summary>
        /// <param name="meetingFeePaymentId">The unique identifier of the meeting fee payment to mark as paid.</param>
        /// <returns>An OK result if the payment was successfully marked as paid.</returns>
        /// <response code="200">The meeting fee payment was successfully marked as paid.</response>
        [HttpPut]
        [Route("{meetingFeePaymentId}/purchased")]
        [HasPermission(PaymentsPermissions.RegisterPayment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterMeetingFeePayment(
            Guid meetingFeePaymentId)
        {
            await _meetingsModule.ExecuteCommandAsync(new MarkMeetingFeePaymentAsPaidCommand(meetingFeePaymentId));

            return Ok();
        }
    }
}
