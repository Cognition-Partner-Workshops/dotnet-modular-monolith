namespace CompanyName.MyMeetings.API.Modules.Meetings.Meetings
{
    /// <summary>
    /// Request model for changing the main attributes of an existing meeting.
    /// </summary>
    public class ChangeMeetingMainAttributesRequest
    {
        /// <summary>
        /// The unique identifier of the meeting to update.
        /// </summary>
        public Guid MeetingId { get; set; }

        /// <summary>
        /// The updated title of the meeting.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The updated start date and time of the meeting.
        /// </summary>
        public DateTime TermStartDate { get; set; }

        /// <summary>
        /// The updated end date and time of the meeting.
        /// </summary>
        public DateTime TermEndDate { get; set; }

        /// <summary>
        /// The updated description of the meeting.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The updated name of the meeting location.
        /// </summary>
        public string MeetingLocationName { get; set; }

        /// <summary>
        /// The updated street address of the meeting location.
        /// </summary>
        public string MeetingLocationAddress { get; set; }

        /// <summary>
        /// The updated postal code of the meeting location.
        /// </summary>
        public string MeetingLocationPostalCode { get; set; }

        /// <summary>
        /// The updated city of the meeting location.
        /// </summary>
        public string MeetingLocationCity { get; set; }

        /// <summary>
        /// The updated maximum number of attendees. Null indicates no limit.
        /// </summary>
        public int? AttendeesLimit { get; set; }

        /// <summary>
        /// The updated maximum number of guests each attendee may bring.
        /// </summary>
        public int GuestsLimit { get; set; }

        /// <summary>
        /// The updated RSVP period start date. Null if no RSVP window is set.
        /// </summary>
        public DateTime? RSVPTermStartDate { get; set; }

        /// <summary>
        /// The updated RSVP period end date. Null if no RSVP window is set.
        /// </summary>
        public DateTime? RSVPTermEndDate { get; set; }

        /// <summary>
        /// The updated event fee value. Null if the event is free.
        /// </summary>
        public decimal? EventFeeValue { get; set; }

        /// <summary>
        /// The updated ISO 4217 currency code for the event fee.
        /// </summary>
        public string EventFeeCurrency { get; set; }

        /// <summary>
        /// The updated list of member identifiers assigned as hosts.
        /// </summary>
        public List<Guid> HostMemberIds { get; set; }
    }
}
