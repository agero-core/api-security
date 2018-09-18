using Agero.Core.Api.Security.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agero.Core.Api.Security.Tests.Tests
{
    [TestClass]
    public class AgeroActiveDirectoryUsernamePasswordValidatorTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("Ignore")]
        public void Validate_Should_Return_True_When_Username_And_Password_Are_Valid()
        {
            // Arrange
            var validator = new AgeroActiveDirectoryUsernamePasswordValidator(); 

            // Act
            var result = validator.Validate("csr1", "Oneroad2013");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("Ignore")]
        public void Validate_Should_Return_False_When_Username_Is_Invalid()
        {
            // Arrange
            var validator = new AgeroActiveDirectoryUsernamePasswordValidator();

            // Act
            var result = validator.Validate("invaliduser", "password");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("Ignore")]
        public void Validate_Should_Return_False_When_Password_Is_Invalid()
        {
            // Arrange
            var validator = new AgeroActiveDirectoryUsernamePasswordValidator();

            // Act
            var result = validator.Validate("csr1", "invalidpassword");

            // Assert
            Assert.IsFalse(result);
        }
    }
}
