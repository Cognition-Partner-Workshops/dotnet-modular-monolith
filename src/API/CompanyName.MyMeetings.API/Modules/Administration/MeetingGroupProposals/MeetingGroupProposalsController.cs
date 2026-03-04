using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Administration.Application.Contracts;
using CompanyName.MyMeetings.Modules.Administration.Application.MeetingGroupProposals.AcceptMeetingGroupProposal;
using CompanyName.MyMeetings.Modules.Administration.Application.MeetingGroupProposals.GetMeetingGroupProposal;
using CompanyName.MyMeetings.Modules.Administration.Application.MeetingGroupProposals.GetMeetingGroupProposals;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Administration.MeetingGroupProposals
{
    /// <summary>
    /// Controller for managing meeting group proposals within the Administration module.
    /// Provides endpoints for administrators to review and accept meeting group proposals.
    /// </summary>
    [Route("api/administration/meetingGroupProposals")]
    [ApiController]
    public class MeetingGroupProposalsController : ControllerBase
    {
        private readonly IAdministrationModule _administrationModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingGroupProposalsController"/> class.
        /// </summary>
        /// <param name="administrationModule">The administration module used to execute commands and queries.</param>
        public MeetingGroupProposalsController(IAdministrationModule administrationModule)
        {
            _administrationModule = administrationModule;
        }

        /// <summary>
        /// Retrieves all meeting group proposals pending administrative review.
        /// </summary>
        /// <returns>A list of all meeting group proposals.</returns>
        /// <response code="200">Returns the list of meeting group proposals.</response>
        [HttpGet("")]
        [HasPermission(AdministrationPermissions.AcceptMeetingGroupProposal)]
        [ProducesResponseType(typeof(List<MeetingGroupProposalDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeetingGroupProposals()
        {
            var meetingGroupProposals =
                await _administrationModule.ExecuteQueryAsync(new GetMeetingGroupProposalsQuery());

            return Ok(meetingGroupProposals);
        }

        /// <summary>
        /// Accepts a meeting group proposal, allowing the proposed group to become active.
        /// </summary>
        /// <param name="meetingGroupProposalId">The unique identifier of the meeting group proposal to accept.</param>
        /// <returns>An OK result if the proposal was successfully accepted.</returns>
        /// <response code="200">The meeting group proposal was successfully accepted.</response>
        [HttpPatch("{meetingGroupProposalId}/accept")]
        [HasPermission(AdministrationPermissions.AcceptMeetingGroupProposal)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AcceptMeetingGroupProposal(Guid meetingGroupProposalId)
        {
            await _administrationModule.ExecuteCommandAsync(
                new AcceptMeetingGroupProposalCommand(meetingGroupProposalId));

            return Ok();
        }
    }
}
