using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Theater.Application.Credentials;
using Theater.Domain.Credentials;
using Theater.Infra.Data.Repositories;
using Theater.Integration.Tests.Common;

namespace Theater.Integration.Tests.Application
{
    [TestClass]
    public class CredentialsServiceTests : TheaterIntegrationBase
    {
        private ICredentialsService _credentialsService;
        private ICredentialsRepository _credentialsRepository;

        [TestInitialize]
        public void Initialize()
        {
            base.Reset();

            _credentialsRepository = new CredentialsRepository(_theaterContext);
            _credentialsService = new CredentialsService(_credentialsRepository);
        }

        [TestMethod]
        public void CredentialsService_Authenticate_Should_True_When_Login_And_PassWord_Are_Equal_Database_Login_And_PassWord()
        {
            string login = "user";
            string password = "secret";

            Credential credential = new Credential()
            {
                Login = login,
                Password = password
            };

            Credential credentialToAuthenticate = new Credential()
            {
                Login = login,
                Password = password
            };

            _credentialsRepository.AddCredential(credential);

            bool result = _credentialsService.Authenticate(credentialToAuthenticate);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void CredentialsService_Authenticate_Should_False_When_Login_Is_Equal_Database_Login_But_PassWord_Is_Different()
        {
            string login = "user";
            string password = "secret";

            Credential credential = new Credential()
            {
                Login = login,
                Password = password
            };

            Credential credentialToAuthenticate = new Credential()
            {
                Login = login,
                Password = "asdf"
            };

            _credentialsRepository.AddCredential(credential);

            bool result = _credentialsService.Authenticate(credentialToAuthenticate);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void CredentialsService_Authenticate_Should_Throw_Exception_When_Login_Is_Not_Found_In_Database()
        {
            string login = "user";
            string password = "secret";

            Credential credential = new Credential()
            {
                Login = login,
                Password = password
            };

            Credential credentialToAuthenticate = new Credential()
            {
                Login = "quert",
                Password = "asdf"
            };


            _credentialsRepository.AddCredential(credential);

            Action act = () => _credentialsService.Authenticate(credentialToAuthenticate);

            act.Should().Throw<Exception>().WithMessage("Usuário não localizado");
        }
    }
}
