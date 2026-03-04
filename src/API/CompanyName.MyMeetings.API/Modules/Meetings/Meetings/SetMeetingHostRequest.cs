namespace CompanyName.MyMeetings.API.Modules.Meetings.Meetings
{
    /// <summary>
    /// Request model for promoting an attendee to the host role.
    /// </summary>
    public class SetMeetingHostRequest
    {
        /// <summary>
        /// The unique identifier of the attendee to promote to host.
        /// </summary>
        public Guid AttendeeId { get; set; }
    }
}
