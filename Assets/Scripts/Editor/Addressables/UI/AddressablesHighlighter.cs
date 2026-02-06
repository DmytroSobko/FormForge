using System.Collections.Generic;
using System.Linq;
using FormForge.AddressableConfiguration.Editor.Helpers;
using FormForge.AddressableConfiguration.Editor.Settings.Data;
using FormForge.AddressableConfiguration.Editor.Utilities;
using FormForge.Editor.Utilities;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.UI
{
    [InitializeOnLoad]
    public static class AddressablesHighlighter
    {
        private const float LABEL_SIZE = 16f;
        private const int LABEL_FONT_MAX_SIZE = 9;
        private const int MIN_LABEL_WIDTH = 20;
        private const int MAX_LABEL_WIDTH = 70;
        private const int LABEL_HEIGHT = 12;
        private const float LABEL_PADDING = 6f;

        private const float BOX_OFFSET_X = 2f;
        private const float BOX_OFFSET_Y = 2f;
        private const float LABEL_OFFSET_X = 3f;
        private const float LABEL_OFFSET_Y = 3f;
        private const float GROUP_OFFSET = 4f;

        private const float GROUP_NAME_SIZE = 16f;
        private const float MIN_GROUP_LABEL_VISIBILITY_SCALE_FACTOR = 0.5f;
        private static readonly Color s_groupLabelBackgroundColor = new Color (0, 0, 0, 1f);

        private static readonly AddressableAssetSettings s_addressableSettings =
            AddressableAssetSettingsDefaultObject.Settings;

        private static readonly AddressableConfigurationSettings s_configurationSettings =
            AddressableConfigurationSettingsLoader.ConfigurationSettings;

        private static readonly Dictionary<string, bool> cachedFolderStatus = new Dictionary<string, bool>();

        static AddressablesHighlighter()
        {
            RefreshCache();

            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
            EditorApplication.delayCall += RefreshCache;

            if (s_addressableSettings != null)
            {
                AddressableAssetSettingsDefaultObject.Settings.OnModification += OnAddressablesModified;
            }
        }

        private static void OnAddressablesModified(AddressableAssetSettings settings,
            AddressableAssetSettings.ModificationEvent evt, object obj)
        {
            RefreshCache();
            EditorApplication.RepaintProjectWindow();
        }

        private static void OnProjectWindowItemGUI(string guid, Rect rect)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path) || s_addressableSettings == null)
            {
                return;
            }

            AddressableAssetEntry assetEntry = s_addressableSettings.FindAssetEntry(guid);
            bool isAddressable = assetEntry != null;
            bool isFolder = AssetDatabase.IsValidFolder(path);

            if (s_configurationSettings.ShowLabels)
            {
                if (isAddressable)
                {         
                    DrawAssetLabel(rect, GetGroupLabelConfig(assetEntry.parentGroup.Name));
                }

                if (isFolder && IsAddressableFolder(path))
                {
                    DrawFolderLabel(rect);
                }
            }

            if (isAddressable && s_configurationSettings.ShowGroupNames)
                DrawGroupName(rect, assetEntry.parentGroup.Name);
        }

        private static AddressableGroupLabelConfig GetGroupLabelConfig(string groupName) =>
            s_configurationSettings.GetGroupLabelConfig(groupName) ??
            new AddressableGroupLabelConfig { Label = "Default", LabelColor = s_configurationSettings.DefaultLabelColor };
        
        private static void DrawAssetLabel(Rect rect, AddressableGroupLabelConfig labelConfig = null)
        {
            GUIStyle labelStyle = GetLabelStyle(labelConfig?.LabelColor ?? s_configurationSettings.DefaultLabelColor);
            GUI.Label(GetLabelRect(rect), labelConfig?.Label ?? s_configurationSettings.DefaultLabel, labelStyle);
        }

        private static GUIStyle GetLabelStyle(Color textColor) => 
            new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = LABEL_FONT_MAX_SIZE,
                normal = { textColor = textColor },
                alignment = TextAnchor.MiddleCenter
            };

        private static Rect GetLabelRect(Rect rect) => 
            new Rect(rect.xMax - LABEL_SIZE, rect.y, LABEL_SIZE, LABEL_SIZE);

        private static void DrawFolderLabel(Rect rect)
        {
            GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = LABEL_FONT_MAX_SIZE,
                normal = { textColor = s_configurationSettings.FolderLabelColor },
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(GetLabelRect(rect), s_configurationSettings.FoldertLabel, labelStyle);
        }
        
        private static void DrawGroupName(Rect rect, string text)
        {
            bool isListViewMode = ProjectBrowserUtils.IsOneColumnMode();
            bool drawGroupNameNextToLabel = isListViewMode;
            float scaleFactor = isListViewMode ? 1 : ProjectBrowserUtils.GetListAreaScaleFactor();

            if (!isListViewMode && scaleFactor < MIN_GROUP_LABEL_VISIBILITY_SCALE_FACTOR)
            {
                if (scaleFactor > 0)
                {
                    return;
                }
                drawGroupNameNextToLabel = true;
                scaleFactor = 1;
            }

            GUIStyle labelStyle = GetGroupNameStyle(scaleFactor);
            float textWidth = labelStyle.CalcSize(new GUIContent(text)).x;
            float maxLabelWidth = Mathf.Lerp(MIN_LABEL_WIDTH, MAX_LABEL_WIDTH, scaleFactor);
            float finalWidth = Mathf.Min(textWidth + LABEL_PADDING, maxLabelWidth);

            Rect boxRect;
            Rect labelRect;

            if (drawGroupNameNextToLabel)
            {
                float groupXOffset = rect.xMax - GROUP_NAME_SIZE - finalWidth;

                boxRect = new Rect(groupXOffset - GROUP_OFFSET, rect.y + BOX_OFFSET_Y, 
                    finalWidth, LABEL_HEIGHT * scaleFactor);
                labelRect = new Rect(groupXOffset - (GROUP_OFFSET - 1), rect.y + LABEL_OFFSET_Y, 
                    finalWidth, LABEL_HEIGHT * scaleFactor);
            }
            else
            {
                boxRect = new Rect(rect.x + BOX_OFFSET_X, rect.y + BOX_OFFSET_Y, 
                    finalWidth, LABEL_HEIGHT * scaleFactor);
                labelRect = new Rect(rect.x + LABEL_OFFSET_X, rect.y + LABEL_OFFSET_Y,
                    finalWidth, LABEL_HEIGHT * scaleFactor);
            }

            Color originalColor = GUI.color;
            GUI.color = s_groupLabelBackgroundColor;
            GUI.Box(boxRect, GUIContent.none);
            GUI.color = originalColor;

            string truncatedText = EditorGUIUtils.TruncateText(text, labelStyle, finalWidth);
            GUI.Label(labelRect, truncatedText, labelStyle);
        }

        private static GUIStyle GetGroupNameStyle(float scaleFactor) => 
            new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = Mathf.RoundToInt(LABEL_FONT_MAX_SIZE * scaleFactor),
                normal = { textColor = Color.white },
                alignment = TextAnchor.UpperLeft
            };

        private static void RefreshCache()
        {
            cachedFolderStatus.Clear();
            var allFolders = AssetDatabase.GetAllAssetPaths().Where(AssetDatabase.IsValidFolder);
            foreach (var folder in allFolders)
            {
                cachedFolderStatus[folder] = AddressableAssetsLocationHelper.CheckIfFolderContainsAddressables(folder);
            }
        }

        private static bool IsAddressableFolder(string folderPath)
        {
            if (cachedFolderStatus.TryGetValue(folderPath, out bool containsAddressables))
            {
                return containsAddressables;
            }
            containsAddressables = AddressableAssetsLocationHelper.CheckIfFolderContainsAddressables(folderPath);
            cachedFolderStatus[folderPath] = containsAddressables;
            return containsAddressables;
        }
    }
}
