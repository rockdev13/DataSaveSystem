using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;
using System.Text;

namespace SaveLoadSystem
{
    [Serializable]
    public class SaveSlot
    {
        public string Name;
        public string SavePath;
        public Compression CompressionType;
        public Encryption EncryptionType;

        private byte[] _encryptionKey;
        private string _keyPath;
        private string _metadataPath;

        // Thread-safety: Only one save/load operation at a time per slot
        private readonly SemaphoreSlim _fileAccessSemaphore = new SemaphoreSlim(1, 1);
        
        // In-memory cache to avoid file reads, improves load time :)
        private Dictionary<string, object> _dataCache;
        private bool _cacheLoaded = false;
        private readonly object _cacheLock = new object();
        public string DataCache;

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter>
            {
                new UnityJsonConverters.Vector2Converter(),
                new UnityJsonConverters.Vector3Converter(),
                new UnityJsonConverters.Vector4Converter(),
                new UnityJsonConverters.Vector2IntConverter(),
                new UnityJsonConverters.Vector3IntConverter(),
                new UnityJsonConverters.QuaternionConverter(),
                new UnityJsonConverters.ColorConverter(),
                new UnityJsonConverters.Color32Converter(),
                new UnityJsonConverters.RectConverter(),
                new UnityJsonConverters.RectIntConverter(),
                new UnityJsonConverters.BoundsConverter(),
                new UnityJsonConverters.BoundsIntConverter(),
                new UnityJsonConverters.Matrix4x4Converter(),
                new UnityJsonConverters.RayConverter(),
                new UnityJsonConverters.Ray2DConverter(),
                new UnityJsonConverters.PlaneConverter(),
                new UnityJsonConverters.AnimationCurveConverter(),
                new UnityJsonConverters.GradientConverter()
            }
        };

        public SaveSlot(string name, Compression compressionType, Encryption encryptionType)
        {
            Name = name;
            SavePath = Path.Combine(Application.persistentDataPath, "Saveslots", name + ".saveslot");
            CompressionType = compressionType;
            EncryptionType = encryptionType;

            _keyPath = Path.Combine(Application.persistentDataPath, "Saveslots", name + "_KEY.key");
            _metadataPath = Path.Combine(Application.persistentDataPath, "Saveslots", name + "_METADATA.metadata");

            InitializeEncryptionKey();

            if (!File.Exists(SavePath))
            {
                CreateEmptyInitialSaveFile();
            }

            // Load cache on initialization
            LoadCacheFromFile();
        }

