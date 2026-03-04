using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Payments.Application.Contracts;
using CompanyName.MyMeetings.Modules.Payments.Application.PriceListItems.ActivatePriceListItem;
using CompanyName.MyMeetings.Modules.Payments.Application.PriceListItems.ChangePriceListItemAttributes;
using CompanyName.MyMeetings.Modules.Payments.Application.PriceListItems.CreatePriceListItem;
using CompanyName.MyMeetings.Modules.Payments.Application.PriceListItems.DeactivatePriceListItem;
using CompanyName.MyMeetings.Modules.Payments.Application.PriceListItems.GetPriceListItem;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Payments.PriceListItems
{
    /// <summary>
    /// Controller for managing subscription price list items.
    /// Provides endpoints to create, query, activate, deactivate, and update price list items.
    /// </summary>
    [ApiController]
    [Route("api/payments/priceListItems")]
    public class PriceListItemsController : ControllerBase
    {
        private readonly IPaymentsModule _paymentsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceListItemsController"/> class.
        /// </summary>
        /// <param name="paymentsModule">The payments module used to execute commands and queries.</param>
        public PriceListItemsController(IPaymentsModule paymentsModule)
        {
            _paymentsModule = paymentsModule;
        }

        /// <summary>
        /// Retrieves a price list item by country, category, and period type.
        /// </summary>
        /// <param name="request">The query parameters including country code, category code, and period type code.</param>
        /// <returns>The price list item matching the specified criteria.</returns>
        /// <response code="200">Returns the matching price list item.</response>
        [HttpGet]
        [HasPermission(PaymentsPermissions.GetPriceListItem)]
        [ProducesResponseType(typeof(PriceListItemMoneyValueDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPriceListItem([FromQuery] GetPriceListItemRequest request)
        {
            var priceListItem = await _paymentsModule.ExecuteQueryAsync(new GetPriceListItemQuery(
                request.CountryCode,
                request.CategoryCode,
                request.PeriodTypeCode));

            return Ok(priceListItem);
        }

        /// <summary>
        /// Creates a new price list item for a subscription type, country, and category.
        /// </summary>
        /// <param name="request">The request containing the price list item details.</param>
        /// <returns>An OK result if the price list item was successfully created.</returns>
        /// <response code="200">The price list item was successfully created.</response>
        [HttpPost]
        [HasPermission(PaymentsPermissions.CreatePriceListItem)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePriceListItem([FromBody] CreatePriceListItemRequest request)
        {
            await _paymentsModule.ExecuteCommandAsync(new CreatePriceListItemCommand(
                request.SubscriptionPeriodCode,
                request.CategoryCode,
                request.CountryCode,
                request.PriceValue,
                request.PriceCurrency));

            return Ok();
        }

        /// <summary>
        /// Activates a price list item, making it available for use.
        /// </summary>
        /// <param name="priceListItemId">The unique identifier of the price list item to activate.</param>
        /// <returns>An OK result if the price list item was successfully activated.</returns>
        /// <response code="200">The price list item was successfully activated.</response>
        [HttpPatch("{priceListItemId}/activate")]
        [HasPermission(PaymentsPermissions.ActivatePriceListItem)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ActivatePriceListItem([FromRoute] Guid priceListItemId)
        {
            await _paymentsModule.ExecuteCommandAsync(new ActivatePriceListItemCommand(priceListItemId));

            return Ok();
        }

        /// <summary>
        /// Deactivates a price list item, removing it from active use.
        /// </summary>
        /// <param name="priceListItemId">The unique identifier of the price list item to deactivate.</param>
        /// <returns>An OK result if the price list item was successfully deactivated.</returns>
        /// <response code="200">The price list item was successfully deactivated.</response>
        [HttpPatch("{priceListItemId}/deactivate")]
        [HasPermission(PaymentsPermissions.DeactivatePriceListItem)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeactivatePriceListItem([FromRoute] Guid priceListItemId)
        {
            await _paymentsModule.ExecuteCommandAsync(new DeactivatePriceListItemCommand(priceListItemId));

            return Ok();
        }

        /// <summary>
        /// Updates the attributes of an existing price list item.
        /// </summary>
        /// <param name="request">The request containing the updated price list item attributes.</param>
        /// <returns>An OK result if the price list item attributes were successfully updated.</returns>
        /// <response code="200">The price list item attributes were successfully updated.</response>
        [HttpPut]
        [HasPermission(PaymentsPermissions.ChangePriceListItemAttributes)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePriceListItemAttributes(
            [FromBody] ChangePriceListItemAttributesRequest request)
        {
            await _paymentsModule.ExecuteCommandAsync(new ChangePriceListItemAttributesCommand(
                request.PriceListItemId,
                request.CountryCode,
                request.SubscriptionPeriodCode,
                request.CategoryCode,
                request.PriceValue,
                request.PriceCurrency));

            return Ok();
        }
    }
}
