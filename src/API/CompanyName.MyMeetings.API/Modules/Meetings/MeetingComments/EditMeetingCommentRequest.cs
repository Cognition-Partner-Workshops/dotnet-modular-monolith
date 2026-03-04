namespace CompanyName.MyMeetings.API.Modules.Meetings.MeetingComments
{
    /// <summary>
    /// Request model for editing an existing meeting comment.
    /// </summary>
    public class EditMeetingCommentRequest
    {
        /// <summary>
        /// The updated text content of the comment.
        /// </summary>
        public string EditedComment { get; set; }
    }
}
