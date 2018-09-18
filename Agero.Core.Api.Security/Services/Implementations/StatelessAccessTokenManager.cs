using Agero.Core.Api.Security.Helpers;
using Agero.Core.Api.Security.Services.Interfaces;
using Agero.Core.Checker;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;

namespace Agero.Core.Api.Security.Services.Implementations
{
    /// <summary>Stateless access token manager which uses Rijndael (256-bit) encryption</summary>
    public class StatelessAccessTokenManager : IAccessTokenManager
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        /// <summary>Constructor</summary>
        /// <param name="key">Rijndael key encoded by Base64</param>
        /// <param name="initializationVector">Rijndael initialization vector encoded by Base64</param>
        public StatelessAccessTokenManager(string key, string initializationVector)
        {
            Check.ArgumentIsNullOrWhiteSpace(key, "key");
            Check.ArgumentIsNullOrWhiteSpace(initializationVector, "initializationVector");

            _key = Base64Helper.Decode(key);
            _iv = Base64Helper.Decode(initializationVector);
        }

        /// <summary>Generates access token associated with username</summary>
        /// <param name="username">Username associated with access token</param>
        /// <param name="expireInMinutes">Number of minutes from current time when access token will expire</param>
        /// <exception cref="ArgumentException">When username is invalid</exception>
        /// <exception cref="ArgumentException">When expire in minutes is invalid</exception>
        public string Generate(string username, int expireInMinutes)
        {
            Check.ArgumentIsNullOrWhiteSpace(username, "username");
            Check.Argument(username.Length <= 50, "username.Length <= 50");
            Check.Argument(expireInMinutes > 0, "expireInMinutes > 0");

            var data = new AccessTokenData(username, DateTime.UtcNow.AddMinutes(expireInMinutes));
            var json = JsonConvert.SerializeObject(data);

            var encrypted = RijndaelHelper.Encrypt(json, _key, _iv);

            return Base64Helper.Encode(encrypted);
        }

        /// <summary>Extracts data associated with access token</summary>
        /// <exception cref="ArgumentException">When access token is invalid</exception>
        public AccessTokenData ExtractData(string accessToken)
        {
            Check.ArgumentIsNullOrWhiteSpace(accessToken, "accessToken");
            Check.Argument(accessToken.Length <= 1000, "accessToken.Length <= 1000");

            try
            {
                var encrypted = Base64Helper.Decode(accessToken);

                var json = RijndaelHelper.Decrypt(encrypted, _key, _iv);

                return JsonConvert.DeserializeObject<AccessTokenData>(json);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid access token.", nameof(accessToken), ex);
            }
            catch (CryptographicException ex)
            {
                throw new ArgumentException("Invalid access token.", nameof(accessToken), ex);
            }
        }
    }
}
