using CompanyName.MyMeetings.Modules.Payments.Domain.Payers;
using CompanyName.MyMeetings.Modules.Payments.Domain.SeedWork;
using CompanyName.MyMeetings.Modules.Payments.Domain.SubscriptionPayments;
using CompanyName.MyMeetings.Modules.Payments.Domain.SubscriptionRenewalPayments;
using CompanyName.MyMeetings.Modules.Payments.Domain.Subscriptions;
using CompanyName.MyMeetings.Modules.Payments.Domain.Subscriptions.Events;
using CompanyName.MyMeetings.Modules.Payments.Domain.UnitTests.SeedWork;
using FluentAssertions;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.Payments.Domain.UnitTests.Subscriptions
{
    [TestFixture]
    public class SubscriptionExpireNegativeTests : TestBase
    {
        [Test]
        public void ExpireSubscription_WhenNotYetExpired_DoesNotPublishEvent()
        {
            var referenceDate = DateTime.UtcNow;
            SystemClock.Set(referenceDate);

            var subscriptionPaymentSnapshot = new SubscriptionPaymentSnapshot(
                new SubscriptionPaymentId(Guid.NewGuid()),
                new PayerId(Guid.NewGuid()),
                SubscriptionPeriod.Month,
                "PL");

            var subscription = Subscription.Create(subscriptionPaymentSnapshot);

            subscription.Expire();

            AssertDomainEventNotPublished<SubscriptionExpiredDomainEvent>(subscription);
        }

        [Test]
        public void CreateSubscription_WithHalfYearPeriod_IsSuccessful()
        {
            var subscriptionPaymentSnapshot = new SubscriptionPaymentSnapshot(
                new SubscriptionPaymentId(Guid.NewGuid()),
                new PayerId(Guid.NewGuid()),
                SubscriptionPeriod.HalfYear,
                "US");

            var subscription = Subscription.Create(subscriptionPaymentSnapshot);

            var created = AssertPublishedDomainEvent<SubscriptionCreatedDomainEvent>(subscription);
            created.SubscriptionPeriodCode.Should().Be(SubscriptionPeriod.HalfYear.Code);
        }

        [Test]
        public void CreateSubscription_WithDifferentCountry_IsSuccessful()
        {
            var subscriptionPaymentSnapshot = new SubscriptionPaymentSnapshot(
                new SubscriptionPaymentId(Guid.NewGuid()),
                new PayerId(Guid.NewGuid()),
                SubscriptionPeriod.Month,
                "DE");

            var subscription = Subscription.Create(subscriptionPaymentSnapshot);

            var created = AssertPublishedDomainEvent<SubscriptionCreatedDomainEvent>(subscription);
            created.CountryCode.Should().Be("DE");
        }

        [Test]
        public void ExpireSubscription_AfterHalfYearPeriod_IsSuccessful()
        {
            var referenceDate = DateTime.UtcNow;
            SystemClock.Set(referenceDate);

            var subscriptionPaymentSnapshot = new SubscriptionPaymentSnapshot(
                new SubscriptionPaymentId(Guid.NewGuid()),
                new PayerId(Guid.NewGuid()),
                SubscriptionPeriod.HalfYear,
                "PL");

            var subscription = Subscription.Create(subscriptionPaymentSnapshot);

            SystemClock.Set(referenceDate.AddMonths(6).AddMilliseconds(1));

            subscription.Expire();

            AssertPublishedDomainEvent<SubscriptionExpiredDomainEvent>(subscription);
        }

        [Test]
        public void RenewSubscription_WithHalfYearPeriod_IsSuccessful()
        {
            var subscriptionPaymentSnapshot = new SubscriptionPaymentSnapshot(
                new SubscriptionPaymentId(Guid.NewGuid()),
                new PayerId(Guid.NewGuid()),
                SubscriptionPeriod.Month,
                "PL");

            var subscription = Subscription.Create(subscriptionPaymentSnapshot);

            var subscriptionRenewalPaymentSnapshot = new SubscriptionRenewalPaymentSnapshot(
                new SubscriptionRenewalPaymentId(Guid.NewGuid()),
                new PayerId(Guid.NewGuid()),
                SubscriptionPeriod.HalfYear,
                "PL");

            subscription.Renew(subscriptionRenewalPaymentSnapshot);

            AssertPublishedDomainEvent<SubscriptionRenewedDomainEvent>(subscription);
        }
    }
}
