using System;

namespace Agero.Core.Api.Security.Exceptions
{
    /// <summary>API security validation exception</summary>
    [Serializable]
    public class ApiSecurityValidationException : Exception
    {
        /// <summary>Constructor</summary>
        public ApiSecurityValidationException(string message, ApiSecurityValidationErrorCode errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>Constructor</summary>
        public ApiSecurityValidationException(string message, ApiSecurityValidationErrorCode errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>Error code</summary>
        public ApiSecurityValidationErrorCode ErrorCode { get; }
    }
}
