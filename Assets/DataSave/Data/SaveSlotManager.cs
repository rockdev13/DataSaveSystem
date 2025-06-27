using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SaveLoadSystem;
using UnityEngine;

namespace SaveLoadSystem
{
    public static class SaveSlotManager
    {
        internal readonly static List<SaveSlot> _saveSlots = new();
        internal static SaveSlot ActiveSaveSlot { get; private set; }

        private static readonly object _lock = new(); 
        
        internal static void Initialize()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Saveslots");

            // Ensure directory exists
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            // Get every save slot file
            string[] files = Directory.GetFiles(filePath);

            // Initialize each save slot
            foreach (string file in files)
            {
                if (Path.GetExtension(file).EndsWith("key") || Path.GetExtension(file).EndsWith(".metadata")) {
                    continue; // skip key and metadata files
                }

                try
                {
                    CreateSaveSlot(Path.GetFileNameWithoutExtension(file));
                }
                catch (Exception ex)
                {
                    CustomLogger.LogWarning($"Failed to load save slot {file}: {ex.Message}");
                    // Continue loading other slots
                }
            }

            ActiveSaveSlot ??= _saveSlots.FirstOrDefault();
        }

        internal static void ChangeActiveSaveSlot(string newSlotName)
        {
            SaveSlot targetSaveSlot = _saveSlots.FirstOrDefault(x => x.Name == newSlotName);
            ActiveSaveSlot = targetSaveSlot;
        }

        internal static void CreateSaveSlot(string newSlotName)
        {
            lock (_lock)
            {
                if (DataManager.DoesSlotExist(newSlotName))
                {
                    CustomLogger.LogWarning($"Name already exists: {newSlotName}");
                    return;
                }

                SaveSlot newSlot = new(newSlotName, DataManager.CompressionType, DataManager.EncryptionType);
                _saveSlots.Add(newSlot);
            }
        }

        internal static void DeleteSaveSlot(string newSlotName)
        {
            CustomLogger.LogInfo($"Deleting save slot: {newSlotName}");
            try
            {
                SaveSlot targetSaveSlot = _saveSlots.FirstOrDefault(x => x.Name == newSlotName);

                if (targetSaveSlot == null)
                {
                    CustomLogger.LogWarning($"Save slot '{newSlotName}' not found");
                    return;
                }

                targetSaveSlot.ClearFiles();
                targetSaveSlot.Dispose();

                _saveSlots.Remove(targetSaveSlot);
            }
            catch (IOException ex)
            {
                CustomLogger.LogError($"Failed to delete save slot '{newSlotName}': {ex.Message}");
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Unexpected error deleting save slot: {ex.Message}");
            }
        }

        internal static void RenameSaveSlot(string oldName, string newName)
        {
            CustomLogger.LogInfo($"Renaming save slot: {oldName} to {newName}");

            SaveSlot targetSlot = _saveSlots.FirstOrDefault(x => x.Name == oldName);

            if (targetSlot == null)
            {
                CustomLogger.LogError($"You can't rename a non-existent slot: {oldName} to {newName}");
                return;
            }

            targetSlot.Rename(newName);
        }

        internal static bool DoesSlotExist(string name)
        {
            return _saveSlots.Any(x => x.Name == name);
        }

        internal static void DeleteAllSaveSlots()
        {
            foreach (var slot in _saveSlots)
            {
                slot.ClearFiles();
            }

            _saveSlots.Clear();
        }
    }
}
