using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Theater.Application.Credentials.Commands;
using Theater.Application.Credentials.Handlers;
using Theater.Domain.Credentials;
using Theater.Infra.Data.Repositories;
using Theater.Integration.Tests.Common;

namespace Theater.Integration.Tests.Application
{
    [TestClass]
    public class CredentialsHandlersTests : TheaterIntegrationBase
    {

        private ICredentialsRepository _credentialsRepository;
        private CredentialHandler _credentialHandler;

        [TestInitialize]
        public void Initialize()
        {
            base.Reset();
            base.ConfigureAutomapper();
            _credentialsRepository = new CredentialsRepository(_theaterContext);
            _credentialHandler = new CredentialHandler(_credentialsRepository, _mapper);

        }

        [TestMethod]
        public void CredentialHandler_Should_True_When_Login_And_PassWord_Are_Equal_Database_Login_And_PassWord()
        {
            string login = "user";
            string password = "secret";

            Credential credential = new Credential()
            {
                Login = login,
                Password = password
            };

            CredentialCommand credentialToAuthenticate = new CredentialCommand()
            {
                Login = login,
                Password = password
            };

            _credentialsRepository.AddCredential(credential);


            var result = _credentialHandler.Handle(credentialToAuthenticate, new System.Threading.CancellationToken()).Result;

            result.Should().BeTrue();
        }

        [TestMethod]
        public void CredentialHandler_Should_False_When_Login_Is_Equal_Database_Login_But_PassWord_Is_Different()
        {
            string login = "user";
            string password = "secret";

            Credential credential = new Credential()
            {
                Login = login,
                Password = password
            };

            CredentialCommand credentialToAuthenticate = new CredentialCommand()
            {
                Login = login,
                Password = "asdf"
            };

            _credentialsRepository.AddCredential(credential);

            bool result = _credentialHandler.Handle(credentialToAuthenticate, new System.Threading.CancellationToken()).Result;

            result.Should().BeFalse();
        }

        [TestMethod]
        public void CredentialHandler_Should_Throw_Exception_When_Login_Is_Not_Found_In_Database()
        {
            string login = "user";
            string password = "secret";

            Credential credential = new Credential()
            {
                Login = login,
                Password = password
            };

            CredentialCommand credentialToAuthenticate = new CredentialCommand()
            {
                Login = "quert",
                Password = "asdf"
            };


            _credentialsRepository.AddCredential(credential);

            Action act = () => _credentialHandler.Handle(credentialToAuthenticate, new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Usuário não localizado");
        }
    }
}
