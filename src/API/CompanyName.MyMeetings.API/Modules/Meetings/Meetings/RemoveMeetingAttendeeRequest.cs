namespace CompanyName.MyMeetings.API.Modules.Meetings.Meetings
{
    /// <summary>
    /// Request model for removing a meeting attendee.
    /// </summary>
    public class RemoveMeetingAttendeeRequest
    {
        /// <summary>
        /// The reason for removing the attendee from the meeting.
        /// </summary>
        public string RemovingReason { get; set; }
    }
}
