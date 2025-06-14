using UnityEngine;


namespace SaveLoadSystem
{
    [CreateAssetMenu(fileName = "SaveSystemConfig", menuName = "Save System/Config")]
    public class SaveSettings : ScriptableObject
    {
        public Compression compressionType = Compression.GZIP;
        public Encryption encryptionType = Encryption.AES;
        public MessageType logLevel = MessageType.Info;
    }
}
