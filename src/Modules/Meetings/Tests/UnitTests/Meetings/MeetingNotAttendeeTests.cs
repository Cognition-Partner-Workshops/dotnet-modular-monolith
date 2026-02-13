using CompanyName.MyMeetings.Modules.Meetings.Domain.MeetingGroups.Events;
using CompanyName.MyMeetings.Modules.Meetings.Domain.Meetings;
using CompanyName.MyMeetings.Modules.Meetings.Domain.Meetings.Events;
using CompanyName.MyMeetings.Modules.Meetings.Domain.Meetings.Rules;
using CompanyName.MyMeetings.Modules.Meetings.Domain.Members;
using FluentAssertions;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.Meetings.Domain.UnitTests.Meetings
{
    [TestFixture]
    public class MeetingNotAttendeeTests : MeetingTestsBase
    {
        [Test]
        public void AddNotAttendee_WhenMeetingHasNotStarted_IsSuccessful()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var memberId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId
            });

            meetingTestData.Meeting.AddNotAttendee(memberId);

            var notAttendeeAdded = AssertPublishedDomainEvent<MeetingNotAttendeeAddedDomainEvent>(meetingTestData.Meeting);
            notAttendeeAdded.MemberId.Should().Be(memberId);
            notAttendeeAdded.MeetingId.Should().Be(meetingTestData.Meeting.Id);
        }

        [Test]
        public void AddNotAttendee_WhenMeetingHasStarted_IsNotPossible()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var memberId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId,
                MeetingTerm = MeetingTerm.CreateNewBetweenDates(DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1))
            });

            AssertBrokenRule<MeetingCannotBeChangedAfterStartRule>(() =>
            {
                meetingTestData.Meeting.AddNotAttendee(memberId);
            });
        }

        [Test]
        public void AddNotAttendee_WhenMemberIsAlreadyNotAttendee_IsNotPossible()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var memberId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId
            });

            meetingTestData.Meeting.AddNotAttendee(memberId);

            AssertBrokenRule<MemberCannotBeNotAttendeeTwiceRule>(() =>
            {
                meetingTestData.Meeting.AddNotAttendee(memberId);
            });
        }

        [Test]
        public void ChangeNotAttendeeDecision_WhenMeetingHasNotStarted_IsSuccessful()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var memberId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId
            });

            meetingTestData.Meeting.AddNotAttendee(memberId);

            meetingTestData.Meeting.ChangeNotAttendeeDecision(memberId);

            var notAttendeeChangedDecision = AssertPublishedDomainEvent<MeetingNotAttendeeChangedDecisionDomainEvent>(meetingTestData.Meeting);
            notAttendeeChangedDecision.MemberId.Should().Be(memberId);
        }

        [Test]
        public void ChangeNotAttendeeDecision_WhenMeetingHasStarted_IsNotPossible()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var memberId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId,
                MeetingTerm = MeetingTerm.CreateNewBetweenDates(DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1))
            });

            AssertBrokenRule<MeetingCannotBeChangedAfterStartRule>(() =>
            {
                meetingTestData.Meeting.ChangeNotAttendeeDecision(memberId);
            });
        }

        [Test]
        public void ChangeNotAttendeeDecision_WhenMemberIsNotActiveNotAttendee_IsNotPossible()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var memberId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId
            });

            AssertBrokenRule<NotActiveNotAttendeeCannotChangeDecisionRule>(() =>
            {
                meetingTestData.Meeting.ChangeNotAttendeeDecision(memberId);
            });
        }

        [Test]
        public void MarkAttendeeFeeAsPayed_WhenAttendeeIsActive_IsSuccessful()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var attendeeId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId,
                Attendees = new[] { attendeeId }
            });

            meetingTestData.Meeting.MarkAttendeeFeeAsPayed(attendeeId);

            var feePayedEvent = AssertPublishedDomainEvent<MeetingAttendeeFeePaidDomainEvent>(meetingTestData.Meeting);
            feePayedEvent.AttendeeId.Should().Be(attendeeId);
            feePayedEvent.MeetingId.Should().Be(meetingTestData.Meeting.Id);
        }

        [Test]
        public void Cancel_WhenAlreadyCanceled_DoesNotPublishDuplicateEvent()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId
            });

            meetingTestData.Meeting.Cancel(creatorId);
            meetingTestData.Meeting.Cancel(creatorId);

            var canceledEvents = AssertPublishedDomainEvents<MeetingCanceledDomainEvent>(meetingTestData.Meeting);
            canceledEvents.Should().HaveCount(1);
        }
    }
}
