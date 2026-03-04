using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Meetings.Application.Contracts;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingGroupProposals.GetAllMeetingGroupProposals;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingGroupProposals.GetMeetingGroupProposal;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingGroupProposals.GetMemberMeetingGroupProposals;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingGroupProposals.ProposeMeetingGroup;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Meetings.MeetingGroupProposals
{
    /// <summary>
    /// Controller for managing meeting group proposals within the Meetings module.
    /// Provides endpoints for members to propose new meeting groups and view existing proposals.
    /// </summary>
    [Route("api/meetings/[controller]")]
    [ApiController]
    public class MeetingGroupProposalsController : ControllerBase
    {
        private readonly IMeetingsModule _meetingsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingGroupProposalsController"/> class.
        /// </summary>
        /// <param name="meetingsModule">The meetings module used to execute commands and queries.</param>
        public MeetingGroupProposalsController(IMeetingsModule meetingsModule)
        {
            _meetingsModule = meetingsModule;
        }

        /// <summary>
        /// Retrieves meeting group proposals submitted by the authenticated member.
        /// </summary>
        /// <returns>A list of the authenticated member's meeting group proposals.</returns>
        /// <response code="200">Returns the list of meeting group proposals for the authenticated member.</response>
        [HttpGet("")]
        [HasPermission(MeetingsPermissions.GetMeetingGroupProposals)]
        [ProducesResponseType(typeof(List<MeetingGroupProposalDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMemberMeetingGroupProposals()
        {
            var meetingGroupProposals = await _meetingsModule.ExecuteQueryAsync(
                new GetMemberMeetingGroupProposalsQuery());

            return Ok(meetingGroupProposals);
        }

        /// <summary>
        /// Retrieves all meeting group proposals with optional pagination.
        /// </summary>
        /// <param name="page">Optional page number for pagination.</param>
        /// <param name="perPage">Optional number of items per page.</param>
        /// <returns>A paginated list of all meeting group proposals.</returns>
        /// <response code="200">Returns the paginated list of all meeting group proposals.</response>
        [HttpGet("all")]
        [HasPermission(MeetingsPermissions.GetMeetingGroupProposals)]
        [ProducesResponseType(typeof(List<MeetingGroupProposalDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMeetingGroupProposals(int? page, int? perPage)
        {
            var meetingGroupProposals = await _meetingsModule.ExecuteQueryAsync(
                new GetAllMeetingGroupProposalsQuery(page, perPage));

            return Ok(meetingGroupProposals);
        }

        /// <summary>
        /// Proposes a new meeting group for administrative approval.
        /// </summary>
        /// <param name="request">The request containing the proposed meeting group details.</param>
        /// <returns>An OK result if the proposal was successfully submitted.</returns>
        /// <response code="200">The meeting group proposal was successfully submitted.</response>
        [HttpPost("")]
        [HasPermission(MeetingsPermissions.ProposeMeetingGroup)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ProposeMeetingGroup(ProposeMeetingGroupRequest request)
        {
            await _meetingsModule.ExecuteCommandAsync(
                new ProposeMeetingGroupCommand(
                    request.Name,
                    request.Description,
                    request.LocationCity,
                    request.LocationCountryCode));

            return Ok();
        }
    }
}
