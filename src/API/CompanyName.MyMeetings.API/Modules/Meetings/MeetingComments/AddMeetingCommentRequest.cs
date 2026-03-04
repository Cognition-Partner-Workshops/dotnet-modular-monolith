namespace CompanyName.MyMeetings.API.Modules.Meetings.MeetingComments
{
    /// <summary>
    /// Request model for adding a new comment to a meeting.
    /// </summary>
    public class AddMeetingCommentRequest
    {
        /// <summary>
        /// The unique identifier of the meeting to comment on.
        /// </summary>
        public Guid MeetingId { get; set; }

        /// <summary>
        /// The text content of the comment.
        /// </summary>
        public string Comment { get; set; }
    }
}
