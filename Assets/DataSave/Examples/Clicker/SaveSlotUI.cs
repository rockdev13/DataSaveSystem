using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SaveLoadSystem.Examples
{
    public class SaveSlotUI : MonoBehaviour
    {
        public static event Action OnGameStarted;

        [Header("UI References")]
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _slotsParent;
        [SerializeField] private Button _createSlotButton;

        [Header("Settings")]
        [SerializeField] private string _defaultSlotPrefix = "SaveSlot";
        [SerializeField] private int _maxSlots = 10;

        // Track UI slot instances to avoid memory leaks
        private readonly List<GameObject> _activeSlotUIs = new();

        private void Awake()
        {
            SetupCreateButton();
            RefreshUI();
        }

        private void OnDestroy()
        {
            CleanupEventListeners();
        }

        private void SetupCreateButton()
        {
            if (_createSlotButton != null)
            {
                _createSlotButton.onClick.RemoveAllListeners();
                _createSlotButton.onClick.AddListener(() => CreateNewSaveSlot());
            }
        }

        #region UI Management

        private void RefreshUI()
        {
            ClearAllSlotUIs();
            PopulateSlotUIs();
        }

        private void ClearAllSlotUIs()
        {
            // Clean up existing UI elements
            foreach (GameObject slotUI in _activeSlotUIs)
            {
                if (slotUI != null)
                {
                    DestroyImmediate(slotUI);
                }
            }
            _activeSlotUIs.Clear();

            // Failsafe: clear any remaining children
            for (int i = _slotsParent.childCount - 1; i >= 0; i--)
            {
                Transform child = _slotsParent.GetChild(i);
                if (child != null)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        private void PopulateSlotUIs()
        {
            var saveSlots = DataManager.GetSaveSlots();

            if (saveSlots == null || saveSlots.Count == 0)
            {
                CustomLogger.LogInfo("No save slots found. UI will be empty.");
                return;
            }

            foreach (var slot in saveSlots)
            {
                if (slot != null && !string.IsNullOrEmpty(slot.Name))
                {
                    CreateSlotUI(slot.Name);
                }
            }

            CustomLogger.LogInfo($"Created UI for {_activeSlotUIs.Count} save slots.");
        }

        #endregion

        #region Slot UI Creation

        private void CreateSlotUI(string slotName)
        {
            if (string.IsNullOrWhiteSpace(slotName))
            {
                CustomLogger.LogError("Cannot create UI for slot with empty name");
                return;
            }

            try
            {
                GameObject newSlotUI = Instantiate(_slotPrefab, _slotsParent);
                _activeSlotUIs.Add(newSlotUI);

                SetupSlotComponents(newSlotUI, slotName);
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to create slot UI for '{slotName}': {ex.Message}");
            }
        }

        private void SetupSlotComponents(GameObject slotUI, string slotName)
        {
            // Get components
            TMP_InputField nameField = slotUI.transform.Find("Name")?.GetComponent<TMP_InputField>();
            Button selectButton = slotUI.transform.Find("ChooseButton")?.GetComponent<Button>();
            Button deleteButton = slotUI.transform.Find("DeleteButton")?.GetComponent<Button>();

            // Validate components exist
            if (nameField == null)
            {
                CustomLogger.LogError($"Name input field not found in slot UI for '{slotName}'");
                return;
            }

            if (selectButton == null || deleteButton == null)
            {
                CustomLogger.LogError($"Required buttons not found in slot UI for '{slotName}'");
                return;
            }

            // Setup name field
            nameField.text = slotName;

            // Store original name for rename operations
            string originalName = slotName;

            nameField.onEndEdit.RemoveAllListeners();
            nameField.onEndEdit.AddListener((newName) => {
                if (HandleSlotRename(originalName, newName, nameField))
                {
                    originalName = newName; // Update stored name if successful
                }
            });

            // Setup select button
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() => SelectSaveSlot(nameField.text));

            // Setup delete button
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => ConfirmDeleteSlot(nameField.text));
        }

        #endregion

        #region Slot Operations

        public void CreateNewSaveSlot()
        {
            try
            {
                // Check slot limit
                if (DataManager.GetSaveSlots().Count >= _maxSlots)
                {
                    CustomLogger.LogWarning($"Maximum number of save slots ({_maxSlots}) reached!");
                    return;
                }

                string newSlotName = GenerateUniqueSlotName();

                DataManager.CreateSaveSlot(newSlotName);
                CreateSlotUI(newSlotName);

                CustomLogger.LogInfo($"Created new save slot: '{newSlotName}'");
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to create new save slot: {ex.Message}");
            }
        }

        private bool HandleSlotRename(string oldName, string newName, TMP_InputField inputField)
        {
            // Validate new name
            if (string.IsNullOrWhiteSpace(newName))
            {
                CustomLogger.LogWarning("Save slot name cannot be empty");
                inputField.text = oldName; // Revert to original name
                return false;
            }

            // Check if name actually changed
            if (oldName.Equals(newName, StringComparison.Ordinal))
            {
                return true; // No change needed
            }

            // Check if new name already exists
            if (DataManager.DoesSlotExist(newName))
            {
                CustomLogger.LogWarning($"Save slot '{newName}' already exists");
                inputField.text = oldName; // Revert to original name
                return false;
            }

            // Attempt rename
            try
            {
                DataManager.RenameSaveSlot(oldName, newName);
                CustomLogger.LogInfo($"Renamed save slot from '{oldName}' to '{newName}'");
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to rename save slot: {ex.Message}");
                inputField.text = oldName; // Revert to original name
                return false;
            }
        }

        public void SelectSaveSlot(string slotName)
        {
            if (string.IsNullOrWhiteSpace(slotName))
            {
                CustomLogger.LogError("Cannot select save slot with empty name");
                return;
            }

            if (!DataManager.DoesSlotExist(slotName))
            {
                CustomLogger.LogError($"Save slot '{slotName}' does not exist");
                RefreshUI(); // Refresh in case of desync
                return;
            }

            try
            {
                DataManager.SetActiveSlot(slotName);
                CustomLogger.LogInfo($"Selected save slot: '{slotName}'");

                OnGameStarted?.Invoke();
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to select save slot '{slotName}': {ex.Message}");
            }
        }

        private void ConfirmDeleteSlot(string slotName)
        {
            if (string.IsNullOrWhiteSpace(slotName))
            {
                CustomLogger.LogError("Cannot delete save slot with empty name");
                return;
            }

            // In a real implementation, you might want to show a confirmation dialog
            // For now, we'll delete directly but with safety checks
            DeleteSaveSlot(slotName);
        }

        public void DeleteSaveSlot(string slotName)
        {
            if (!DataManager.DoesSlotExist(slotName))
            {
                CustomLogger.LogWarning($"Cannot delete non-existent save slot: '{slotName}'");
                RefreshUI(); // Refresh in case of desync
                return;
            }

            try
            {
                DataManager.DeleteSaveSlot(slotName);
                RefreshUI(); // Refresh UI after deletion

                CustomLogger.LogInfo($"Deleted save slot: '{slotName}'");
            }
            catch (Exception ex)
            {
                CustomLogger.LogError($"Failed to delete save slot '{slotName}': {ex.Message}");
                RefreshUI(); // Refresh UI even if deletion failed
            }
        }

        #endregion

        #region Utility Methods

        private string GenerateUniqueSlotName()
        {
            int counter = 1;
            string candidateName;

            do
            {
                candidateName = $"{_defaultSlotPrefix}_{counter}";
                counter++;

                // Safety check to prevent infinite loop
                if (counter > 1000)
                {
                    candidateName = $"{_defaultSlotPrefix}_{System.Guid.NewGuid().ToString("N")[..8]}";
                    break;
                }
            }
            while (DataManager.DoesSlotExist(candidateName));

            return candidateName;
        }

        private void CleanupEventListeners()
        {
            // Clean up any remaining event listeners
            foreach (GameObject slotUI in _activeSlotUIs)
            {
                if (slotUI != null)
                {
                    TMP_InputField nameField = slotUI.GetComponentInChildren<TMP_InputField>();
                    Button[] buttons = slotUI.GetComponentsInChildren<Button>();

                    nameField?.onEndEdit.RemoveAllListeners();

                    foreach (Button button in buttons)
                    {
                        button?.onClick.RemoveAllListeners();
                    }
                }
            }
        }

        #endregion
    }
}