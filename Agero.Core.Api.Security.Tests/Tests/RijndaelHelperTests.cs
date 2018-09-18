using Agero.Core.Api.Security.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agero.Core.Api.Security.Tests.Tests
{
    [TestClass]
    public class RijndaelHelperTests
    {
        [TestMethod]
        public void Encrypt_Decrypt()
        {
            // Arrange
            var keyAndIv = RijndaelHelper.GenerateKeyAndIV();

            const string TEXT = @"{ message: ""!@!#$%^&435647768hgnhgdnhndfg45y56gfgfg"", number: ""124335"", flag: ""false"" }";

            // Act
            var encryptedText = RijndaelHelper.Encrypt(TEXT, keyAndIv.Item1, keyAndIv.Item2);
            var resultText = RijndaelHelper.Decrypt(encryptedText, keyAndIv.Item1, keyAndIv.Item2);

            // Assert
            Assert.AreEqual(TEXT, resultText);
        }
    }
}
