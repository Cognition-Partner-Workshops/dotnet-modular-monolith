namespace CompanyName.MyMeetings.API.Modules.Payments.PriceListItems
{
    /// <summary>
    /// Request model for creating a new price list item.
    /// </summary>
    public class CreatePriceListItemRequest
    {
        /// <summary>
        /// The ISO country code the price applies to.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// The subscription period code (e.g., "Month", "HalfYear").
        /// </summary>
        public string SubscriptionPeriodCode { get; set; }

        /// <summary>
        /// The subscription category code (e.g., "Standard", "Premium").
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// The monetary price value for the subscription.
        /// </summary>
        public decimal PriceValue { get; set; }

        /// <summary>
        /// The ISO 4217 currency code for the price (e.g., "USD", "EUR").
        /// </summary>
        public string PriceCurrency { get; set; }
    }
}
