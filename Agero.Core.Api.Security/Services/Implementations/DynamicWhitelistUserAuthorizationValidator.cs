using Agero.Core.Api.Security.Services.Interfaces;
using Agero.Core.Checker;
using System;
using System.Linq;

namespace Agero.Core.Api.Security.Services.Implementations
{
    /// <summary>Validates user's authorization based on dynamic white list</summary>
    public class DynamicWhitelistUserAuthorizationValidator : IUserAuthorizationValidator<object>
    {
        /// <summary>Constructor</summary>
        /// <param name="getUsernames">Callback to retrieve a white list of usernames.</param>
        /// <param name="ignoreCase">Specifies whether username case is ignored</param>
        public DynamicWhitelistUserAuthorizationValidator(Func<string[]> getUsernames = null, bool ignoreCase = false)
        {
            Check.ArgumentIsNull(getUsernames, "getUsernames");

            GetUsernames = getUsernames;
            IgnoreCase = ignoreCase;
        }

        /// <summary>Callback to retrieve a white list of usernames</summary>
        public Func<string[]> GetUsernames { get; }

        /// <summary>Specifies whether username case is ignored</summary>
        public bool IgnoreCase { get; }

        /// <summary>Validates user's authorization for resource ID</summary>
        /// <exception cref="ArgumentException">When username is invalid</exception>
        /// <exception cref="ArgumentException">When resource ID is invalid</exception>
        /// <returns>Returns true if user is authorized, otherwise false</returns>
        public bool Validate(string username, object resourceId = null)
        {
            Check.ArgumentIsNullOrWhiteSpace(username, "username");
            Check.Argument(username.Length <= 50, "username.Length <= 50");

            var usernames = GetUsernames();

            return usernames.Contains(username, IgnoreCase ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture);
        }
    }
}
