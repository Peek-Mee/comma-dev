using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Comma.Utility.Collections
{
    public class SimpleEncryption
    {
        private static string _key = "PeekMee_Comma2022";
        private static byte[] _salt = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        /// <summary>
        /// Convert clean string into encrypted string.
        /// </summary>
        /// <param name="text">Clean string</param>
        /// <returns>Cipher string</returns>
        public static string Encrypt(string text)
        {
            string clearText;
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new(_key, _salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new())
                {
                    using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        /// <summary>
        /// Conver encrypted string into clean string
        /// </summary>
        /// <param name="text">Cipher string</param>
        /// <returns>Clean string</returns>
        public static string Decrypt(string text)
        {
            string clearText = text;
            clearText = clearText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(clearText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_key, _salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new())
                {
                    using (CryptoStream cs = new(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    clearText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return clearText;
        }
    }
}
