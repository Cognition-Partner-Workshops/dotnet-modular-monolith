namespace CompanyName.MyMeetings.API.Modules.Payments.MeetingFees
{
    /// <summary>
    /// Request model for registering a meeting fee payment.
    /// </summary>
    public class RegisterMeetingFeePaymentRequest
    {
        /// <summary>
        /// The unique identifier of the payment to register.
        /// </summary>
        public Guid PaymentId { get; set; }
    }
}
