namespace CompanyName.MyMeetings.API.Modules.Payments.Subscriptions
{
    /// <summary>
    /// Request model for renewing an existing subscription.
    /// </summary>
    public class RenewSubscriptionRequest
    {
        /// <summary>
        /// The subscription type code for the renewal (e.g., "Standard", "Premium").
        /// </summary>
        public string SubscriptionTypeCode { get; set; }

        /// <summary>
        /// The ISO country code for pricing.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// The monetary value of the renewal payment.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// The ISO 4217 currency code for the renewal payment (e.g., "USD", "EUR").
        /// </summary>
        public string Currency { get; set; }
    }
}
