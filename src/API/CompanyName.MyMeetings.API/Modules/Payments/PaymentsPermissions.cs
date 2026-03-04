namespace CompanyName.MyMeetings.API.Modules.Payments
{
    /// <summary>
    /// Defines permission constants used by the Payments module to authorize API actions.
    /// </summary>
    public class PaymentsPermissions
    {
        /// <summary>Permission required to register a payment.</summary>
        public const string RegisterPayment = "RegisterPayment";

        /// <summary>Permission required to buy a subscription.</summary>
        public const string BuySubscription = "BuySubscription";

        /// <summary>Permission required to renew a subscription.</summary>
        public const string RenewSubscription = "RenewSubscription";

        /// <summary>Permission required to create a price list item.</summary>
        public const string CreatePriceListItem = "CreatePriceListItem";

        /// <summary>Permission required to activate a price list item.</summary>
        public const string ActivatePriceListItem = "ActivatePriceListItem";

        /// <summary>Permission required to deactivate a price list item.</summary>
        public const string DeactivatePriceListItem = "DeactivatePriceListItem";

        /// <summary>Permission required to change price list item attributes.</summary>
        public const string ChangePriceListItemAttributes = "ChangePriceListItemAttributes";

        /// <summary>Permission required to get the authenticated payer's subscription.</summary>
        public const string GetAuthenticatedPayerSubscription = "GetAuthenticatedPayerSubscription";

        /// <summary>Permission required to get a price list item.</summary>
        public const string GetPriceListItem = "GetPriceListItem";
    }
}
