using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Authorization.GetAuthenticatedUserPermissions;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Authorization.GetUserPermissions;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Contracts;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Users.GetAuthenticatedUser;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Users.GetUser;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.UserAccess
{
    /// <summary>
    /// Controller for retrieving information about the currently authenticated user.
    /// Provides endpoints to get user profile details and permissions.
    /// </summary>
    [Route("api/userAccess/authenticatedUser")]
    [ApiController]
    public class AuthenticatedUserController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedUserController"/> class.
        /// </summary>
        /// <param name="userAccessModule">The user access module used to execute queries.</param>
        public AuthenticatedUserController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        /// <summary>
        /// Retrieves the profile of the currently authenticated user.
        /// </summary>
        /// <returns>The authenticated user's profile information.</returns>
        /// <response code="200">Returns the authenticated user's profile.</response>
        [NoPermissionRequired]
        [HttpGet("")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedUser()
        {
            var user = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatedUserQuery());

            return Ok(user);
        }

        /// <summary>
        /// Retrieves the permissions assigned to the currently authenticated user.
        /// </summary>
        /// <returns>A list of the authenticated user's permissions.</returns>
        /// <response code="200">Returns the list of user permissions.</response>
        [NoPermissionRequired]
        [HttpGet("permissions")]
        [ProducesResponseType(typeof(List<UserPermissionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedUserPermissions()
        {
            var permissions = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatedUserPermissionsQuery());

            return Ok(permissions);
        }
    }
}
