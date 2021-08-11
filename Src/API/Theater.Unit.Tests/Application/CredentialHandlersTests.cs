using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Theater.Application.Credentials;
using Theater.Application.Credentials.Commands;
using Theater.Application.Credentials.Handlers;
using Theater.Application.Credentials.Models;
using Theater.Domain.Credentials;

namespace Theater.Unit.Tests.Application
{
    [TestClass]
    public class CredentialHandlersTests
    {
        private Mock<ICredentialsRepository> _credentialsRepository;
        private IMapper _mapper;
        private CredentialHandler _credentialHandler;

        [TestInitialize]
        public void Initialize()
        {
            _credentialsRepository = new Mock<ICredentialsRepository>();
            _mapper = new MapperConfiguration(mc => mc.AddProfile(new CredentialsMappingProfile())).CreateMapper();
            _credentialHandler = new CredentialHandler(_credentialsRepository.Object, _mapper);

        }

        [TestMethod]
        public void CredentialHandler_Should_Return_LoginInfoModel_When_Login_And_Password_Are_The_Same_As_Database()
        {
            string login = "admin";
            string password = "123";
            Credential credential = GetTestCredential(login, password);
            CredentialCommand credentialToAuthenticate = new CredentialCommand() { Login = login, Password = password };
            _credentialsRepository.Setup(x => x.RetrieveByLogonName(It.Is<string>(s => s == login))).Returns(credential);

            LoginInfoModel result = _credentialHandler.Handle(credentialToAuthenticate, new System.Threading.CancellationToken()).Result;

            result.Should().NotBeNull();
            result.Login.Should().Be(login);
            result.Token.Should().NotBeEmpty();

        }

        [TestMethod]
        public void CredentialHandler_Should_Throw_Exception_When_User_Is_On_Database_And_Password_Is_Not_The_Same()
        {
            string login = "admin";
            string password = "123";
            Credential credential = GetTestCredential(login, password);
            CredentialCommand credentialToAuthenticate = new CredentialCommand() { Login = login, Password = "asdf" };
            _credentialsRepository.Setup(x => x.RetrieveByLogonName(It.Is<string>(s => s == login))).Returns(credential);

            Action act = () => _credentialHandler.Handle(credentialToAuthenticate, new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Senha inválida");
        }

        [TestMethod]
        public void CredentialHandler_Should_Throw_Exception_When_User_Is_Not_Found_On_Database()
        {
            CredentialCommand credentialToAuthenticate = new CredentialCommand() { Login = "user", Password = "asdf" };
            _credentialsRepository.Setup(x => x.RetrieveByLogonName(It.IsAny<string>()));
            
            Action act = () => _credentialHandler.Handle(credentialToAuthenticate, new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Usuário não localizado");
        }

        private Credential GetTestCredential(string login, string password)
        {
            return new Credential
            {
                Login = login,
                Password = password
            };
        }
    }
}
