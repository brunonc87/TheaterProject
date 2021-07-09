using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Theater.Domain.Credentials;

namespace Theater.Unit.Tests.Domain
{
    [TestClass]
    public class CredentialTests
    {
        [TestMethod]
        public void Credential_ValidatePassword_Should_Return_True_When_Password_Is_Valid()
        {
            string password = "P@ssw0rd";
            Credential credential = new Credential() { Login = "user", Password = password };

            bool result = credential.ValidatePassword(password);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void Credential_ValidatePassword_Should_Return_False_When_Password_Is_The_Same_With_Case_Sensitive()
        {
            Credential credential = new Credential() { Login = "user", Password = "P@ssw0rd" };

            bool result = credential.ValidatePassword("p@ssw0rd");

            result.Should().BeFalse();
        }

        [TestMethod]
        public void Credential_ValidatePassword_Should_Return_False_When_Password_Is_Different()
        {
            Credential credential = new Credential() { Login = "user", Password = "P@ssw0rd" };

            bool result = credential.ValidatePassword("otherpass");

            result.Should().BeFalse();
        }
    }
}
