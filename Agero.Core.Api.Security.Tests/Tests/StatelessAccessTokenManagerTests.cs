using Agero.Core.Api.Security.Helpers;
using Agero.Core.Api.Security.Services.Implementations;
using Agero.Core.Api.Security.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Agero.Core.Api.Security.Tests.Tests
{
    [TestClass]
    public class StatelessAccessTokenManagerTests
    {
        [TestMethod]
        public void Generate_ExtractData()
        {
            // Arrange
            var keyAndIv = RijndaelHelper.GenerateKeyAndIV();
            var key = Base64Helper.Encode(keyAndIv.Item1);
            var iv = Base64Helper.Encode(keyAndIv.Item2);

            const int EXPIRE_IN_MINUTES = 120;

            var manager = new StatelessAccessTokenManager(key, iv);
            
            // Act
            var token = manager.Generate(ConstantHelper.Username.ExistingAndAuthorized, EXPIRE_IN_MINUTES);
            var data = manager.ExtractData(token);

            // Assert
            Assert.AreEqual(ConstantHelper.Username.ExistingAndAuthorized, data.Username);
            Assert.IsTrue(DateTime.UtcNow.AddMinutes(EXPIRE_IN_MINUTES).AddSeconds(-30) < data.ExpirationUtcTime);
            Assert.IsTrue(DateTime.UtcNow.AddMinutes(EXPIRE_IN_MINUTES).AddSeconds(30) > data.ExpirationUtcTime);
        }

        [TestMethod]
        public void ExtractData_Should_Throw_Argument_Exception_When_Access_Token_Is_Not_Base64()
        {
            // Arrange
            var keyAndIv = RijndaelHelper.GenerateKeyAndIV();
            var key = Base64Helper.Encode(keyAndIv.Item1);
            var iv = Base64Helper.Encode(keyAndIv.Item2);

            var manager = new StatelessAccessTokenManager(key, iv);

            // Act
            // Assert
            ExceptionHelper.Assert<ArgumentException>(() => manager.ExtractData("abcdefghijklmnopq"));
        }

        [TestMethod]
        public void ExtractData_Should_Throw_Argument_Exception_When_Access_Token_Cannot_Be_Extracted()
        {
            // Arrange
            var keyAndIv = RijndaelHelper.GenerateKeyAndIV();
            var key = Base64Helper.Encode(keyAndIv.Item1);
            var iv = Base64Helper.Encode(keyAndIv.Item2);

            var manager = new StatelessAccessTokenManager(key, iv);

            // Act
            // Assert
            ExceptionHelper.Assert<ArgumentException>(() => manager.ExtractData("abcd"));
        }
    }
}
