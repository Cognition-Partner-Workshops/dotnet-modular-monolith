using CompanyName.MyMeetings.API.Configuration.Authorization;
using CompanyName.MyMeetings.Modules.Meetings.Application.Contracts;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingComments.AddMeetingComment;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingComments.AddMeetingCommentLike;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingComments.AddMeetingCommentReply;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingComments.EditMeetingComment;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingComments.RemoveMeetingComment;
using CompanyName.MyMeetings.Modules.Meetings.Application.MeetingComments.RemoveMeetingCommentLike;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.API.Modules.Meetings.MeetingComments
{
    /// <summary>
    /// Controller for managing comments on meetings.
    /// Provides endpoints to add, edit, delete, reply to, like, and unlike meeting comments.
    /// </summary>
    [Route("api/meetings/[controller]")]
    [ApiController]
    public class MeetingCommentsController : ControllerBase
    {
        private readonly IMeetingsModule _meetingModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingCommentsController"/> class.
        /// </summary>
        /// <param name="meetingModule">The meetings module used to execute commands.</param>
        public MeetingCommentsController(IMeetingsModule meetingModule)
        {
            _meetingModule = meetingModule;
        }

        /// <summary>
        /// Adds a new comment to a meeting.
        /// </summary>
        /// <param name="request">The request containing the meeting identifier and comment text.</param>
        /// <returns>The unique identifier of the newly created comment.</returns>
        /// <response code="200">Returns the identifier of the created comment.</response>
        [HttpPost]
        [HasPermission(MeetingsPermissions.AddMeetingComment)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddComment([FromBody] AddMeetingCommentRequest request)
        {
            var commentId =
                await _meetingModule.ExecuteCommandAsync(new AddMeetingCommentCommand(
                    request.MeetingId,
                    request.Comment));

            return Ok(commentId);
        }

        /// <summary>
        /// Edits an existing meeting comment.
        /// </summary>
        /// <param name="meetingCommentId">The unique identifier of the comment to edit.</param>
        /// <param name="request">The request containing the updated comment text.</param>
        /// <returns>An OK result if the comment was successfully edited.</returns>
        /// <response code="200">The comment was successfully edited.</response>
        [HttpPut("{meetingCommentId}")]
        [HasPermission(MeetingsPermissions.EditMeetingComment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditComment(
            [FromRoute] Guid meetingCommentId,
            [FromBody] EditMeetingCommentRequest request)
        {
            await _meetingModule.ExecuteCommandAsync(new EditMeetingCommentCommand(
                meetingCommentId,
                request.EditedComment));

            return Ok();
        }

        /// <summary>
        /// Deletes a meeting comment.
        /// </summary>
        /// <param name="meetingCommentId">The unique identifier of the comment to delete.</param>
        /// <param name="reason">The reason for removing the comment.</param>
        /// <returns>An OK result if the comment was successfully deleted.</returns>
        /// <response code="200">The comment was successfully deleted.</response>
        [HttpDelete("{meetingCommentId}")]
        [HasPermission(MeetingsPermissions.RemoveMeetingComment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid meetingCommentId, [FromQuery] string reason)
        {
            await _meetingModule.ExecuteCommandAsync(
                new RemoveMeetingCommentCommand(meetingCommentId, reason));

            return Ok();
        }

        /// <summary>
        /// Adds a reply to an existing meeting comment.
        /// </summary>
        /// <param name="meetingCommentId">The unique identifier of the comment to reply to.</param>
        /// <param name="reply">The reply text.</param>
        /// <returns>An OK result if the reply was successfully added.</returns>
        /// <response code="200">The reply was successfully added to the comment.</response>
        [HttpPost("{meetingCommentId}/replies")]
        [HasPermission(MeetingsPermissions.AddMeetingCommentReply)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddReply([FromRoute] Guid meetingCommentId, [FromBody] string reply)
        {
            await _meetingModule.ExecuteCommandAsync(new AddReplyToMeetingCommentCommand(meetingCommentId, reply));

            return Ok();
        }

        /// <summary>
        /// Adds a like to a meeting comment by the authenticated user.
        /// </summary>
        /// <param name="meetingCommentId">The unique identifier of the comment to like.</param>
        /// <returns>An OK result if the like was successfully added.</returns>
        /// <response code="200">The comment was successfully liked.</response>
        [HttpPost("{meetingCommentId}/likes")]
        [HasPermission(MeetingsPermissions.LikeMeetingComment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LikeComment([FromRoute] Guid meetingCommentId)
        {
            await _meetingModule.ExecuteCommandAsync(
                new AddMeetingCommentLikeCommand(meetingCommentId));

            return Ok();
        }

        /// <summary>
        /// Removes the authenticated user's like from a meeting comment.
        /// </summary>
        /// <param name="meetingCommentId">The unique identifier of the comment to unlike.</param>
        /// <returns>An OK result if the like was successfully removed.</returns>
        /// <response code="200">The comment like was successfully removed.</response>
        [HttpDelete("{meetingCommentId}/likes")]
        [HasPermission(MeetingsPermissions.UnlikeMeetingComment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UnlikeComment([FromRoute] Guid meetingCommentId)
        {
            await _meetingModule.ExecuteCommandAsync(
                new RemoveMeetingCommentLikeCommand(meetingCommentId));

            return Ok();
        }
    }
}
