namespace CompanyName.MyMeetings.API.Modules.Meetings.Meetings
{
    /// <summary>
    /// Request model for setting a meeting attendee role.
    /// </summary>
    public class SetMeetingAttendeeRequest
    {
        /// <summary>
        /// The unique identifier of the attendee whose role is being set.
        /// </summary>
        public Guid AttendeeId { get; set; }
    }
}
