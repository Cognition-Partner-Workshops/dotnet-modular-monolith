using CompanyName.MyMeetings.Modules.Administration.Application.Contracts;
using CompanyName.MyMeetings.Modules.Administration.Application.MeetingGroupProposals.AcceptMeetingGroupProposal;
using CompanyName.MyMeetings.Modules.Administration.Application.MeetingGroupProposals.GetMeetingGroupProposal;
using CompanyName.MyMeetings.Modules.Administration.Application.MeetingGroupProposals.GetMeetingGroupProposals;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.Services.Administration.API.Controllers
{
    [Route("api/administration/meetingGroupProposals")]
    [ApiController]
    public class MeetingGroupProposalsController : ControllerBase
    {
        private readonly IAdministrationModule _administrationModule;

        public MeetingGroupProposalsController(IAdministrationModule administrationModule)
        {
            _administrationModule = administrationModule;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<MeetingGroupProposalDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeetingGroupProposals()
        {
            var meetingGroupProposals =
                await _administrationModule.ExecuteQueryAsync(new GetMeetingGroupProposalsQuery());

            return Ok(meetingGroupProposals);
        }

        [HttpPatch("{meetingGroupProposalId}/accept")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AcceptMeetingGroupProposal(Guid meetingGroupProposalId)
        {
            await _administrationModule.ExecuteCommandAsync(
                new AcceptMeetingGroupProposalCommand(meetingGroupProposalId));

            return Ok();
        }
    }
}
