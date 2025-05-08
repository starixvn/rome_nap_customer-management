using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Fanex.NAP.SignIn
{
    public static class Cryptography
    {
#pragma warning disable S2278 // Neither DES (Data Encryption Standard) nor DESede (3DES) should be used
        private static readonly DESCryptoServiceProvider CryptoServiceProvider = new DESCryptoServiceProvider();
#pragma warning restore S2278 // Neither DES (Data Encryption 
        private static readonly byte[] RgbKey = new byte[] { 0x21, 0x63, 0x54, 0x5a, 0x91, 0xac, 110, 0xcc };
        private static readonly byte[] RgbIv = Encoding.UTF8.GetBytes("&％#@?,:*!~^.");

        public static string DecryptPassword(string encryptedPassword)
        {
            if (encryptedPassword == null)
            {
                throw new ArgumentNullException(nameof(encryptedPassword));
            }

            using (var decryptor = CryptoServiceProvider.CreateDecryptor(RgbKey, RgbIv))
            {
                var standardizedEncryptedPassword = StandardizeEncryptedPasswordLength(encryptedPassword);
                var encryptedPasswordInBytes = Convert.FromBase64String(standardizedEncryptedPassword);

                using (var memoryStream = new MemoryStream(encryptedPasswordInBytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd().Trim();
                        }
                    }
                }
            }
        }

        private static string StandardizeEncryptedPasswordLength(string encryptedPassword)
        {
            if (string.IsNullOrWhiteSpace(encryptedPassword))
            {
                return string.Empty;
            }

            var allowedLength = encryptedPassword.Length >> 2 << 2;
            return encryptedPassword.Substring(0, allowedLength);
        }

        public static string Encrypt(string plainText)
        {
            if (plainText == null)
            {
                throw new ArgumentNullException(nameof(plainText));
            }

            using (var aes = new AesCryptoServiceProvider())
            {
                using (var encryptor = aes.CreateEncryptor(RgbKey, RgbIv))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                            return Convert.ToBase64String(msEncrypt.ToArray());
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            if (cipherText == null)
            {
                throw new ArgumentNullException(nameof(cipherText));
            }

            using (var aes = new AesCryptoServiceProvider())
            {
                using (var decryptor = aes.CreateDecryptor(RgbKey, RgbIv))
                {
                    var cipherBytes = Convert.FromBase64String(cipherText);
                    using (var msDecrypt = new MemoryStream(cipherBytes))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}