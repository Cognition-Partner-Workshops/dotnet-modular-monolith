namespace CompanyName.MyMeetings.API.Modules.Payments.MeetingFees
{
    /// <summary>
    /// Request model for creating a meeting fee payment.
    /// </summary>
    public class CreateMeetingFeePaymentRequest
    {
        /// <summary>
        /// The unique identifier of the meeting fee to create a payment for.
        /// </summary>
        public Guid MeetingFeeId { get; set; }
    }
}
