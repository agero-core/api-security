using System;

namespace Agero.Core.Api.Security.Exceptions
{
    /// <summary>API security login exception</summary>
    [Serializable]
    public class ApiSecurityLoginException : Exception
    {
        /// <summary>Constructor</summary>
        public ApiSecurityLoginException(string message, ApiSecurityLoginErrorCode errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>Error code</summary>
        public ApiSecurityLoginErrorCode ErrorCode { get; }
    }
}
