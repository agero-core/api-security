using System;

namespace Agero.Core.Api.Security.Services.Interfaces
{
    /// <summary>Access token manager</summary>
    public interface IAccessTokenManager
    {
        /// <summary>Generates access token associated with username</summary>
        /// <param name="username">Username associated with access token</param>
        /// <param name="expireInMinutes">Number of minutes from current time when access token will expire</param>
        /// <exception cref="ArgumentException">When username is invalid</exception>
        /// <exception cref="ArgumentException">When expire in minutes is invalid</exception>
        string Generate(string username, int expireInMinutes);

        /// <summary>Extracts data associated with access token</summary>
        /// <exception cref="ArgumentException">When access token is invalid</exception>
        AccessTokenData ExtractData(string accessToken);
    }
}
