namespace Agero.Core.Api.Security.Exceptions
{
    /// <summary>API security validation error code</summary>
    public enum ApiSecurityValidationErrorCode
    {
        /// <summary>Invalid access token</summary>
        InvalidAccessToken,

        /// <summary>User is not authorized</summary>
        UserNotAuthorized,

        /// <summary>Access token expired</summary>
        AccessTokenExpired,
    }
}
