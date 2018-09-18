using Agero.Core.Checker;
using System;

namespace Agero.Core.Api.Security.Helpers
{
    /// <summary>Base64 helper</summary>
    public static class Base64Helper
    {
        /// <summary>Encodes to Base64 string</summary>
        public static string Encode(byte[] data)
        {
            Check.ArgumentIsNull(data, "data");
            Check.Argument(data.Length > 0, "data.Length > 0");

            return Convert.ToBase64String(data);
        }

        /// <summary>Decodes from Base64 string</summary>
        public static byte[] Decode(string data)
        {
            Check.ArgumentIsNullOrEmpty(data, "data");

            return Convert.FromBase64String(data);
        }
    }
}
