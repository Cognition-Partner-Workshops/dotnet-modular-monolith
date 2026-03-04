using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Meetings.Application.Contracts;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.AddMeetingAttendee;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.AddMeetingNotAttendee;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.CancelMeeting;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.ChangeMeetingMainAttributes;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.ChangeNotAttendeeDecision;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.CreateMeeting;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.GetAuthenticatedMemberMeetings;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.GetMeetingAttendees;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.GetMeetingDetails;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.RemoveMeetingAttendee;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.SetMeetingAttendeeRole;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.SetMeetingHostRole;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.SignOffMemberFromWaitlist;
using CompanyName.MyMeetings.Modules.Meetings.Application.Meetings.SignUpMemberToWaitlist;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Meetings.Meetings
{
    /// <summary>
    /// Controller for managing meetings.
    /// Provides endpoints for creating, editing, canceling meetings and managing attendees, waitlists, and roles.
    /// </summary>
    [Route("api/meetings/meetings")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingsModule _meetingsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingsController"/> class.
        /// </summary>
        /// <param name="meetingsModule">The meetings module used to execute commands and queries.</param>
        public MeetingsController(IMeetingsModule meetingsModule)
        {
            _meetingsModule = meetingsModule;
        }

        /// <summary>
        /// Retrieves the meetings associated with the authenticated member.
        /// </summary>
        /// <returns>A list of the authenticated member's meetings.</returns>
        /// <response code="200">Returns the list of meetings for the authenticated member.</response>
        [HttpGet("")]
        [HasPermission(MeetingsPermissions.GetAuthenticatedMemberMeetings)]
        [ProducesResponseType(typeof(List<MemberMeetingDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedMemberMeetings()
        {
            var meetings = await _meetingsModule.ExecuteQueryAsync(new GetAuthenticatedMemberMeetingsQuery());

            return Ok(meetings);
        }

        /// <summary>
        /// Retrieves the details of a specific meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <returns>The detailed information about the meeting.</returns>
        /// <response code="200">Returns the meeting details.</response>
        [HttpGet("{meetingId}")]
        [HasPermission(MeetingsPermissions.GetMeetingDetails)]
        [ProducesResponseType(typeof(MeetingDetailsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeetingDetails(Guid meetingId)
        {
            var meetingDetails = await _meetingsModule.ExecuteQueryAsync(new GetMeetingDetailsQuery(meetingId));

            return Ok(meetingDetails);
        }

        /// <summary>
        /// Creates a new meeting within a meeting group.
        /// </summary>
        /// <param name="request">The request containing the meeting details including schedule, location, attendee limits, and fee information.</param>
        /// <returns>An OK result if the meeting was successfully created.</returns>
        /// <response code="200">The meeting was successfully created.</response>
        [HttpPost("")]
        [HasPermission(MeetingsPermissions.CreateNewMeeting)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewMeeting([FromBody] CreateMeetingRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(new CreateMeetingCommand(
                request.MeetingGroupId,
                request.Title,
                request.TermStartDate,
                request.TermEndDate,
                request.Description,
                request.MeetingLocationName,
                request.MeetingLocationAddress,
                request.MeetingLocationPostalCode,
                request.MeetingLocationCity,
                request.AttendeesLimit,
                request.GuestsLimit,
                request.RSVPTermStartDate,
                request.RSVPTermEndDate,
                request.EventFeeValue,
                request.EventFeeCurrency,
                request.HostMemberIds));

            return Ok();
        }

        /// <summary>
        /// Updates the main attributes of an existing meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting to edit.</param>
        /// <param name="mainAttributesRequest">The request containing the updated meeting attributes.</param>
        /// <returns>An OK result if the meeting was successfully updated.</returns>
        /// <response code="200">The meeting attributes were successfully updated.</response>
        [HttpPut("{meetingId}")]
        [HasPermission(MeetingsPermissions.EditMeeting)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditMeeting(
            [FromRoute] Guid meetingId,
            [FromBody] ChangeMeetingMainAttributesRequest mainAttributesRequest)
        {
            await _meetingsModule.ExecuteCommandAsync(new ChangeMeetingMainAttributesCommand(
                meetingId,
                mainAttributesRequest.Title,
                mainAttributesRequest.TermStartDate,
                mainAttributesRequest.TermEndDate,
                mainAttributesRequest.Description,
                mainAttributesRequest.MeetingLocationName,
                mainAttributesRequest.MeetingLocationAddress,
                mainAttributesRequest.MeetingLocationPostalCode,
                mainAttributesRequest.MeetingLocationCity,
                mainAttributesRequest.AttendeesLimit,
                mainAttributesRequest.GuestsLimit,
                mainAttributesRequest.RSVPTermStartDate,
                mainAttributesRequest.RSVPTermEndDate,
                mainAttributesRequest.EventFeeValue,
                mainAttributesRequest.EventFeeCurrency));

            return Ok();
        }

        /// <summary>
        /// Retrieves the list of attendees for a specific meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <returns>A list of meeting attendees.</returns>
        /// <response code="200">Returns the list of attendees for the meeting.</response>
        [HttpGet("{meetingId}/attendees")]
        [HasPermission(MeetingsPermissions.GetMeetingAttendees)]
        [ProducesResponseType(typeof(List<MeetingAttendeeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeetingAttendees(Guid meetingId)
        {
            var meetingAttendees = await _meetingsModule.ExecuteQueryAsync(new GetMeetingAttendeesQuery(meetingId));

            return Ok(meetingAttendees);
        }

        /// <summary>
        /// Adds the authenticated member as an attendee to a meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <param name="attendeeRequest">The request containing the number of guests.</param>
        /// <returns>An OK result if the attendee was successfully added.</returns>
        /// <response code="200">The attendee was successfully added to the meeting.</response>
        [HttpPost("{meetingId}/attendees")]
        [HasPermission(MeetingsPermissions.AddMeetingAttendee)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddMeetingAttendee(
            [FromRoute] Guid meetingId,
            [FromBody] AddMeetingAttendeeRequest attendeeRequest)
        {
            await _meetingsModule.ExecuteCommandAsync(new AddMeetingAttendeeCommand(
                meetingId,
                attendeeRequest.GuestsNumber));

            return Ok();
        }

        /// <summary>
        /// Removes an attendee from a meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <param name="attendeeId">The unique identifier of the attendee to remove.</param>
        /// <param name="request">The request containing the reason for removal.</param>
        /// <returns>An OK result if the attendee was successfully removed.</returns>
        /// <response code="200">The attendee was successfully removed from the meeting.</response>
        [HttpDelete("{meetingId}/attendees/{attendeeId}")]
        [HasPermission(MeetingsPermissions.RemoveMeetingAttendee)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveMeetingAttendee(
            Guid meetingId,
            Guid attendeeId,
            RemoveMeetingAttendeeRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(
                new RemoveMeetingAttendeeCommand(meetingId, attendeeId, request.RemovingReason));

            return Ok();
        }

        /// <summary>
        /// Marks the authenticated member as not attending a meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <returns>An OK result if the member was successfully marked as not attending.</returns>
        /// <response code="200">The member was successfully marked as not attending.</response>
        [HttpPost("{meetingId}/notAttendees")]
        [HasPermission(MeetingsPermissions.AddNotAttendee)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddNotAttendee(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new AddMeetingNotAttendeeCommand(meetingId));

            return Ok();
        }

        /// <summary>
        /// Reverses the authenticated member's decision to not attend a meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <returns>An OK result if the not-attendee decision was successfully reversed.</returns>
        /// <response code="200">The not-attendee decision was successfully reversed.</response>
        [HttpDelete("{meetingId}/notAttendees")]
        [HasPermission(MeetingsPermissions.ChangeNotAttendeeDecision)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeNotAttendeeDecision(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new ChangeNotAttendeeDecisionCommand(meetingId));

            return Ok();
        }

        /// <summary>
        /// Signs up the authenticated member to the meeting waitlist.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <returns>An OK result if the member was successfully added to the waitlist.</returns>
        /// <response code="200">The member was successfully added to the meeting waitlist.</response>
        [HttpPost("{meetingId}/waitlistMembers")]
        [HasPermission(MeetingsPermissions.SignUpMemberToWaitlist)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SignUpMemberToWaitlist(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new SignUpMemberToWaitlistCommand(meetingId));

            return Ok();
        }

        /// <summary>
        /// Removes the authenticated member from the meeting waitlist.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <returns>An OK result if the member was successfully removed from the waitlist.</returns>
        /// <response code="200">The member was successfully removed from the meeting waitlist.</response>
        [HttpDelete("{meetingId}/waitlistMembers")]
        [HasPermission(MeetingsPermissions.SignOffMemberFromWaitlist)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SignOffMemberFromWaitlist(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new SignOffMemberFromWaitlistCommand(meetingId));

            return Ok();
        }

        /// <summary>
        /// Promotes an attendee to the host role for a meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <param name="request">The request containing the attendee identifier to promote.</param>
        /// <returns>An OK result if the host role was successfully assigned.</returns>
        /// <response code="200">The attendee was successfully promoted to host.</response>
        [HttpPost("{meetingId}/hosts")]
        [HasPermission(MeetingsPermissions.SetMeetingHostRole)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SetMeetingHostRole(Guid meetingId, SetMeetingHostRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(new SetMeetingHostRoleCommand(request.AttendeeId, meetingId));

            return Ok();
        }

        /// <summary>
        /// Demotes a host back to the attendee role for a meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting.</param>
        /// <param name="request">The request containing the attendee identifier to demote.</param>
        /// <returns>An OK result if the attendee role was successfully assigned.</returns>
        /// <response code="200">The host was successfully demoted to attendee.</response>
        [HttpPost("{meetingId}/attendees/attendeeRole")]
        [HasPermission(MeetingsPermissions.SetMeetingAttendeeRole)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SetMeetingAttendeeRole(Guid meetingId, SetMeetingHostRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(new SetMeetingAttendeeRoleCommand(request.AttendeeId, meetingId));

            return Ok();
        }

        /// <summary>
        /// Cancels a scheduled meeting.
        /// </summary>
        /// <param name="meetingId">The unique identifier of the meeting to cancel.</param>
        /// <returns>An OK result if the meeting was successfully canceled.</returns>
        /// <response code="200">The meeting was successfully canceled.</response>
        [HttpPatch("{meetingId}/cancel")]
        [HasPermission(MeetingsPermissions.CancelMeeting)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelMeeting(Guid meetingId)
        {
            await _meetingsModule.ExecuteCommandAsync(new CancelMeetingCommand(meetingId));

            return Ok();
        }
    }
}
