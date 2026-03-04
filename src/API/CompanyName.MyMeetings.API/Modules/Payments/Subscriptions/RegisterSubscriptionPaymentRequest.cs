namespace CompanyName.MyMeetings.API.Modules.Payments.Subscriptions
{
    /// <summary>
    /// Request model for registering a subscription payment as paid.
    /// </summary>
    public class RegisterSubscriptionPaymentRequest
    {
        /// <summary>
        /// The unique identifier of the payment to mark as paid.
        /// </summary>
        public Guid PaymentId { get; set; }
    }
}
