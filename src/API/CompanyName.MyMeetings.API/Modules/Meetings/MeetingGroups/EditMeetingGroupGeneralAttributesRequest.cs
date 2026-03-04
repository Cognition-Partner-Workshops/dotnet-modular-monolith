namespace CompanyName.MyMeetings.API.Modules.Meetings.MeetingGroups
{
    /// <summary>
    /// Request model for editing the general attributes of a meeting group.
    /// </summary>
    public class EditMeetingGroupGeneralAttributesRequest
    {
        /// <summary>
        /// The updated name of the meeting group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The updated description of the meeting group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The updated city where the meeting group is located.
        /// </summary>
        public string LocationCity { get; set; }

        /// <summary>
        /// The updated country where the meeting group is located.
        /// </summary>
        public string LocationCountry { get; set; }
    }
}
