using CompanyName.MyMeetings.Modules.UserAccess.Domain.UnitTests.SeedWork;
using CompanyName.MyMeetings.Modules.UserAccess.Domain.Users;
using CompanyName.MyMeetings.Modules.UserAccess.Domain.Users.Events;
using FluentAssertions;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UserAccess.Domain.UnitTests.Users
{
    [TestFixture]
    public class UserTests : TestBase
    {
        [Test]
        public void CreateAdmin_IsSuccessful()
        {
            var user = User.CreateAdmin(
                "adminLogin",
                "adminPassword",
                "admin@mail.com",
                "Admin",
                "User",
                "Admin User");

            var userCreated = AssertPublishedDomainEvent<UserCreatedDomainEvent>(user);
            userCreated.Id.Should().Be(user.Id);
        }

        [Test]
        public void CreateUser_IsSuccessful()
        {
            var userId = Guid.NewGuid();
            var user = User.CreateUser(
                userId,
                "memberLogin",
                "memberPassword",
                "member@mail.com",
                "John",
                "Doe");

            var userCreated = AssertPublishedDomainEvent<UserCreatedDomainEvent>(user);
            userCreated.Id.Should().Be(user.Id);
            user.Id.Value.Should().Be(userId);
        }

        [Test]
        public void CreateAdmin_PublishesExactlyOneUserCreatedDomainEvent()
        {
            var user = User.CreateAdmin(
                "adminLogin",
                "adminPassword",
                "admin@mail.com",
                "Admin",
                "User",
                "Admin User");

            var domainEvents = DomainEventsTestHelper.GetAllDomainEvents(user);
            domainEvents.OfType<UserCreatedDomainEvent>().Should().HaveCount(1);
        }

        [Test]
        public void CreateUser_PublishesExactlyOneUserCreatedDomainEvent()
        {
            var userId = Guid.NewGuid();
            var user = User.CreateUser(
                userId,
                "memberLogin",
                "memberPassword",
                "member@mail.com",
                "Jane",
                "Smith");

            var domainEvents = DomainEventsTestHelper.GetAllDomainEvents(user);
            domainEvents.OfType<UserCreatedDomainEvent>().Should().HaveCount(1);
        }

        [Test]
        public void CreateUser_WithDifferentUserIds_ProducesDifferentIds()
        {
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            var user1 = User.CreateUser(userId1, "login1", "pass1", "e1@mail.com", "A", "B");
            var user2 = User.CreateUser(userId2, "login2", "pass2", "e2@mail.com", "C", "D");

            user1.Id.Should().NotBe(user2.Id);
        }

        [Test]
        public void CreateAdmin_GeneratesNewId()
        {
            var admin1 = User.CreateAdmin("login1", "pass1", "e1@mail.com", "A", "B", "A B");
            var admin2 = User.CreateAdmin("login2", "pass2", "e2@mail.com", "C", "D", "C D");

            admin1.Id.Should().NotBe(admin2.Id);
        }
    }
}
