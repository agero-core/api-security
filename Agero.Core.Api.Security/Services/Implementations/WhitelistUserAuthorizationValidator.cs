using Agero.Core.Api.Security.Services.Interfaces;
using Agero.Core.Checker;
using System;
using System.Linq;

namespace Agero.Core.Api.Security.Services.Implementations
{
    /// <summary>Validates user's authorization based on white list</summary>
    public class WhitelistUserAuthorizationValidator : IUserAuthorizationValidator<object>
    {
        /// <summary>Constructor</summary>
        /// <param name="usernames">White list of usernames. If null then all usernames are in white list.</param>
        /// <param name="ignoreCase">Specifies whether username case is ignored</param>
        public WhitelistUserAuthorizationValidator(string[] usernames = null, bool ignoreCase = false)
        {
            Check.Argument(usernames == null || usernames.All(u => !string.IsNullOrWhiteSpace(u) && u.Length <= 50), "Several of usernames are invalid.");

            Usernames = usernames;
            IgnoreCase = ignoreCase;
        }
        
        /// <summary>White list usernames</summary>
        public string[] Usernames { get; }

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

            if (Usernames == null)
                return true;

            return Usernames.Contains(username, IgnoreCase ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture);
        }
    }
}
