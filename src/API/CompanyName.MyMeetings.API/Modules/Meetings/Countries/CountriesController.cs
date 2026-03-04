using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Meetings.Application.Contracts;
using CompanyName.MyMeetings.Modules.Meetings.Application.Countries;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Meetings.Countries
{
    /// <summary>
    /// Controller for retrieving country reference data used within the Meetings module.
    /// </summary>
    [Route("api/meetings/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IMeetingsModule _meetingsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesController"/> class.
        /// </summary>
        /// <param name="meetingsModule">The meetings module used to execute queries.</param>
        public CountriesController(IMeetingsModule meetingsModule)
        {
            _meetingsModule = meetingsModule;
        }

        /// <summary>
        /// Retrieves all available countries.
        /// </summary>
        /// <param name="page">Optional page number for pagination.</param>
        /// <param name="perPage">Optional number of items per page.</param>
        /// <returns>A list of all countries.</returns>
        /// <response code="200">Returns the list of countries.</response>
        [HttpGet("")]
        [HasPermission(MeetingsPermissions.GetMeetingGroupProposals)]
        [ProducesResponseType(typeof(List<CountryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCountries(int? page, int? perPage)
        {
            var countries = await _meetingsModule.ExecuteQueryAsync(
                new GetAllCountriesQuery());

            return Ok(countries);
        }
    }
}
