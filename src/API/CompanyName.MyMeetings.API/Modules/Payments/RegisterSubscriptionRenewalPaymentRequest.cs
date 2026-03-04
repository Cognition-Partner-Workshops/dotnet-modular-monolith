namespace CompanyName.MyMeetings.API.Modules.Payments
{
    /// <summary>
    /// Request model for registering a subscription renewal payment as paid.
    /// </summary>
    public class RegisterSubscriptionRenewalPaymentRequest
    {
        /// <summary>
        /// The unique identifier of the renewal payment to mark as paid.
        /// </summary>
        public Guid PaymentId { get; set; }
    }
}
