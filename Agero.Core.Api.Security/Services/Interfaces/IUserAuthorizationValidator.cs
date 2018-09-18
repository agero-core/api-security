using System;

namespace Agero.Core.Api.Security.Services.Interfaces
{
    /// <summary>Validates user's authorization</summary>
    public interface IUserAuthorizationValidator<in TResource>
    {
        /// <summary>Validates user's authorization for resource ID</summary>
        /// <exception cref="ArgumentException">When username is invalid</exception>
        /// <exception cref="ArgumentException">When resource ID is invalid</exception>
        /// <returns>Returns true if user is authorized, otherwise false</returns>
        bool Validate(string username, TResource resource = default(TResource));
    }
}
