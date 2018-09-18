namespace Agero.Core.Api.Security.Exceptions
{
    /// <summary>API security login error code</summary>
    public enum ApiSecurityLoginErrorCode
    {
        /// <summary>Wrong username/password</summary>
        WrongUsernamePassword,

        /// <summary>User is not authorized</summary>
        UserNotAuthorized
    }
}
