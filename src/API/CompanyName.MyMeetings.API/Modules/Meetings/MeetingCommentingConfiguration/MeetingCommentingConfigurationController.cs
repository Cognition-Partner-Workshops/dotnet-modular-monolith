using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Meetings.Application.Contracts;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingCommentingConfigurations.DisableMeetingCommentingConfiguration;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingCommentingConfigurations.EnableMeetingCommentingConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Meetings.MeetingCommentingConfiguration
{
    /// <summary>
    /// Controller for managing the commenting configuration of a specific meeting.
    /// Allows enabling or disabling the ability for members to post comments on a meeting.
    /// </summary>
    [Route("api/meetings/meetings/{meetingId}/configuration/commenting")]
    [ApiController]
    public class MeetingCommentingConfigurationController : ControllerBase
    {
        private readonly IMeetingsModule _meetingsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingCommentingConfigurationController"/> class.
        /// </summary>
        /// <param name="meetingsModule">The meetings module used to execute commands.</param>
        public MeetingCommentingConfigurationController(IMeetingsModule meetingsModule)
        {
            _meetingsModule = meetingsModule;
        }

        /// <summary>
        /// Disables commenting on the specified meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <returns>An OK result if commenting was successfully disabled.</returns>
        /// <response code="200">Commenting was successfully disabled for the meeting.</response>
        [HttpPatch("disable")]
        [HasPermission(MeetingsPermissions.DisableMeetingCommenting)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DisableCommenting(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new DisableMeetingCommentingConfigurationCommand(meetingId));
            return Ok();
        }

        /// <summary>
        /// Enables commenting on the specified meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <returns>An OK result if commenting was successfully enabled.</returns>
        /// <response code="200">Commenting was successfully enabled for the meeting.</response>
        [HttpPatch("enable")]
        [HasPermission(MeetingsPermissions.EnableMeetingCommenting)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EnableCommenting(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new EnableMeetingCommentingConfigurationCommand(meetingId));
            return Ok();
        }
    }
}
