using System;
using System.Security.Cryptography;
using System.IO;

namespace SaveLoadSystem {
    /// <summary>
    /// Includes functionality to encrypt and decrypt data
    /// </summary>
    public static class EncryptionUtility {
        // Encrypt data
        public static byte[] Encrypt(byte[] data, byte[] key) {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV(); // Generate new IV for each encryption

            using var encryptor = aes.CreateEncryptor();
            var encrypted = PerformCryptography(data, encryptor);

            // Prepend IV to encrypted data
            var result = new byte[ aes.IV.Length + encrypted.Length ];
            Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
            Array.Copy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

            return result;
        }

        // Decrypt data
        public static byte[] Decrypt(byte[] encryptedDataWithIV, byte[] key) {
            using var aes = Aes.Create();
            aes.Key = key;

            // Extract IV
            var ivLength = aes.BlockSize / 8;
            var iv = new byte[ ivLength ];
            var encryptedData = new byte[ encryptedDataWithIV.Length - ivLength ];

            Array.Copy(encryptedDataWithIV, 0, iv, 0, ivLength);
            Array.Copy(encryptedDataWithIV, ivLength, encryptedData, 0, encryptedData.Length);

            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor();
            return PerformCryptography(encryptedData, decryptor);
        }

        // Generate a random key
        public static byte[] GenerateKey() {
            using var aes = Aes.Create();
            aes.GenerateKey();
            return aes.Key;
        }

        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform) {
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }
    }
}
