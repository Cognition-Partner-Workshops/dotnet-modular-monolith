using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Contracts;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.UserAccess
{
    /// <summary>
    /// Controller for retrieving system emails.
    /// Provides an endpoint to query all emails sent by the system. This endpoint is publicly accessible.
    /// </summary>
    [Route("api/userAccess/emails")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailsController"/> class.
        /// </summary>
        /// <param name="userAccessModule">The user access module used to execute queries.</param>
        public EmailsController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        /// <summary>
        /// Retrieves all system emails. This endpoint is publicly accessible.
        /// </summary>
        /// <returns>A list of all emails sent by the system.</returns>
        /// <response code="200">Returns the list of system emails.</response>
        [NoPermissionRequired]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetEmails()
        {
            var allEmails = await _userAccessModule.ExecuteQueryAsync(new GetAllEmailsQuery());

            return Ok(allEmails);
        }
    }
}
