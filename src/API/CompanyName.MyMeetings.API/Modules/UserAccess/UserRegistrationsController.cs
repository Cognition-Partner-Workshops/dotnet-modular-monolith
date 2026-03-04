using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Registrations.Application.Contracts;
using CompanyName.MyMeetings.Modules.Registrations.Application.UserRegistrations.ConfirmUserRegistration;
using CompanyName.MyMeetings.Modules.Registrations.Application.UserRegistrations.RegisterNewUser;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.UserAccess
{
    /// <summary>
    /// Controller for managing user registrations.
    /// Provides endpoints for new user sign-up and registration confirmation.
    /// These endpoints are publicly accessible (no authentication required).
    /// </summary>
    [Route("userAccess/[controller]")]
    [ApiController]
    public class UserRegistrationsController : ControllerBase
    {
        private readonly IRegistrationsModule _registrationsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegistrationsController"/> class.
        /// </summary>
        /// <param name="registrationsModule">The registrations module used to execute commands.</param>
        public UserRegistrationsController(IRegistrationsModule registrationsModule)
        {
            _registrationsModule = registrationsModule;
        }

        /// <summary>
        /// Registers a new user account. This endpoint is publicly accessible.
        /// </summary>
        /// <param name="request">The request containing the new user's registration details.</param>
        /// <returns>An OK result if the registration was successfully submitted.</returns>
        /// <response code="200">The user registration was successfully submitted.</response>
        [NoPermissionRequired]
        [AllowAnonymous]
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterNewUser(RegisterNewUserRequest request)
        {
            await _registrationsModule.ExecuteCommandAsync(new RegisterNewUserCommand(
                request.Login,
                request.Password,
                request.Email,
                request.FirstName,
                request.LastName,
                request.ConfirmLink));

            return Ok();
        }

        /// <summary>
        /// Confirms a pending user registration. This endpoint is publicly accessible.
        /// </summary>
        /// <param name="userRegistrationId">The unique identifier of the user registration to confirm.</param>
        /// <returns>An OK result if the registration was successfully confirmed.</returns>
        /// <response code="200">The user registration was successfully confirmed.</response>
        [NoPermissionRequired]
        [AllowAnonymous]
        [HttpPatch("{userRegistrationId}/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmRegistration(Guid userRegistrationId)
        {
            await _registrationsModule.ExecuteCommandAsync(new ConfirmUserRegistrationCommand(userRegistrationId));

            return Ok();
        }
    }
}
