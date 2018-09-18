using Agero.Core.Api.Security.Exceptions;
using Agero.Core.Api.Security.Helpers;
using Agero.Core.Api.Security.Services.Implementations;
using Agero.Core.Api.Security.Services.Interfaces;
using Agero.Core.Api.Security.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Agero.Core.Api.Security.Tests.Tests
{
    [TestClass]
    public class ApiSecurityHelperTests
    {
        private static ApiSecurityHelper<object> CreateApiSecurityHelper()
        {
            var usernamePasswordValidatorMock = new Mock<IUsernamePasswordValidator>(MockBehavior.Strict);

            var userAuthorizationValidator = new Mock<IUserAuthorizationValidator<object>>(MockBehavior.Strict);

            var accessTokenManager = new Mock<IAccessTokenManager>(MockBehavior.Strict);

            return 
                new ApiSecurityHelper<object>(
                    usernamePasswordValidatorMock.Object, 
                    userAuthorizationValidator.Object,
                    accessTokenManager.Object);
        }

        [TestMethod]
        public void Login_Should_Return_AccessToken_When_Username_Password_Is_Valid_And_User_Is_Authorized()
        {
            // Arrange
            var helper = CreateApiSecurityHelper();

            var usernamePasswordValidatorMock = Mock.Get(helper.UsernamePasswordValidator);
            usernamePasswordValidatorMock
                .Setup(m => m.Validate(ConstantHelper.Username.ExistingAndAuthorized, ConstantHelper.Password))
                .Returns(true);

            var userAuthorizationValidatorMock = Mock.Get(helper.UserAuthorizationValidator);
            userAuthorizationValidatorMock
                .Setup(m => m.Validate(ConstantHelper.Username.ExistingAndAuthorized, null))
                .Returns(true);

            var accessTokenManagerMock = Mock.Get(helper.AccessTokenManager);
            accessTokenManagerMock
                .Setup(m => m.Generate(ConstantHelper.Username.ExistingAndAuthorized, ConstantHelper.ExpireInMinutes))
                .Returns(ConstantHelper.AccessToken.Valid);

            // Act
            var accessToken = helper.Login(ConstantHelper.Username.ExistingAndAuthorized, ConstantHelper.Password, ConstantHelper.ExpireInMinutes);

            // Assert
            Assert.AreEqual(ConstantHelper.AccessToken.Valid, accessToken);

            usernamePasswordValidatorMock.Verify(m => m.Validate(ConstantHelper.Username.ExistingAndAuthorized, ConstantHelper.Password), Times.Once);
            userAuthorizationValidatorMock.Verify(m => m.Validate(ConstantHelper.Username.ExistingAndAuthorized, null), Times.Once);
            accessTokenManagerMock.Verify(m => m.Generate(ConstantHelper.Username.ExistingAndAuthorized, ConstantHelper.ExpireInMinutes), Times.Once);
        }

        [TestMethod]
        public void Login_Should_Throw_WrongUsernamePassword_Error_When_Username_Paswword_Is_Wrong()
        {
            // Arrange
            var helper = CreateApiSecurityHelper();

            var usernamePasswordValidatorMock = Mock.Get(helper.UsernamePasswordValidator);
            usernamePasswordValidatorMock
                .Setup(m => m.Validate(ConstantHelper.Username.NonExisting, ConstantHelper.Password))
                .Returns(false);

            // Act
            // Assert
            ExceptionHelper.Assert<ApiSecurityLoginException>(
                () => helper.Login(ConstantHelper.Username.NonExisting, ConstantHelper.Password, ConstantHelper.ExpireInMinutes),
                ex => ex.ErrorCode == ApiSecurityLoginErrorCode.WrongUsernamePassword);

            usernamePasswordValidatorMock.Verify(m => m.Validate(ConstantHelper.Username.NonExisting, ConstantHelper.Password), Times.Once);
        }

        [TestMethod]
        public void Login_Should_Throw_UserNotAuthorized_Error_When_User_Is_Not_Authorized()
        {
            // Arrange
            var helper = CreateApiSecurityHelper();

            var usernamePasswordValidatorMock = Mock.Get(helper.UsernamePasswordValidator);
            usernamePasswordValidatorMock
                .Setup(m => m.Validate(ConstantHelper.Username.NonAuthorized, ConstantHelper.Password))
                .Returns(true);

            var userAuthorizationValidatorMock = Mock.Get(helper.UserAuthorizationValidator);
            userAuthorizationValidatorMock
                .Setup(m => m.Validate(ConstantHelper.Username.NonAuthorized, null))
                .Returns(false);

            // Act
            // Assert
            ExceptionHelper.Assert<ApiSecurityLoginException>(
                () => helper.Login(ConstantHelper.Username.NonAuthorized, ConstantHelper.Password, ConstantHelper.ExpireInMinutes),
                ex => ex.ErrorCode == ApiSecurityLoginErrorCode.UserNotAuthorized);

            usernamePasswordValidatorMock.Verify(m => m.Validate(ConstantHelper.Username.NonAuthorized, ConstantHelper.Password), Times.Once);
            userAuthorizationValidatorMock.Verify(m => m.Validate(ConstantHelper.Username.NonAuthorized, null), Times.Once);
        }

        [TestMethod]
        public void Validate_Should_Not_Throw_Exception_When_Access_Token_Is_Valid_And_User_Is_Authorized()
        {
            // Arrange
            var helper = CreateApiSecurityHelper();

            var accessTokenManagerMock = Mock.Get(helper.AccessTokenManager);
            accessTokenManagerMock
                .Setup(m => m.ExtractData(ConstantHelper.AccessToken.Valid))
                .Returns(ConstantHelper.AccessTokenData.AuthorizedUserAndNotExpired);

            var userAuthorizationValidatorMock = Mock.Get(helper.UserAuthorizationValidator);
            userAuthorizationValidatorMock
                .Setup(m => m.Validate(ConstantHelper.Username.ExistingAndAuthorized, ConstantHelper.ResourceId))
                .Returns(true);
            
            // Act
            helper.Validate(ConstantHelper.AccessToken.Valid, ConstantHelper.ResourceId);

            // Assert
            accessTokenManagerMock.Verify(m => m.ExtractData(ConstantHelper.AccessToken.Valid), Times.Once);
            userAuthorizationValidatorMock.Verify(m => m.Validate(ConstantHelper.Username.ExistingAndAuthorized, ConstantHelper.ResourceId), Times.Once);
        }

        [TestMethod]
        public void Validate_Should_Throw_InvalidAccessToken_Error_When_Access_Token_Is_Invalid()
        {
            // Arrange
            var helper = CreateApiSecurityHelper();

            var accessTokenManagerMock = Mock.Get(helper.AccessTokenManager);
            accessTokenManagerMock
                .Setup(m => m.ExtractData(ConstantHelper.AccessToken.Invalid))
                .Throws(new ArgumentException("accessToken"));

            // Act
            // Assert
            ExceptionHelper.Assert<ApiSecurityValidationException>(
                () => helper.Validate(ConstantHelper.AccessToken.Invalid, ConstantHelper.ResourceId),
                ex => ex.ErrorCode == ApiSecurityValidationErrorCode.InvalidAccessToken);

            accessTokenManagerMock.Verify(m => m.ExtractData(ConstantHelper.AccessToken.Invalid), Times.Once);
        }

        [TestMethod]
        public void Validate_Should_Throw_UserNotAuthorized_Error_When_User_Is_Not_Authorized_For_Resource()
        {
            // Arrange
            var helper = CreateApiSecurityHelper();

            var accessTokenManagerMock = Mock.Get(helper.AccessTokenManager);
            accessTokenManagerMock
                .Setup(m => m.ExtractData(ConstantHelper.AccessToken.Valid))
                .Returns(ConstantHelper.AccessTokenData.NonAuthorizedUser);

            var userAuthorizationValidatorMock = Mock.Get(helper.UserAuthorizationValidator);
            userAuthorizationValidatorMock
                .Setup(m => m.Validate(ConstantHelper.Username.NonAuthorized, ConstantHelper.ResourceId))
                .Returns(false);

            // Act
            // Assert
            ExceptionHelper.Assert<ApiSecurityValidationException>(
                () => helper.Validate(ConstantHelper.AccessToken.Valid, ConstantHelper.ResourceId),
                ex => ex.ErrorCode == ApiSecurityValidationErrorCode.UserNotAuthorized);

            accessTokenManagerMock.Verify(m => m.ExtractData(ConstantHelper.AccessToken.Valid), Times.Once);
            userAuthorizationValidatorMock.Verify(m => m.Validate(ConstantHelper.Username.NonAuthorized, ConstantHelper.ResourceId), Times.Once);
        }

        [TestMethod]
        public void Validate_Should_Throw_AccessTokenExpired_Error_When_Access_Token_Is_Expired()
        {
            // Arrange
            var helper = CreateApiSecurityHelper();

            var accessTokenManagerMock = Mock.Get(helper.AccessTokenManager);
            accessTokenManagerMock
                .Setup(m => m.ExtractData(ConstantHelper.AccessToken.Valid))
                .Returns(ConstantHelper.AccessTokenData.Expired);

            // Act
            // Assert
            ExceptionHelper.Assert<ApiSecurityValidationException>(
                () => helper.Validate(ConstantHelper.AccessToken.Valid, ConstantHelper.ResourceId),
                ex => ex.ErrorCode == ApiSecurityValidationErrorCode.AccessTokenExpired);

            accessTokenManagerMock.Verify(m => m.ExtractData(ConstantHelper.AccessToken.Valid), Times.Once);
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("Ignore")]
        public void Integration()
        {
            // Arrange
            const string USERNAME = "csr1";

            var usernamePasswordValidator = new AgeroActiveDirectoryUsernamePasswordValidator();

            var userAuthorizationValidator = new WhitelistUserAuthorizationValidator(new [] { USERNAME });

            var keyAndIv = RijndaelHelper.GenerateKeyAndIV();
            var key = Base64Helper.Encode(keyAndIv.Item1);
            var iv = Base64Helper.Encode(keyAndIv.Item2);
            var accessTokenManager = new StatelessAccessTokenManager(key, iv);

            var helper = new ApiSecurityHelper<object>(usernamePasswordValidator, userAuthorizationValidator, accessTokenManager);

            // Act 
            var accessToken = helper.Login(USERNAME, "Oneroad2013", 5);
            helper.Validate(accessToken);
        }
    }
}
