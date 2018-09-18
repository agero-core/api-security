using Agero.Core.Api.Security.Helpers;
using System;
using System.IO;

namespace Agero.Core.Api.Security.Generator
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                var keyAndIv = RijndaelHelper.GenerateKeyAndIV();

                Console.WriteLine("Generating Rijndael key...");
                var key = Base64Helper.Encode(keyAndIv.Item1);
                File.WriteAllText("key.txt", key);
                Console.WriteLine("Rijndael key generated (key.txt)");

                Console.WriteLine("Generating Rijndael initialization vector...");
                var iv = Base64Helper.Encode(keyAndIv.Item2);
                File.WriteAllText("iv.txt", iv);
                Console.WriteLine("Rijndael initialization vector generated (iv.txt)");

                Console.WriteLine("Generating completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.Read();
        }
    }
}
