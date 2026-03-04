namespace CompanyName.MyMeetings.API.Modules.Meetings.Meetings
{
    /// <summary>
    /// Request model for adding a meeting attendee.
    /// </summary>
    public class AddMeetingAttendeeRequest
    {
        /// <summary>
        /// The number of guests the attendee will bring to the meeting.
        /// </summary>
        public int GuestsNumber { get; set; }
    }
}
