namespace CompanyName.MyMeetings.API.Modules.Payments.PriceListItems
{
    /// <summary>
    /// Query parameters for retrieving a specific price list item.
    /// </summary>
    public class GetPriceListItemRequest
    {
        /// <summary>
        /// The ISO country code to filter by.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// The subscription category code to filter by.
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// The subscription period type code to filter by.
        /// </summary>
        public string PeriodTypeCode { get; set; }
    }
}
