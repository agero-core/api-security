using System;
using Agero.Core.Api.Security.Exceptions;

namespace Agero.Core.Api.Security
{
    /// <summary>Helper which provides functionality to secure API</summary>
    public interface IApiSecurityHelper<in TResource>
    {
        /// <summary>Verifies username/password and returns access token. If fails then throws exception.</summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="expireInMinutes">Number of minutes from current time when access token will expire</param>
        /// <param name="validateUserAuthorization">Specifies whether user authorization needs to be validated</param>
        /// <exception cref="ArgumentException">When username is empty</exception>
        /// <exception cref="ArgumentException">When password is empty</exception>
        /// <exception cref="ArgumentException">When expire in minutes is invalid</exception>
        /// <exception cref="ApiSecurityLoginException">When login is failed</exception>
        /// <returns>Access token if successfull</returns>
        string Login(string username, string password, int expireInMinutes, bool validateUserAuthorization = true);

        /// <summary>Validates access token for resource. If fails then throws exception.</summary>
        /// <param name="accessToken">Access token returned by login method</param>
        /// <param name="resource">Current resouce</param>
        /// <exception cref="ArgumentException">When access token is empty</exception>
        /// <exception cref="ApiSecurityValidationException">When validation failed</exception>
        void Validate(string accessToken, TResource resource = default(TResource));
    }
}
