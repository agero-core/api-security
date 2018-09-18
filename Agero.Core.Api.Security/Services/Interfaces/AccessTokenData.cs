using System;
using System.Runtime.Serialization;

namespace Agero.Core.Api.Security.Services.Interfaces
{
    /// <summary>Data associated with access token</summary>
    [DataContract]
    public class AccessTokenData
    {
        /// <summary>Constructor</summary>
        public AccessTokenData(string username, DateTime expirationUtcTime)
        {
            ExpirationUtcTime = expirationUtcTime;
            Username = username;
        }

        /// <summary>Username associated with access token</summary>
        [DataMember(Name = "username")]
        public string Username { get; }

        /// <summary>Access token expiration UTC time</summary>
        [DataMember(Name = "expirationUtcTime")]
        public DateTime ExpirationUtcTime { get; }
    }
}
