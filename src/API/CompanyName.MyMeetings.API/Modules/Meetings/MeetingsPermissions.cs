namespace CompanyName.MyMeetings.API.Modules.Meetings
{
    /// <summary>
    /// Defines permission constants used by the Meetings module to authorize API actions.
    /// </summary>
    public class MeetingsPermissions
    {
        /// <summary>Permission required to view meeting group proposals.</summary>
        public const string GetMeetingGroupProposals = "GetMeetingGroupProposals";

        /// <summary>Permission required to propose a new meeting group.</summary>
        public const string ProposeMeetingGroup = "ProposeMeetingGroup";

        /// <summary>Permission required to create a new meeting.</summary>
        public const string CreateNewMeeting = "CreateNewMeeting";

        /// <summary>Permission required to edit an existing meeting.</summary>
        public const string EditMeeting = "EditMeeting";

        /// <summary>Permission required to add an attendee to a meeting.</summary>
        public const string AddMeetingAttendee = "AddMeetingAttendee";

        /// <summary>Permission required to remove an attendee from a meeting.</summary>
        public const string RemoveMeetingAttendee = "RemoveMeetingAttendee";

        /// <summary>Permission required to mark a member as not attending a meeting.</summary>
        public const string AddNotAttendee = "AddNotAttendee";

        /// <summary>Permission required to reverse a not-attending decision.</summary>
        public const string ChangeNotAttendeeDecision = "ChangeNotAttendeeDecision";

        /// <summary>Permission required to sign up a member to a meeting waitlist.</summary>
        public const string SignUpMemberToWaitlist = "SignUpMemberToWaitlist";

        /// <summary>Permission required to remove a member from a meeting waitlist.</summary>
        public const string SignOffMemberFromWaitlist = "SignOffMemberFromWaitlist";

        /// <summary>Permission required to promote an attendee to host.</summary>
        public const string SetMeetingHostRole = "SetMeetingHostRole";

        /// <summary>Permission required to demote a host to attendee.</summary>
        public const string SetMeetingAttendeeRole = "SetMeetingAttendeeRole";

        /// <summary>Permission required to cancel a meeting.</summary>
        public const string CancelMeeting = "CancelMeeting";

        /// <summary>Permission required to view all meeting groups.</summary>
        public const string GetAllMeetingGroups = "GetAllMeetingGroups";

        /// <summary>Permission required to edit meeting group general attributes.</summary>
        public const string EditMeetingGroupGeneralAttributes = "EditMeetingGroupGeneralAttributes";

        /// <summary>Permission required to join a meeting group.</summary>
        public const string JoinToGroup = "JoinToGroup";

        /// <summary>Permission required to leave a meeting group.</summary>
        public const string LeaveMeetingGroup = "LeaveMeetingGroup";

        /// <summary>Permission required to add a comment to a meeting.</summary>
        public const string AddMeetingComment = "AddMeetingComment";

        /// <summary>Permission required to edit a meeting comment.</summary>
        public const string EditMeetingComment = "EditMeetingComment";

        /// <summary>Permission required to remove a meeting comment.</summary>
        public const string RemoveMeetingComment = "RemoveMeetingComment";

        /// <summary>Permission required to reply to a meeting comment.</summary>
        public const string AddMeetingCommentReply = "AddMeetingCommentReply";

        /// <summary>Permission required to like a meeting comment.</summary>
        public const string LikeMeetingComment = "LikeMeetingComment";

        /// <summary>Permission required to unlike a meeting comment.</summary>
        public const string UnlikeMeetingComment = "UnlikeMeetingComment";

        /// <summary>Permission required to enable commenting on a meeting.</summary>
        public const string EnableMeetingCommenting = "EnableMeetingCommenting";

        /// <summary>Permission required to disable commenting on a meeting.</summary>
        public const string DisableMeetingCommenting = "DisableMeetingCommenting";

        /// <summary>Permission required to view the authenticated member's meeting groups.</summary>
        public const string GetAuthenticatedMemberMeetingGroups = "GetAuthenticatedMemberMeetingGroups";

        /// <summary>Permission required to view meeting group details.</summary>
        public const string GetMeetingGroupDetails = "GetMeetingGroupDetails";

        /// <summary>Permission required to view meeting details.</summary>
        public const string GetMeetingDetails = "GetMeetingDetails";

        /// <summary>Permission required to view the authenticated member's meetings.</summary>
        public const string GetAuthenticatedMemberMeetings = "GetAuthenticatedMemberMeetings";

        /// <summary>Permission required to view meeting attendees.</summary>
        public const string GetMeetingAttendees = "GetMeetingAttendees";
    }
}