        private void LoadCacheFromFile()
        {
            try
            {
                if (!File.Exists(SavePath))
                {
                    _dataCache = new Dictionary<string, object>();
                    _cacheLoaded = true;
                    return;
                }

                byte[] rawData = File.ReadAllBytes(SavePath);
                string json = SaveUtility.ProcessLoadData(rawData, _encryptionKey, LoadMetadata());
                _dataCache = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, JsonSettings)
                           ?? new Dictionary<string, object>();
                _cacheLoaded = true;

                DataCache = JsonConvert.SerializeObject(_dataCache);
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to load cache: {ex.Message}");
                _dataCache = new Dictionary<string, object>();
                _cacheLoaded = true;
            }
        }

        private void InitializeEncryptionKey()
        {
            try
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
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to initialize encryption key: {ex.Message}");
                throw;
            }
        }

        private void CreateEmptyInitialSaveFile()
        {
            try
            {
                string directory = Path.GetDirectoryName(SavePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string emptyJson = "{}";
                byte[] data = SaveUtility.ProcessSaveData(emptyJson, _encryptionKey, CompressionType, EncryptionType);
                File.WriteAllBytes(SavePath, data);
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to create initial save file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Loads a value from a given key (synchronous, uses cache)
        /// </summary>
        public T Load<T>(string key)
        {
            lock (_cacheLock)
            {
                if (!_cacheLoaded)
                {
                    LoadCacheFromFile();
                }

                if (!_dataCache.ContainsKey(key))
                {
                    return default(T);
                }

                try
                {
                    return JsonConvert.DeserializeObject<T>(_dataCache[key].ToString(), JsonSettings);
                }
                catch (Exception ex)
                {
                    CustomLogger.LogError($"Failed to deserialize data for key '{key}': {ex.Message}");
                    return default(T);
                }
            }
        }

        /// <summary>
        /// Loads a value from a given key (asynchronous, uses cache)
        /// </summary>
        public async Task<T> LoadAsync<T>(string key)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // WebGL: Just call sync version directly
            return Load<T>(key);
#else
            // Other platforms: Use Task.Run for true async
            return await Task.Run(() => Load<T>(key));
#endif
        }

        /// <summary>
        /// Saves a value with a key (synchronous, thread-safe)
        /// </summary>
        public void Save(string key, object value)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // WebGL: Skip semaphore (it's a single threaded environment)
            try
            {
                // Update cache
                lock (_cacheLock)
                {
                    if (!_cacheLoaded)
                    {
                        LoadCacheFromFile();
                    }
                    _dataCache[key] = value;
                }

                // Write data to file (synchronous)
                string newJson = JsonConvert.SerializeObject(_dataCache, JsonSettings);
                byte[] finalData = SaveUtility.ProcessSaveData(newJson, _encryptionKey, CompressionType, EncryptionType);
                File.WriteAllBytes(SavePath, finalData);

                // Save metadata (synchronous)
                SaveMetadata metaData = new SaveMetadata
                {
                    saveTime = DateTime.Now,
                    compression = CompressionType,
                    encryption = EncryptionType
                };

                SaveMetadata(metaData);
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to save data: {ex.Message}");
            }
#else
            // Other platforms: Use semaphore for thread safety
            _fileAccessSemaphore.Wait();
            try
            {
                // Update cache first
                lock (_cacheLock)
                {
                    if (!_cacheLoaded)
                    {
                        LoadCacheFromFile();
                    }
                    _dataCache[key] = value;
                }

                // Write data to file
                string newJson = JsonConvert.SerializeObject(_dataCache, JsonSettings);
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
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to save data: {ex.Message}");
            }
            finally
            {
                _fileAccessSemaphore.Release();
            }
#endif
        }

        private void SaveMetadata(SaveMetadata metadata)
        {
            try
            {
                string metadataJson = JsonConvert.SerializeObject(metadata, JsonSettings);
                byte[] metadataData = SaveUtility.ProcessSaveData(metadataJson, _encryptionKey, Compression.GZIP, Encryption.AES);

                string metadataDirectory = Path.GetDirectoryName(_metadataPath);
                if (!Directory.Exists(metadataDirectory))
                {
                    Directory.CreateDirectory(metadataDirectory);
                }

                File.WriteAllBytes(_metadataPath, metadataData);
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to save metadata: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves a value with a key (asynchronous, thread-safe)
        /// </summary>
        public async Task SaveAsync(string key, object value)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // WebGL: Call sync version directly (no threading support)
            Save(key, value);
            return;
#else
            // Other platforms: Use proper async operations
            await _fileAccessSemaphore.WaitAsync();
            try
            {
                // Update cache first
                lock (_cacheLock)
                {
                    if (!_cacheLoaded)
                    {
                        LoadCacheFromFile();
                    }
                    _dataCache[key] = value;
                }

                // Write to file
                string newJson = JsonConvert.SerializeObject(_dataCache, JsonSettings);
                byte[] finalData = SaveUtility.ProcessSaveData(newJson, _encryptionKey, CompressionType, EncryptionType);
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
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to save data: {ex.Message}");
            }
            finally
            {
                _fileAccessSemaphore.Release();
            }
#endif
        }

        private async Task SaveMetadataAsync(SaveMetadata metadata)
        {
            try
            {
                string metadataJson = JsonConvert.SerializeObject(metadata, JsonSettings);
                byte[] metadataData = SaveUtility.ProcessSaveData(metadataJson, _encryptionKey, Compression.GZIP, Encryption.AES);

                string metadataDirectory = Path.GetDirectoryName(_metadataPath);
                if (!Directory.Exists(metadataDirectory))
                {
                    Directory.CreateDirectory(metadataDirectory);
                }

                await File.WriteAllBytesAsync(_metadataPath, metadataData);
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to save metadata: {ex.Message}");
            }
        }

        /// <summary>
        /// Forces a flush of cached data to disk
        /// </summary>
        public async Task FlushAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // WebGL: Use synchronous flush
            try
            {
                if (_cacheLoaded && _dataCache != null)
                {
                    string json = JsonConvert.SerializeObject(_dataCache, JsonSettings);
                    byte[] data = SaveUtility.ProcessSaveData(json, _encryptionKey, CompressionType, EncryptionType);
                    File.WriteAllBytes(SavePath, data);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to flush data: {ex.Message}");
            }
#else
            // Other platforms: Use async flush
            await _fileAccessSemaphore.WaitAsync();
            try
            {
                if (_cacheLoaded && _dataCache != null)
                {
                    string json = JsonConvert.SerializeObject(_dataCache, JsonSettings);
                    byte[] data = SaveUtility.ProcessSaveData(json, _encryptionKey, CompressionType, EncryptionType);
                    await File.WriteAllBytesAsync(SavePath, data);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to flush data: {ex.Message}");
            }
            finally
            {
                _fileAccessSemaphore.Release();
            }
#endif
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

        public void ClearFiles()
        {
            if (File.Exists(SavePath)) File.Delete(SavePath);
            if (File.Exists(_keyPath)) File.Delete(_keyPath);
            if (File.Exists(_metadataPath)) File.Delete(_metadataPath);

            lock (_cacheLock)
            {
                _dataCache?.Clear();
            }
        }

        public SaveMetadata LoadMetadata()
        {
            try
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

                return JsonConvert.DeserializeObject<SaveMetadata>(decompressed, JsonSettings);
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to load metadata: {ex.Message}");
                return new SaveMetadata
                {
                    saveTime = DateTime.MinValue,
                    compression = CompressionType,
                    encryption = EncryptionType
                };
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

        public void Dispose()
        {
            _fileAccessSemaphore?.Dispose();
        }
    }
}