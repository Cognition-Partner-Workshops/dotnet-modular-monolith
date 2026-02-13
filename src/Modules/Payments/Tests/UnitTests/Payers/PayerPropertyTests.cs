using CompanyName.MyMeetings.Modules.Payments.Domain.Payers;
using CompanyName.MyMeetings.Modules.Payments.Domain.Payers.Events;
using CompanyName.MyMeetings.Modules.Payments.Domain.UnitTests.SeedWork;
using FluentAssertions;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.Payments.Domain.UnitTests.Payers
{
    [TestFixture]
    public class PayerPropertyTests : TestBase
    {
        [Test]
        public void CreatePayer_AllPropertiesAreSetCorrectly()
        {
            var payerId = Guid.NewGuid();

            var payer = Payer.Create(
                payerId,
                "testLogin",
                "test@mail.com",
                "John",
                "Doe",
                "John Doe");

            var payerCreated = AssertPublishedDomainEvent<PayerCreatedDomainEvent>(payer);

            payerCreated.PayerId.Should().Be(payerId);
            payerCreated.Login.Should().Be("testLogin");
            payerCreated.Email.Should().Be("test@mail.com");
            payerCreated.FirstName.Should().Be("John");
            payerCreated.LastName.Should().Be("Doe");
            payerCreated.Name.Should().Be("John Doe");
        }

        [Test]
        public void CreatePayer_AssignsIdCorrectly()
        {
            var payerId = Guid.NewGuid();

            var payer = Payer.Create(
                payerId,
                "login",
                "email@mail.com",
                "First",
                "Last",
                "First Last");

            payer.Id.Should().Be(payerId);
        }

        [Test]
        public void CreatePayer_WithDifferentData_IsSuccessful()
        {
            var payerId = Guid.NewGuid();

            var payer = Payer.Create(
                payerId,
                "anotherLogin",
                "another@mail.com",
                "Jane",
                "Smith",
                "Jane Smith");

            var payerCreated = AssertPublishedDomainEvent<PayerCreatedDomainEvent>(payer);

            payerCreated.Login.Should().Be("anotherLogin");
            payerCreated.Email.Should().Be("another@mail.com");
            payerCreated.FirstName.Should().Be("Jane");
            payerCreated.LastName.Should().Be("Smith");
        }

        [Test]
        public void CreatePayer_PublishesExactlyOneEvent()
        {
            var payerId = Guid.NewGuid();

            var payer = Payer.Create(
                payerId,
                "login",
                "email@mail.com",
                "First",
                "Last",
                "First Last");

            var events = payer.GetDomainEvents().OfType<PayerCreatedDomainEvent>().ToList();
            events.Should().HaveCount(1);
        }
    }
}
