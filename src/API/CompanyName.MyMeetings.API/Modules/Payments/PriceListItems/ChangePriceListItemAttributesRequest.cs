namespace CompanyName.MyMeetings.API.Modules.Payments.PriceListItems
{
    /// <summary>
    /// Request model for changing the attributes of an existing price list item.
    /// </summary>
    public class ChangePriceListItemAttributesRequest
    {
        /// <summary>
        /// The unique identifier of the price list item to update.
        /// </summary>
        public Guid PriceListItemId { get; set; }

        /// <summary>
        /// The updated ISO country code.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// The updated subscription period code (e.g., "Month", "HalfYear").
        /// </summary>
        public string SubscriptionPeriodCode { get; set; }

        /// <summary>
        /// The updated subscription category code (e.g., "Standard", "Premium").
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// The updated monetary price value.
        /// </summary>
        public decimal PriceValue { get; set; }

        /// <summary>
        /// The updated ISO 4217 currency code.
        /// </summary>
        public string PriceCurrency { get; set; }
    }
}
