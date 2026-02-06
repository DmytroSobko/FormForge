using System;
using System.Collections.Generic;
using System.Linq;
using FormForge.AddressableConfiguration.Editor.Settings.Data;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.Settings
{
    public class AddressableConfigurationSettingsDrawer
    {
        private const int MAX_LABEL_SYMBOLS = 2;
        
        public Action SaveSettings;
        
        private int selectedGroupIndex;
        private bool foldoutGroups = true;

        private readonly AddressableConfigurationSettings m_settings =
            AddressableConfigurationSettingsLoader.ConfigurationSettings;

        public void Draw()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                DrawConfigurationSettings();
                if (check.changed)
                {
                    SaveSettings?.Invoke();
                }
            }
        }

        private void DrawConfigurationSettings()
        {
            using (new EditorGUI.IndentLevelScope())
            {
                var settings = AddressableAssetSettingsDefaultObject.Settings;
                if (settings == null)
                {
                    EditorGUILayout.HelpBox("Addressable settings not found.", MessageType.Warning);
                    return;
                }

                DrawGeneralSettings();
                if (!m_settings.ShowLabels)
                {
                    return;
                }

                DrawDefaultLabelSettings();
                DrawFolderLabelSettings();
                DrawGroupLabelSettings(settings);
            }
        }

        private void DrawGeneralSettings()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                m_settings.ShowGroupNames = EditorGUILayout.Toggle("Show Group Names", m_settings.ShowGroupNames);
                m_settings.ShowLabels = EditorGUILayout.Toggle("Show Labels", m_settings.ShowLabels);
            }
        }

        private void DrawDefaultLabelSettings()
        {
            EditorGUILayout.Space(10);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Default Label", EditorStyles.boldLabel);

                m_settings.DefaultLabel = TruncateText(
                    EditorGUILayout.TextField("Label", m_settings.DefaultLabel), MAX_LABEL_SYMBOLS);

                m_settings.DefaultLabelColor = EditorGUILayout.ColorField("Label Color", m_settings.DefaultLabelColor);
            }
        }
        
        private void DrawFolderLabelSettings()
        {
            EditorGUILayout.Space(10);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Addressables Containing Folder Label", EditorStyles.boldLabel);

                m_settings.FoldertLabel = TruncateText(
                    EditorGUILayout.TextField("Folder Label", m_settings.FoldertLabel), MAX_LABEL_SYMBOLS);

                m_settings.FolderLabelColor = EditorGUILayout.ColorField("Folder Label Color", m_settings.FolderLabelColor);
            }
        }

        private void DrawGroupLabelSettings(AddressableAssetSettings settings)
        {
            EditorGUILayout.Space(10);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Addressable Group Labeling", EditorStyles.boldLabel);

                List<string> groupNames = settings.groups.Select(g => g.Name).ToList();
                selectedGroupIndex = EditorGUILayout.Popup("Select Group", selectedGroupIndex, groupNames.ToArray());

                if (GUILayout.Button("Add Group to List"))
                {
                    AddGroupToList(groupNames[selectedGroupIndex]);
                }

                DrawGroupLabelConfigurations();
            }
        }

        private void AddGroupToList(string groupName)
        {
            if (m_settings.GroupLabelConfigs.Any(c => c.GroupName == groupName))
            {
                return;
            }
            
            m_settings.GroupLabelConfigs.Add(new AddressableGroupLabelConfig
            {
                GroupName = groupName,
                Label = m_settings.DefaultLabel,
                LabelColor = m_settings.DefaultLabelColor
            });
        }

        private void DrawGroupLabelConfigurations()
        {
            foldoutGroups = EditorGUILayout.Foldout(foldoutGroups, "Addressable Groups with Labels", true);
            if (!foldoutGroups) return;

            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < m_settings.GroupLabelConfigs.Count; i++)
                {
                    if (DrawGroupLabelConfig(m_settings.GroupLabelConfigs[i]))
                    {
                        m_settings.GroupLabelConfigs.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private bool DrawGroupLabelConfig(AddressableGroupLabelConfig config)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField($"Group: {config.GroupName}", EditorStyles.boldLabel);

                config.Label = TruncateText(EditorGUILayout.TextField("Label", config.Label), MAX_LABEL_SYMBOLS);
                config.LabelColor = EditorGUILayout.ColorField("Label Color", config.LabelColor);

                return GUILayout.Button("Remove");
            }
        }

        private string TruncateText(string text, int maxLength)
        {
            return text.Length > maxLength ? text.Substring(0, maxLength) : text;
        }
    }
}
