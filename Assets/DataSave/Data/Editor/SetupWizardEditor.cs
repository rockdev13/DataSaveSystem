using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SaveLoadSystem
{
    [CustomEditor(typeof(SetupWizard))]
    public class SetupWizardEditor : Editor
    {
        private SerializedProperty compressionProp;
        private SerializedProperty encryptionProp;
        private SerializedProperty autoGenerateProp;
        private SerializedProperty autoGeneratedSlotNameProp;
        private SerializedProperty saveSlotsProp;
        private SerializedProperty loggingDetailProp;

        private SetupWizard wizard;

        private static readonly Color lineColor = new Color(1f, 1f, 1f, 0.15f);

        private void OnEnable()
        {
            compressionProp = serializedObject.FindProperty("_compression");
            encryptionProp = serializedObject.FindProperty("_encryption");
            autoGenerateProp = serializedObject.FindProperty("_autoGenerateSlot");
            autoGeneratedSlotNameProp = serializedObject.FindProperty("_autoGeneratedSlotName");
            saveSlotsProp = serializedObject.FindProperty("_saveSlots");
            loggingDetailProp = serializedObject.FindProperty("_loggingDetail");


            wizard = (SetupWizard)target;
            EditorApplication.update += UpdateSaveSlots;
        }

        private void OnDisable()
        {
            EditorApplication.update -= UpdateSaveSlots;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawSectionHeader("Security and Performance");
            EditorGUILayout.PropertyField(compressionProp);
            EditorGUILayout.PropertyField(encryptionProp);
            EditorGUILayout.Space(10);

            DrawSectionHeader("Ease of Use");
            EditorGUILayout.PropertyField(autoGenerateProp);

            if (autoGenerateProp.boolValue)
            {
                EditorGUILayout.PropertyField(autoGeneratedSlotNameProp);
            }
            EditorGUILayout.Space(10);

            DrawSectionHeader("Debugging");
            EditorGUILayout.PropertyField(loggingDetailProp);
            EditorGUILayout.Space(10);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(saveSlotsProp, true);
            }

            // Button to clear all save files (only in edit-mode)
            if (!Application.isPlaying)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Delete All Save Slots", GUILayout.Width(200)))
                {
                    DataManager.DeleteAllSaveSlots();
                    typeof(SetupWizard)
                        .GetField("_saveSlots", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.SetValue(wizard, new SaveSlot[0]);

                    EditorUtility.SetDirty(wizard);
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSectionHeader(string title)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect lineRect = new Rect(lastRect.x, lastRect.yMax + 2f, lastRect.width, 1f);
            EditorGUI.DrawRect(lineRect, lineColor);

            EditorGUILayout.Space(8);
        }

        private void UpdateSaveSlots()
        {
            if (!Application.isPlaying) return;

            var saveSlots = DataManager.GetSaveSlots();
            typeof(SetupWizard)
                .GetField("_saveSlots", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(wizard, saveSlots.ToArray());

            if (wizard != null)
                EditorUtility.SetDirty(wizard);
        }
    }
}

