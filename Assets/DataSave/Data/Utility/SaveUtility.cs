using System;
using System.Text;

namespace SaveLoadSystem
{
    [Serializable]
    public class SaveMetadata
    {
        public Compression compression;
        public Encryption encryption;
        public DateTime saveTime;
    }

    public static class SaveUtility
    {
        public static string ProcessLoadData(byte[] data, byte[] encryptionKey, SaveMetadata metaData)
        {
            if (metaData.encryption == Encryption.AES)
            {
                data = EncryptionUtility.Decrypt(data, encryptionKey);
            }

            if (metaData.compression == Compression.GZIP)
            {
                string json = GZIP.Decompress(data);
                return json;
            }

            return UTF8Encoding.UTF8.GetString(data);
        }

        public static byte[] ProcessSaveData(string json, byte[] encryptionKey, Compression comp, Encryption enc)
        {
            byte[] data;

            // Compress
            if (comp == Compression.GZIP)
            {
                data = GZIP.Compress(json);
            }
            else
            {
                data = UTF8Encoding.UTF8.GetBytes(json);
            }

            // Encrypt
            if (enc == Encryption.AES)
            {
                data = EncryptionUtility.Encrypt(data, encryptionKey);
            }

            return data;
        }
    }
}

