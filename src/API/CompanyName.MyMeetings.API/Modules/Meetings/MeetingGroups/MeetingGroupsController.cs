using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Meetings.Application.Contracts;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingGroups.EditMeetingGroupGeneralAttributes;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingGroups.GetAllMeetingGroups;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingGroups.GetAuthenticationMemberMeetingGroups;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingGroups.GetMeetingGroupDetails;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingGroups.JoinToGroup;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingGroups.LeaveMeetingGroup;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Meetings.MeetingGroups
{
    /// <summary>
    /// Controller for managing meeting groups.
    /// Provides endpoints to view, edit, join, and leave meeting groups.
    /// </summary>
    [Route("api/meetings/[controller]")]
    [ApiController]
    public class MeetingGroupsController : ControllerBase
    {
        private readonly IMeetingsModule _meetingsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingGroupsController"/> class.
        /// </summary>
        /// <param name="meetingsModule">The meetings module used to execute commands and queries.</param>
        public MeetingGroupsController(IMeetingsModule meetingsModule)
        {
            _meetingsModule = meetingsModule;
        }

        /// <summary>
        /// Retrieves the meeting groups that the authenticated member belongs to.
        /// </summary>
        /// <returns>A list of the authenticated member's meeting groups.</returns>
        /// <response code="200">Returns the list of meeting groups for the authenticated member.</response>
        [HttpGet("")]
        [HasPermission(MeetingsPermissions.GetAuthenticatedMemberMeetingGroups)]
        [ProducesResponseType(typeof(List<MemberMeetingGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedMemberMeetingGroups()
        {
            var meetingGroups = await _meetingsModule.ExecuteQueryAsync(
                new GetAuthenticationMemberMeetingGroupsQuery());

            return Ok(meetingGroups);
        }

        /// <summary>
        /// Retrieves the details of a specific meeting group.
        /// </summary>
        /// <param name="meetingGroupId">The unique identifier of the meeting group.</param>
        /// <returns>The detailed information about the meeting group.</returns>
        /// <response code="200">Returns the meeting group details.</response>
        [HttpGet("{meetingGroupId}")]
        [HasPermission(MeetingsPermissions.GetMeetingGroupDetails)]
        [ProducesResponseType(typeof(MeetingGroupDetailsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeetingGroupDetails(Guid meetingGroupId)
        {
            var meetingGroupDetails = await _meetingsModule.ExecuteQueryAsync(
                new GetMeetingGroupDetailsQuery(meetingGroupId));

            return Ok(meetingGroupDetails);
        }

        /// <summary>
        /// Retrieves all meeting groups in the system.
        /// </summary>
        /// <returns>A list of all meeting groups.</returns>
        /// <response code="200">Returns the list of all meeting groups.</response>
        [HttpGet("all")]
        [HasPermission(MeetingsPermissions.GetAllMeetingGroups)]
        [ProducesResponseType(typeof(List<MeetingGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMeetingGroups()
        {
            var meetingGroups = await _meetingsModule.ExecuteQueryAsync(new GetAllMeetingGroupsQuery());

            return Ok(meetingGroups);
        }

        /// <summary>
        /// Updates the general attributes of a meeting group.
        /// </summary>
        /// <param name="meetingGroupId">The unique identifier of the meeting group to edit.</param>
        /// <param name="request">The request containing the updated meeting group attributes.</param>
        /// <returns>An OK result if the meeting group was successfully updated.</returns>
        /// <response code="200">The meeting group attributes were successfully updated.</response>
        [HttpPut("{meetingGroupId}")]
        [HasPermission(MeetingsPermissions.EditMeetingGroupGeneralAttributes)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditMeetingGroupGeneralAttributes(
            [FromRoute] Guid meetingGroupId,
            [FromBody] EditMeetingGroupGeneralAttributesRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(new EditMeetingGroupGeneralAttributesCommand(
                meetingGroupId,
                request.Name,
                request.Description,
                request.LocationCity,
                request.LocationCountry));

            return Ok();
        }

        /// <summary>
        /// Adds the authenticated member to a meeting group.
        /// </summary>
        /// <param name="meetingGroupId">The unique identifier of the meeting group to join.</param>
        /// <returns>An OK result if the member successfully joined the group.</returns>
        /// <response code="200">The member successfully joined the meeting group.</response>
        [HttpPost("{meetingGroupId}/members")]
        [HasPermission(MeetingsPermissions.JoinToGroup)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> JoinToGroup(Guid meetingGroupId)
        {
            await _meetingsModule.ExecuteCommandAsync(new JoinToGroupCommand(meetingGroupId));

            return Ok();
        }

        /// <summary>
        /// Removes the authenticated member from a meeting group.
        /// </summary>
        /// <param name="meetingGroupId">The unique identifier of the meeting group to leave.</param>
        /// <returns>An OK result if the member successfully left the group.</returns>
        /// <response code="200">The member successfully left the meeting group.</response>
        [HttpDelete("{meetingGroupId}/members")]
        [HasPermission(MeetingsPermissions.LeaveMeetingGroup)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LeaveMeetingGroup(Guid meetingGroupId)
        {
            await _meetingsModule.ExecuteCommandAsync(new LeaveMeetingGroupCommand(meetingGroupId));

            return Ok();
        }
    }
}
