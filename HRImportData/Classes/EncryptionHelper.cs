using System.Security.Cryptography;
using System.Text;

namespace HRImportData.Classes
{
    internal static class EncryptionHelper
    {
        private const string SecurityKey = "2023@5NEY!!@!#!";
        private static readonly int KeySize = 32; // 256-bit key (32 bytes)
        private static readonly int BlockSize = 16; // AES block size (16 bytes)

        public static string GetMD5Base64Hash(string strToHash)
        {
            byte[] byteStr = Encoding.UTF8.GetBytes(strToHash);
            byte[] hashVal = (new System.Security.Cryptography.MD5CryptoServiceProvider()).ComputeHash(byteStr);
            string base64Hash = Convert.ToBase64String(hashVal);
            return base64Hash.TrimEnd('=');
        }

        public static string Encrypt(string plainText, bool useHashing)
        {
            byte[] keyArray = GetKey(SecurityKey, useHashing);
            byte[] ivArray = GenerateRandomIV();
            byte[] encryptedBytes;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyArray;
                aesAlg.IV = ivArray;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
            }

            // Combine IV + Encrypted Data
            byte[] combinedData = new byte[ivArray.Length + encryptedBytes.Length];
            Array.Copy(ivArray, 0, combinedData, 0, ivArray.Length);
            Array.Copy(encryptedBytes, 0, combinedData, ivArray.Length, encryptedBytes.Length);

            return Convert.ToBase64String(combinedData);
        }


        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray = GetKey(SecurityKey, useHashing);
            byte[] combinedData = Convert.FromBase64String(cipherString);

            byte[] ivArray = new byte[BlockSize];
            byte[] encryptedBytes = new byte[combinedData.Length - BlockSize];

            Array.Copy(combinedData, 0, ivArray, 0, BlockSize);
            Array.Copy(combinedData, BlockSize, encryptedBytes, 0, encryptedBytes.Length);

            string decryptedText;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyArray;
                aesAlg.IV = ivArray;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                }
            }

            return decryptedText;
        }

        private static byte[] GetKey(string securityKey, bool useHashing)
        {
            if (useHashing)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    return sha256.ComputeHash(Encoding.UTF8.GetBytes(securityKey));
                }
            }
            else
            {
                return Encoding.UTF8.GetBytes(securityKey);
            }
        }

        private static byte[] GenerateRandomIV()
        {
            byte[] iv = new byte[BlockSize];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }
    }
}
