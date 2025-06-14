using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaveLoadSystem
{
    /// <summary>
    /// API for the asset
    /// </summary>
    public static class DataManager
    {
        private static SaveSlot ActiveSaveSlot => SaveSlotManager.ActiveSaveSlot;

        public static Compression CompressionType;
        public static Encryption EncryptionType;

        /// <summary>
        /// Saves a value with a key (synchronous)
        /// </summary>
        /// <param name="key">The key to access the saved data</param>
        /// <param name="value">The value to save</param>
        public static void Save(string key, object value)
        {
            if (ActiveSaveSlot == null) return;

            ActiveSaveSlot.Save(key, value);
        }

        /// <summary>
        /// Saves a value with a key (asynchronous)
        /// </summary>
        /// <param name="key">The key to access the saved data</param>
        /// <param name="value">The value to save</param>
        public static async Task SaveAsync(string key, object value)
        {
            if (ActiveSaveSlot == null) return;

            await ActiveSaveSlot.SaveAsync(key, value);
        }

        /// <summary>
        /// Loads a value from a given key (synchronous)
        /// </summary>
        /// <typeparam name="T">Type of the value to load</typeparam>
        /// <param name="key">The key to access the saved data</param>
        /// <returns>The loaded value or default if not found</returns>
        public static T Load<T>(string key)
        {
            if (ActiveSaveSlot == null) return default;

            return ActiveSaveSlot.Load<T>(key);
        }

        /// <summary>
        /// Loads a value from a given key (asynchronous)
        /// </summary>
        /// <typeparam name="T">Type of the value to load</typeparam>
        /// <param name="key">The key to access the saved data</param>
        /// <returns>The loaded value or default if not found</returns>
        public static async Task<T> LoadAsync<T>(string key)
        {
            if (ActiveSaveSlot == null) return default;

            return await ActiveSaveSlot.LoadAsync<T>(key);
        }

        // Core functionality
        public static void CreateSaveSlot(string name) => SaveSlotManager.CreateSaveSlot(name);
        public static void DeleteSaveSlot(string name) => SaveSlotManager.DeleteSaveSlot(name);
        public static void RenameSaveSlot(string oldName, string newName) => SaveSlotManager.RenameSaveSlot(oldName, newName);
        public static void SetActiveSlot(string name) => SaveSlotManager.ChangeActiveSaveSlot(name);
        public static void DeleteAllSaveSlots() => SaveSlotManager.DeleteAllSaveSlots();

        // Helper methods
        public static List<SaveSlot> GetSaveSlots() => SaveSlotManager._saveSlots;
        public static bool DoesSlotExist(string name) => SaveSlotManager.DoesSlotExist(name);
    }
}