using CompanyName.MyMeetings.Modules.Administration.Domain.MeetingGroupProposals;
using CompanyName.MyMeetings.Modules.Administration.Domain.MeetingGroupProposals.Rules;
using CompanyName.MyMeetings.Modules.Administration.Domain.UnitTests.SeedWork;
using CompanyName.MyMeetings.Modules.Administration.Domain.Users;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.Administration.Domain.UnitTests.MeetingGroupProposals
{
    [TestFixture]
    public class MeetingGroupProposalRejectTests : TestBase
    {
        [Test]
        public void RejectProposal_WhenDecisionIsNotMade_IsSuccessful()
        {
            var meetingGroupProposalId = Guid.NewGuid();
            var location = MeetingGroupLocation.Create("Warsaw", "Poland");
            var proposalUserId = new UserId(Guid.NewGuid());
            var proposalDate = DateTime.Now;
            var meetingGroupProposal = MeetingGroupProposal.CreateToVerify(
                meetingGroupProposalId,
                "meetingName",
                "meetingDescription",
                location,
                proposalUserId,
                proposalDate);

            var rejectingUserId = new UserId(Guid.NewGuid());
            meetingGroupProposal.Reject(rejectingUserId, "Not suitable for our platform");

            AssertBrokenRule<MeetingGroupProposalCanBeVerifiedOnceRule>(() =>
            {
                meetingGroupProposal.Accept(rejectingUserId);
            });
        }

        [Test]
        public void AcceptProposal_WhenAlreadyRejected_CannotBeVerifiedAgain()
        {
            var meetingGroupProposalId = Guid.NewGuid();
            var location = MeetingGroupLocation.Create("London", "UK");
            var userId = new UserId(Guid.NewGuid());
            var proposalDate = DateTime.Now;
            var meetingGroupProposal = MeetingGroupProposal.CreateToVerify(
                meetingGroupProposalId,
                "meetingName",
                "meetingDescription",
                location,
                userId,
                proposalDate);

            meetingGroupProposal.Reject(userId, "Not suitable");

            AssertBrokenRule<MeetingGroupProposalCanBeVerifiedOnceRule>(() =>
            {
                meetingGroupProposal.Accept(userId);
            });
        }

        [Test]
        public void RejectProposal_WhenAlreadyRejected_CannotBeRejectedAgain()
        {
            var meetingGroupProposalId = Guid.NewGuid();
            var location = MeetingGroupLocation.Create("Berlin", "Germany");
            var userId = new UserId(Guid.NewGuid());
            var proposalDate = DateTime.Now;
            var meetingGroupProposal = MeetingGroupProposal.CreateToVerify(
                meetingGroupProposalId,
                "meetingName",
                "meetingDescription",
                location,
                userId,
                proposalDate);

            meetingGroupProposal.Reject(userId, "Not suitable");

            AssertBrokenRule<MeetingGroupProposalCanBeVerifiedOnceRule>(() =>
            {
                meetingGroupProposal.Reject(userId, "Another reason");
            });
        }

        [Test]
        public void RejectProposal_WithNullReason_CannotBeRejected()
        {
            var meetingGroupProposalId = Guid.NewGuid();
            var location = MeetingGroupLocation.Create("Paris", "France");
            var userId = new UserId(Guid.NewGuid());
            var proposalDate = DateTime.Now;
            var meetingGroupProposal = MeetingGroupProposal.CreateToVerify(
                meetingGroupProposalId,
                "meetingName",
                "meetingDescription",
                location,
                userId,
                proposalDate);

            AssertBrokenRule<MeetingGroupProposalRejectionMustHaveAReasonRule>(() =>
            {
                meetingGroupProposal.Reject(userId, null);
            });
        }
    }
}
