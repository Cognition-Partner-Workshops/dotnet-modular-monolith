namespace CompanyName.MyMeetings.API.Modules.Meetings.MeetingGroupProposals
{
    /// <summary>
    /// Request model for proposing a new meeting group.
    /// </summary>
    public class ProposeMeetingGroupRequest
    {
        /// <summary>
        /// The name of the proposed meeting group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the proposed meeting group's purpose and activities.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The city where the meeting group is located.
        /// </summary>
        public string LocationCity { get; set; }

        /// <summary>
        /// The ISO country code of the meeting group's location.
        /// </summary>
        public string LocationCountryCode { get; set; }
    }
}
