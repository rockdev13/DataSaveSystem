using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SaveLoadSystem
{
    [Serializable]
    public class SaveSlot
    {
        #region Variables
        public string Name;
        public string SavePath;
        public Compression CompressionType;
        public Encryption EncryptionType;

        private byte[] _encryptionKey;
        private string _keyPath;
        private string _metadataPath;

        private readonly SemaphoreSlim _fileAccessSemaphore = new SemaphoreSlim(1, 1);
        private readonly DataCacheManager _dataCacheManager;

        #endregion

        #region Initialization
        public SaveSlot(string name, Compression compressionType, Encryption encryptionType)
        {
            Name = name;
            CompressionType = compressionType;
            EncryptionType = encryptionType;
            _dataCacheManager = new DataCacheManager();

            SavePath = Path.Combine(Application.persistentDataPath, "Saveslots", name + ".saveslot");
            _keyPath = Path.Combine(Application.persistentDataPath, "Saveslots", name + "_KEY.key");
            _metadataPath = Path.Combine(Application.persistentDataPath, "Saveslots", name + "_METADATA.metadata");

            InitializeEncryptionKey();
            InitializeSaveFile();
        }

        private void InitializeEncryptionKey()
        {
            if (File.Exists(_keyPath))
            {
                _encryptionKey = File.ReadAllBytes(_keyPath);
            }
            else
            {
                _encryptionKey = EncryptionUtility.GenerateKey();
                string keyDirectory = Path.GetDirectoryName(_keyPath);
                if (!Directory.Exists(keyDirectory))
                {
                    Directory.CreateDirectory(keyDirectory);
                }
                File.WriteAllBytes(_keyPath, _encryptionKey);
            }
        }

        private void InitializeSaveFile()
        {
            if (File.Exists(SavePath))
            {
                // Load existing data into cache
                string json = SaveUtility.ProcessLoadData(File.ReadAllBytes(SavePath), _encryptionKey, LoadMetadata());
                _dataCacheManager.FromJson(json);

                return;
            }

            string directory = Path.GetDirectoryName(SavePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string emptyJson = "{}";
            byte[] data = SaveUtility.ProcessSaveData(emptyJson, _encryptionKey, CompressionType, EncryptionType);
            File.WriteAllBytes(SavePath, data);

            _dataCacheManager.FromJson(emptyJson);
        }
        #endregion

        #region Load Methods
        public T Load<T>(string key)
        {
            _fileAccessSemaphore.Wait();
            try
            {
                return _dataCacheManager.GetValue<T>(key);
            }
            finally
            {
                _fileAccessSemaphore.Release();
                CustomLogger.Log($"Loaded data from key: {key}");
            }
        }
        #endregion

        #region Save Methods
        public void Save(string key, object value)
        {
            _fileAccessSemaphore.Wait();

            try
            {
                // Update cache
                _dataCacheManager.SetValue(key, value);

                // Write data from new cache to file
                string newJson = _dataCacheManager.ToJson();
                byte[] finalData = SaveUtility.ProcessSaveData(newJson, _encryptionKey, CompressionType, EncryptionType);
                File.WriteAllBytes(SavePath, finalData);

                // Save metadata
                SaveMetadata metaData = new SaveMetadata
                {
                    saveTime = DateTime.Now,
                    compression = CompressionType,
                    encryption = EncryptionType
                };

                SaveMetadata(metaData);
            }
            finally
            {
                _fileAccessSemaphore.Release();
                CustomLogger.Log($"Saved value: {JsonConvert.SerializeObject(value, JsonConverters.JsonSettings)} with key: {key}");
            }
        }

        public async Task SaveAsync(string key, object value)
        {
            await _fileAccessSemaphore.WaitAsync();

            try
            {
                // Update cache 
                _dataCacheManager.SetValue(key, value);

                // Write to file
                string newJson = JsonConvert.SerializeObject(_dataCacheManager.Cache, JsonConverters.JsonSettings);
                byte[] finalData = await Task.Run(() => 
                    SaveUtility.ProcessSaveData(newJson, _encryptionKey, CompressionType, EncryptionType));
                await File.WriteAllBytesAsync(SavePath, finalData);

                // Save metadata
                SaveMetadata metaData = new SaveMetadata
                {
                    saveTime = DateTime.Now,
                    compression = CompressionType,
                    encryption = EncryptionType
                };

                await SaveMetadataAsync(metaData);
            }
            finally {
                _fileAccessSemaphore.Release();
                CustomLogger.Log($"Saved value: {JsonConvert.SerializeObject(value)} with key: {key}");
            }
        }
        private void SaveCacheToFile()
        {
            string newJson = JsonConvert.SerializeObject(_dataCacheManager.Cache, JsonConverters.JsonSettings);
            byte[] finalData = SaveUtility.ProcessSaveData(newJson, _encryptionKey, CompressionType, EncryptionType);
            File.WriteAllBytes(SavePath, finalData);
        }
        private async Task SaveCacheToFileAsync()
        {
            string newJson = JsonConvert.SerializeObject(_dataCacheManager.Cache, JsonConverters.JsonSettings);
            byte[] finalData = SaveUtility.ProcessSaveData(newJson, _encryptionKey, CompressionType, EncryptionType);
            await File.WriteAllBytesAsync(SavePath, finalData);
        }
        #endregion

        #region Delete Methods
        public void DeleteKey(string key)
        {
            _fileAccessSemaphore.Wait();

            try
            {
                _dataCacheManager.RemoveValue(key);
                SaveCacheToFile();
            }
            finally
            {
                _fileAccessSemaphore.Release();
                CustomLogger.Log($"Deleted key: {key}");
            }
        }

        public async Task DeleteKeyAsync(string key)
        {
            await _fileAccessSemaphore.WaitAsync();
            try
            {
                _dataCacheManager.RemoveValue(key);
                await SaveCacheToFileAsync();
            }
            finally
            {
                _fileAccessSemaphore.Release();
                CustomLogger.Log($"Deleted key: {key}");
            }
        }

        public void ClearFiles()
        {
            if (File.Exists(SavePath)) File.Delete(SavePath);
            if (File.Exists(_keyPath)) File.Delete(_keyPath);
            if (File.Exists(_metadataPath)) File.Delete(_metadataPath);
        }
        #endregion

        #region Metadata
        private void SaveMetadata(SaveMetadata metadata)
        {
            string metadataJson = JsonConvert.SerializeObject(metadata, JsonConverters.JsonSettings);
            byte[] metadataData = SaveUtility.ProcessSaveData(metadataJson, _encryptionKey, Compression.GZIP, Encryption.AES);

            string metadataDirectory = Path.GetDirectoryName(_metadataPath);
            if (!Directory.Exists(metadataDirectory))
            {
                Directory.CreateDirectory(metadataDirectory);
            }

            File.WriteAllBytes(_metadataPath, metadataData);
        }

        private async Task SaveMetadataAsync(SaveMetadata metadata)
        {
            string metadataJson = JsonConvert.SerializeObject(metadata, JsonConverters.JsonSettings);
            byte[] metadataData = SaveUtility.ProcessSaveData(metadataJson, _encryptionKey, Compression.GZIP, Encryption.AES);

            string metadataDirectory = Path.GetDirectoryName(_metadataPath);
            if (!Directory.Exists(metadataDirectory))
            {
                Directory.CreateDirectory(metadataDirectory);
            }

            await File.WriteAllBytesAsync(_metadataPath, metadataData);
        }

        public SaveMetadata LoadMetadata()
        {
            if (!File.Exists(_metadataPath))
            {
                return new SaveMetadata
                {
                    saveTime = DateTime.MinValue,
                    compression = CompressionType,
                    encryption = EncryptionType
                };
            }

            byte[] rawMetadata = File.ReadAllBytes(_metadataPath);
            byte[] decrypted = EncryptionUtility.Decrypt(rawMetadata, _encryptionKey);
            string decompressed = GZIP.Decompress(decrypted);

            return JsonConvert.DeserializeObject<SaveMetadata>(decompressed, JsonConverters.JsonSettings);
        }

        public bool Rename(string newName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newName))
                {
                    CustomLogger.LogWarning("You can't rename a save slot to an empty name");
                    return false;
                }

                string directory = Path.GetDirectoryName(SavePath);
                string extension = Path.GetExtension(SavePath);
                string newPath = Path.Combine(directory, newName + extension);
                string newKeyPath = Path.Combine(directory, newName + "_KEY.key");
                string newMetadataPath = Path.Combine(directory, newName + "_METADATA.metadata");

                if (File.Exists(newPath) && newPath != SavePath)
                {
                    CustomLogger.LogWarning($"Cannot rename: A save slot named '{newName}' already exists");
                    return false;
                }

                if (File.Exists(SavePath) && newPath != SavePath)
                {
                    File.Move(SavePath, newPath);
                }

                if (EncryptionType != Encryption.None && File.Exists(_keyPath) && newKeyPath != _keyPath)
                {
                    File.Move(_keyPath, newKeyPath);
                }

                if (File.Exists(_metadataPath) && newMetadataPath != _metadataPath)
                {
                    File.Move(_metadataPath, newMetadataPath);
                }

                Name = newName;
                SavePath = newPath;
                _keyPath = newKeyPath;
                _metadataPath = newMetadataPath;
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to rename save slot: {ex.Message}");
                return false;
            }
        }

        public DateTime GetLastModified()
        {
            try
            {
                if (!File.Exists(SavePath)) return default;
                return File.GetLastWriteTime(SavePath);
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to get last modified time: {ex.Message}");
                return default;
            }
        }

        public long GetFileSize()
        {
            try
            {
                if (!File.Exists(SavePath)) return 0;
                return new FileInfo(SavePath).Length;
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to get file size: {ex.Message}");
                return 0;
            }
        }

        #endregion

        public Dictionary<string, object> GetCacheData()
        {
            return _dataCacheManager.Cache;
        }
        
        public void Dispose()
        {
            _fileAccessSemaphore?.Dispose();
        }
    }
}