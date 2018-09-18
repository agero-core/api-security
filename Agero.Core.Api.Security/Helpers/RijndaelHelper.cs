using Agero.Core.Checker;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Agero.Core.Api.Security.Helpers
{
    /// <summary>Rijndael (256 bit) helper</summary>
    public static class RijndaelHelper
    {
        /// <summary>Generates key and initialization vector</summary>
        public static Tuple<byte[], byte[]> GenerateKeyAndIV()
        {
            using (var rijndael = new RijndaelManaged { KeySize = 256 })
            {
                rijndael.GenerateKey();
                rijndael.GenerateIV();

                return Tuple.Create(rijndael.Key, rijndael.IV);
            }
        }

        /// <summary>Encypts text using key and initialization vector</summary>
        public static byte[] Encrypt(string text, byte[] key, byte[] iv)
        {
            Check.ArgumentIsNullOrWhiteSpace(text, "text");
            Check.Argument(key != null && key.Length > 0, "key != null && key.Length > 0");
            Check.Argument(iv != null && iv.Length > 0, "iv != null && iv.Length > 0");

            // ReSharper disable once AssignNullToNotNullAttribute
            using (var rijndael = new RijndaelManaged { KeySize = 256, Key = key, IV = iv })
            {
                var encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(text);
                        }

                        return memoryStream.ToArray();
                    }
                }
            }
        }

        /// <summary>Decrypts text using key and initialization vector</summary>
        public static string Decrypt(byte[] encryptedText, byte[] key, byte[] iv)
        {
            Check.Argument(encryptedText != null && encryptedText.Length > 0, "encryptedText != null && encryptedText.Length > 0");
            Check.Argument(key != null && key.Length > 0, "key != null && key.Length > 0");
            Check.Argument(iv != null && iv.Length > 0, "iv != null && iv.Length > 0");

            using (var rijndael = new RijndaelManaged { KeySize = 256, Key = key, IV = iv })
            {
                var decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

                // ReSharper disable once AssignNullToNotNullAttribute
                using (var memoryStream = new MemoryStream(encryptedText))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
