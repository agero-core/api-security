using Agero.Core.Api.Security.Exceptions;
using Agero.Core.Api.Security.Services.Interfaces;
using Agero.Core.Checker;
using System;

namespace Agero.Core.Api.Security
{
    /// <summary>Helper which provides functionality to secure API</summary>
    public class ApiSecurityHelper<TResource> : IApiSecurityHelper<TResource>
    {
        /// <summary>Constructor</summary>
        public ApiSecurityHelper(IUsernamePasswordValidator usernamePasswordValidator, IUserAuthorizationValidator<TResource> userAuthorizationValidator, IAccessTokenManager accessTokenManager)
        {
            Check.ArgumentIsNull(usernamePasswordValidator, "usernamePasswordValidator");
            Check.ArgumentIsNull(userAuthorizationValidator, "userAuthorizationValidator");
            Check.ArgumentIsNull(accessTokenManager, "accessTokenManager");

            UsernamePasswordValidator = usernamePasswordValidator;
            UserAuthorizationValidator = userAuthorizationValidator;
            AccessTokenManager = accessTokenManager;
        }

        /// <summary>Username/password validator</summary>
        public IUsernamePasswordValidator UsernamePasswordValidator { get; }

        /// <summary>User authorization validator</summary>
        public IUserAuthorizationValidator<TResource> UserAuthorizationValidator { get; }

        /// <summary>Access token manager</summary>
        public IAccessTokenManager AccessTokenManager { get; }

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
        public string Login(string username, string password, int expireInMinutes, bool validateUserAuthorization = true)
        {
            Check.ArgumentIsNullOrWhiteSpace(username, "username");
            Check.ArgumentIsNullOrWhiteSpace(password, "password");
            Check.Argument(expireInMinutes > 0, "expireInMinutes > 0");

            if (!UsernamePasswordValidator.Validate(username, password))
                throw new ApiSecurityLoginException("Wrong username/password.", ApiSecurityLoginErrorCode.WrongUsernamePassword);

            if (validateUserAuthorization)
            {
                if (!UserAuthorizationValidator.Validate(username))
                    throw new ApiSecurityLoginException($"User '{username}' is not authorized to login.", ApiSecurityLoginErrorCode.UserNotAuthorized);
            }

            return AccessTokenManager.Generate(username, expireInMinutes);
        }

        /// <summary>Validates access token for resource. If fails then throws exception.</summary>
        /// <param name="accessToken">Access token returned by login method</param>
        /// <param name="resource">Current resouce ID</param>
        /// <exception cref="ArgumentException">When access token is empty</exception>
        /// <exception cref="ApiSecurityValidationException">When validation failed</exception>
        public void Validate(string accessToken, TResource resource = default(TResource))
        {
            Check.ArgumentIsNullOrWhiteSpace(accessToken, "accessToken");

            var data = ExtractTokenData(accessToken);

            if (DateTime.UtcNow >= data.ExpirationUtcTime)
                throw new ApiSecurityValidationException("Expired access token.", ApiSecurityValidationErrorCode.AccessTokenExpired);

            if (!UserAuthorizationValidator.Validate(data.Username, resource))
                throw new ApiSecurityValidationException($"User '{data.Username}' is not authorized to '{resource}' resource.", ApiSecurityValidationErrorCode.UserNotAuthorized);
        }

        private AccessTokenData ExtractTokenData(string accessToken)
        {
            Check.ArgumentIsNullOrWhiteSpace(accessToken, "accessToken");

            try
            {
                return AccessTokenManager.ExtractData(accessToken);
            }
            catch (ArgumentException ex)
            {
                throw new ApiSecurityValidationException("Invalid access token.", ApiSecurityValidationErrorCode.InvalidAccessToken, ex);
            }
        }
    }
}
