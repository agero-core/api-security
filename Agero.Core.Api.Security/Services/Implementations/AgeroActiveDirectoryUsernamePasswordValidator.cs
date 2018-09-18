using Agero.Core.Api.Security.Services.Interfaces;
using Agero.Core.Checker;
using System;
using System.DirectoryServices;
using System.Runtime.InteropServices;

namespace Agero.Core.Api.Security.Services.Implementations
{
    /// <summary>Validates username/password in active directory</summary>
    public class AgeroActiveDirectoryUsernamePasswordValidator : IUsernamePasswordValidator
    {
        /// <summary>Constructor</summary>
        /// <param name="activeDirectoryPath">Active directory path</param>
        /// <param name="userDomainName">Active directory user's domain name</param>
        public AgeroActiveDirectoryUsernamePasswordValidator(string activeDirectoryPath = "LDAP://DC=CORPPVT,DC=com", string userDomainName = "CORPPVT")
        {
            Check.ArgumentIsNullOrWhiteSpace(activeDirectoryPath, "activeDirectoryPath");
            Check.ArgumentIsNull(userDomainName, "userDomainName");

            ActiveDirectoryPath = activeDirectoryPath;
            UserDomainName = userDomainName;
        }

        /// <summary>Active directory path</summary>
        public string ActiveDirectoryPath { get; }

        /// <summary>Active directory user's domain name</summary>
        public string UserDomainName { get; }

        /// <summary>Validates username/password</summary>
        /// <exception cref="ArgumentException">When username is invalid</exception>
        /// <exception cref="ArgumentException">When password is invalid</exception>
        /// <returns>Returns true if username/password can be found, otherwise false</returns>
        public bool Validate(string username, string password)
        {
            Check.ArgumentIsNullOrWhiteSpace(username, "username");
            Check.Argument(username.Length <= 50, "username.Length <= 50");
            Check.ArgumentIsNullOrWhiteSpace(password, "password");
            Check.Argument(password.Length <= 50, "password.Length <= 50");

            var entry = new DirectoryEntry(ActiveDirectoryPath, $@"{UserDomainName}\{username}", password);

            var searcher = new DirectorySearcher(entry) {Filter = "(SAMAccountName=" + username + ")"};
            searcher.PropertiesToLoad.Add("cn");

            try
            {
                return searcher.FindOne() != null;
            }
            catch (DirectoryServicesCOMException ex) when (ex.Message.Contains("The user name or password is incorrect"))
            {
                return false;
            }
            catch (COMException ex) when (ex.Message.Contains("Logon failure:"))
            {
                return false;
            }
        }
    }
}
