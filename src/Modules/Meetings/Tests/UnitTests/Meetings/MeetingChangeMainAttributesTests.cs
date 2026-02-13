using CompanyName.MyMeetings.Modules.Meetings.Domain.Meetings;
using CompanyName.MyMeetings.Modules.Meetings.Domain.Meetings.Events;
using CompanyName.MyMeetings.Modules.Meetings.Domain.Meetings.Rules;
using CompanyName.MyMeetings.Modules.Meetings.Domain.Members;
using FluentAssertions;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.Meetings.Domain.UnitTests.Meetings
{
    [TestFixture]
    public class MeetingChangeMainAttributesTests : MeetingTestsBase
    {
        [Test]
        public void ChangeMainAttributes_WhenMeetingHasNotStarted_IsSuccessful()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId
            });

            meetingTestData.Meeting.ChangeMainAttributes(
                "newTitle",
                MeetingTerm.CreateNewBetweenDates(DateTime.UtcNow.AddDays(3), DateTime.UtcNow.AddDays(4)),
                "newDescription",
                MeetingLocation.CreateNew("NewName", "NewAddress", "NewPostalCode", "NewCity"),
                MeetingLimits.Create(null, 0),
                Term.NoTerm,
                MoneyValue.Undefined,
                creatorId);

            var meetingChanged = AssertPublishedDomainEvent<MeetingMainAttributesChangedDomainEvent>(meetingTestData.Meeting);
            meetingChanged.MeetingId.Should().Be(meetingTestData.Meeting.Id);
        }

        [Test]
        public void ChangeMainAttributes_WithEventFee_IsSuccessful()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId
            });

            meetingTestData.Meeting.ChangeMainAttributes(
                "feeTitle",
                MeetingTerm.CreateNewBetweenDates(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)),
                "feeDescription",
                MeetingLocation.CreateNew("Name", "Addr", "Code", "City"),
                MeetingLimits.Create(20, 5),
                Term.NoTerm,
                MoneyValue.Of(50, "USD"),
                creatorId);

            var meetingChanged = AssertPublishedDomainEvent<MeetingMainAttributesChangedDomainEvent>(meetingTestData.Meeting);
            meetingChanged.MeetingId.Should().Be(meetingTestData.Meeting.Id);
        }

        [Test]
        public void ChangeMainAttributes_WithSufficientAttendeesLimit_IsSuccessful()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var attendeeId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId,
                Attendees = new[] { attendeeId }
            });

            meetingTestData.Meeting.ChangeMainAttributes(
                "newTitle",
                MeetingTerm.CreateNewBetweenDates(DateTime.UtcNow.AddDays(3), DateTime.UtcNow.AddDays(4)),
                "newDescription",
                MeetingLocation.CreateNew("NewName", "NewAddress", "NewPostalCode", "NewCity"),
                MeetingLimits.Create(10, 5),
                Term.NoTerm,
                MoneyValue.Undefined,
                creatorId);

            var meetingChanged = AssertPublishedDomainEvent<MeetingMainAttributesChangedDomainEvent>(meetingTestData.Meeting);
            meetingChanged.MeetingId.Should().Be(meetingTestData.Meeting.Id);
        }

        [Test]
        public void ChangeMainAttributes_WithNoAttendeesLimit_IsSuccessful()
        {
            var creatorId = new MemberId(Guid.NewGuid());
            var meetingTestData = CreateMeetingTestData(new MeetingTestDataOptions
            {
                CreatorId = creatorId
            });

            meetingTestData.Meeting.ChangeMainAttributes(
                "updatedTitle",
                MeetingTerm.CreateNewBetweenDates(DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(6)),
                "updatedDescription",
                MeetingLocation.CreateNew("Updated", "Addr", "Code", "City"),
                MeetingLimits.Create(null, 10),
                Term.NoTerm,
                MoneyValue.Of(25, "PLN"),
                creatorId);

            var meetingChanged = AssertPublishedDomainEvent<MeetingMainAttributesChangedDomainEvent>(meetingTestData.Meeting);
            meetingChanged.MeetingId.Should().Be(meetingTestData.Meeting.Id);
        }
    }
}
