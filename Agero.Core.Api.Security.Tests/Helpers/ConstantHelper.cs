using System;

namespace Agero.Core.Api.Security.Tests.Helpers
{
    public static class ConstantHelper
    {
        public static class Username
        {
            public const string ExistingAndAuthorized = "ExistingAndAuthorizedUsername";
            public const string NonExisting = "NonExistingUsername";
            public const string NonAuthorized = "NonAuthorizedUsername";
        }

        public const string Password = "Password";
        public const int ExpireInMinutes = 60;

        public static class AccessToken
        {
            public const string Valid = "ValidAccessToken";
            public const string Invalid = "InvalidAccessToken";
        }

        public static class AccessTokenData
        {
            public static readonly Services.Interfaces.AccessTokenData AuthorizedUserAndNotExpired = new Services.Interfaces.AccessTokenData(Username.ExistingAndAuthorized, DateTime.UtcNow.AddHours(1));
            public static readonly Services.Interfaces.AccessTokenData Expired = new Services.Interfaces.AccessTokenData(Username.ExistingAndAuthorized, DateTime.UtcNow.AddHours(-1));
            public static readonly Services.Interfaces.AccessTokenData NonAuthorizedUser = new Services.Interfaces.AccessTokenData(Username.NonAuthorized, DateTime.UtcNow.AddHours(1));
        }

        public const string ResourceId = "/api/someapi";
    }
}
