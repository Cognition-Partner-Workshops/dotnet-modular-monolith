namespace CompanyName.MyMeetings.API.Modules.Meetings.MeetingGroups
{
    /// <summary>
    /// Request model for creating a new meeting group.
    /// </summary>
    public class CreateNewMeetingGroupRequest
    {
        /// <summary>
        /// The name of the meeting group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the meeting group's purpose and activities.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The city where the meeting group is located.
        /// </summary>
        public string LocationCity { get; set; }

        /// <summary>
        /// The country where the meeting group is located.
        /// </summary>
        public string LocationCountry { get; set; }
    }
}
