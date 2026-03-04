namespace CompanyName.MyMeetings.API.Modules.Payments.Subscriptions
{
    /// <summary>
    /// Request model for buying a new subscription.
    /// </summary>
    public class BuySubscriptionRequest
    {
        /// <summary>
        /// The subscription type code (e.g., "Standard", "Premium").
        /// </summary>
        public string SubscriptionTypeCode { get; set; }

        /// <summary>
        /// The ISO country code for pricing.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// The monetary value of the payment.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// The ISO 4217 currency code for the payment (e.g., "USD", "EUR").
        /// </summary>
        public string Currency { get; set; }
    }
}
