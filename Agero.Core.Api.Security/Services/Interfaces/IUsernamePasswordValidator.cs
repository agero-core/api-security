using System;

namespace Agero.Core.Api.Security.Services.Interfaces
{
    /// <summary>Validates username/password</summary>
    public interface IUsernamePasswordValidator
    {
        /// <summary>Validates username/password</summary>
        /// <exception cref="ArgumentException">When username is invalid</exception>
        /// <exception cref="ArgumentException">When password is invalid</exception>
        /// <returns>Returns true if username/password can be found, otherwise false</returns>
        bool Validate(string username, string password);
    }
}
