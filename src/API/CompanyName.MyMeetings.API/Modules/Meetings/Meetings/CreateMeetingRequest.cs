namespace CompanyName.MyMeetings.API.Modules.Meetings.Meetings
{
    /// <summary>
    /// Request model for creating a new meeting.
    /// </summary>
    public class CreateMeetingRequest
    {
        /// <summary>
        /// The unique identifier of the meeting group under which the meeting is created.
        /// </summary>
        public Guid MeetingGroupId { get; set; }

        /// <summary>
        /// The title of the meeting.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The start date and time of the meeting.
        /// </summary>
        public DateTime TermStartDate { get; set; }

        /// <summary>
        /// The end date and time of the meeting.
        /// </summary>
        public DateTime TermEndDate { get; set; }

        /// <summary>
        /// A description of the meeting.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of the meeting location (e.g., venue name).
        /// </summary>
        public string MeetingLocationName { get; set; }

        /// <summary>
        /// The street address of the meeting location.
        /// </summary>
        public string MeetingLocationAddress { get; set; }

        /// <summary>
        /// The postal code of the meeting location.
        /// </summary>
        public string MeetingLocationPostalCode { get; set; }

        /// <summary>
        /// The city of the meeting location.
        /// </summary>
        public string MeetingLocationCity { get; set; }

        /// <summary>
        /// The maximum number of attendees allowed. Null indicates no limit.
        /// </summary>
        public int? AttendeesLimit { get; set; }

        /// <summary>
        /// The maximum number of guests each attendee may bring.
        /// </summary>
        public int GuestsLimit { get; set; }

        /// <summary>
        /// The start date and time of the RSVP period. Null if no RSVP window is set.
        /// </summary>
        public DateTime? RSVPTermStartDate { get; set; }

        /// <summary>
        /// The end date and time of the RSVP period. Null if no RSVP window is set.
        /// </summary>
        public DateTime? RSVPTermEndDate { get; set; }

        /// <summary>
        /// The monetary value of the event fee. Null if the event is free.
        /// </summary>
        public decimal? EventFeeValue { get; set; }

        /// <summary>
        /// The ISO 4217 currency code for the event fee (e.g., "USD", "EUR").
        /// </summary>
        public string EventFeeCurrency { get; set; }

        /// <summary>
        /// A list of member identifiers to assign as hosts for the meeting.
        /// </summary>
        public List<Guid> HostMemberIds { get; set; }
    }
}
