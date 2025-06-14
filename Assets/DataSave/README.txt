# Quick Setup Guide:
1. Drag the SetupWizard script onto any gameobject in your scene
2. Configure the settings via the SetupWizard to match your security & performance needs
3. Optionally autogenerate a saveslot - This will remove the need for manual saveslot management

# Usage Guide:

## Saving / Loading:
- DataManager.Save(string key, object value) // saves the value with the 'key' as a unique identifier
- DataManager.Load<T>(string key) // loads the value saved with the key
- await DataManager.SaveAsync(string key, object value) // For large data - avoids UI freezes
- await DataManager.LoadAsync<T>(string key) // For large data - avoids UI freezes

## Saveslot Management:
- DataManager.CreateSaveSlot(string name) // Creates a new save slot
- DataManager.DeleteSaveSlot(string name) // Deletes the save slot with the given name
- DataManager.RenameSaveSlot(string oldName, string newName) // Renames the slot
- DataManager.SetActiveSlot(string name) // Sets which slot you save to and load from
- DataManager.DeleteAllSaveSlots() // Deletes all save slots and files

# Examples:
- Multi-saveslot example: Examples/Clicker
- Single-saveslot example: Examples/Spawning

# Features:
- AES encryption & GZIP compression
- 18+ Unity type converters (Vector3, Quaternion, etc.)
- Cross-platform support
- Async operations for performance