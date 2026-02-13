using CompanyName.MyMeetings.Modules.Payments.Domain.MeetingFees;
using CompanyName.MyMeetings.Modules.Payments.Domain.MeetingFees.Events;
using CompanyName.MyMeetings.Modules.Payments.Domain.Payers;
using CompanyName.MyMeetings.Modules.Payments.Domain.SeedWork;
using CompanyName.MyMeetings.Modules.Payments.Domain.UnitTests.SeedWork;
using FluentAssertions;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.Payments.Domain.UnitTests.MeetingFees
{
    [TestFixture]
    public class MeetingFeeTests : TestBase
    {
        [Test]
        public void CreateMeetingFee_IsSuccessful()
        {
            var payerId = new PayerId(Guid.NewGuid());
            var meetingId = new MeetingId(Guid.NewGuid());
            var fee = MoneyValue.Of(100, "PLN");

            var meetingFee = MeetingFee.Create(payerId, meetingId, fee);

            var meetingFeeCreated = AssertPublishedDomainEvent<MeetingFeeCreatedDomainEvent>(meetingFee);
            meetingFeeCreated.PayerId.Should().Be(payerId.Value);
            meetingFeeCreated.MeetingId.Should().Be(meetingId.Value);
            meetingFeeCreated.FeeValue.Should().Be(100);
            meetingFeeCreated.FeeCurrency.Should().Be("PLN");
            meetingFeeCreated.Status.Should().Be(MeetingFeeStatus.WaitingForPayment.Code);
        }

        [Test]
        public void CreateMeetingFee_WithDifferentCurrency_IsSuccessful()
        {
            var payerId = new PayerId(Guid.NewGuid());
            var meetingId = new MeetingId(Guid.NewGuid());
            var fee = MoneyValue.Of(50, "USD");

            var meetingFee = MeetingFee.Create(payerId, meetingId, fee);

            var meetingFeeCreated = AssertPublishedDomainEvent<MeetingFeeCreatedDomainEvent>(meetingFee);
            meetingFeeCreated.FeeValue.Should().Be(50);
            meetingFeeCreated.FeeCurrency.Should().Be("USD");
        }

        [Test]
        public void MarkMeetingFeeAsPaid_IsSuccessful()
        {
            var payerId = new PayerId(Guid.NewGuid());
            var meetingId = new MeetingId(Guid.NewGuid());
            var fee = MoneyValue.Of(100, "PLN");

            var meetingFee = MeetingFee.Create(payerId, meetingId, fee);

            meetingFee.MarkAsPaid();

            var meetingFeePaid = AssertPublishedDomainEvent<MeetingFeePaidDomainEvent>(meetingFee);
            meetingFeePaid.MeetingFeeId.Should().Be(meetingFee.Id);
            meetingFeePaid.Status.Should().Be(MeetingFeeStatus.Paid.Code);
        }

        [Test]
        public void GetSnapshot_ReturnsCorrectData()
        {
            var payerId = new PayerId(Guid.NewGuid());
            var meetingId = new MeetingId(Guid.NewGuid());
            var fee = MoneyValue.Of(75, "EUR");

            var meetingFee = MeetingFee.Create(payerId, meetingId, fee);

            var snapshot = meetingFee.GetSnapshot();

            snapshot.MeetingFeeId.Should().Be(meetingFee.Id);
            snapshot.PayerId.Should().Be(payerId.Value);
            snapshot.MeetingId.Should().Be(meetingId.Value);
        }

        [Test]
        public void CreateMeetingFee_AssignsUniqueId()
        {
            var payerId = new PayerId(Guid.NewGuid());
            var meetingId = new MeetingId(Guid.NewGuid());
            var fee = MoneyValue.Of(100, "PLN");

            var meetingFee1 = MeetingFee.Create(payerId, meetingId, fee);
            var meetingFee2 = MeetingFee.Create(payerId, meetingId, fee);

            meetingFee1.Id.Should().NotBe(meetingFee2.Id);
        }

        [Test]
        public void CreateMeetingFee_WithZeroFee_IsSuccessful()
        {
            var payerId = new PayerId(Guid.NewGuid());
            var meetingId = new MeetingId(Guid.NewGuid());
            var fee = MoneyValue.Of(0, "PLN");

            var meetingFee = MeetingFee.Create(payerId, meetingId, fee);

            var meetingFeeCreated = AssertPublishedDomainEvent<MeetingFeeCreatedDomainEvent>(meetingFee);
            meetingFeeCreated.FeeValue.Should().Be(0);
        }
    }
}
