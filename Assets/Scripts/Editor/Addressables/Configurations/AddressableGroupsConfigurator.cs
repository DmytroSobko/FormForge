using System.Collections.Generic;
using FormForge.AddressableConfiguration.Editor.Schema;
using FormForge.AddressableConfiguration.Editor.Schema.GroupRules;
using FormForge.AddressableConfiguration.Editor.Schema.Helpers;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor
{
    public static class AddressableGroupsConfigurator
    {
        private static readonly AddressableAssetSettings s_addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        private static readonly AddressableSchema s_schema = AddressableSchemaHelper.GetSchema();

        public static void ConfigureAddressableGroupsAsDefinedInSchema()
        {
            if (s_addressableSettings == null || s_schema == null)
            {
                Debug.LogError("Missing Addressable settings or schema.");
                return;
            }

            Dictionary<string, List<string>> groupedAssets = GetGroupedTargetAssets(s_schema.rules);
            foreach (var entry in groupedAssets)
            {
                ConfigureAssetsAsAddressable(entry.Value, entry.Key, true);
            }

            AssetDatabase.SaveAssets();
            Debug.Log("Addressable groups configured.");
        }

        private static Dictionary<string, List<string>> GetGroupedTargetAssets(List<AddressableSettingsGroupRule> rules, bool ignoreFolders = true)
        {
            Dictionary<string, List<string>> groupedAssets = new Dictionary<string, List<string>>();
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();

            foreach (string assetPath in allAssetPaths)
            {
                if (ignoreFolders && AssetDatabase.IsValidFolder(assetPath))
                {
                    continue;
                }

                foreach (var rule in rules)
                {
                    if (rule.AssetPathMatchesGroup(assetPath))
                    {
                        string guid = AssetDatabase.AssetPathToGUID(assetPath);
                        if (!groupedAssets.ContainsKey(rule.GroupName))
                        {
                            groupedAssets[rule.GroupName] = new List<string>();
                        }
                        groupedAssets[rule.GroupName].Add(guid);
                        break;
                    }
                }
            }

            return groupedAssets;
        }

        private static void ConfigureAssetsAsAddressable(IEnumerable<string> assetGUIDs,
            string addressableGroupName,
            bool createGroupByName = false)
        {
            AddressableAssetGroup addressableAssetGroup = GetAddressableAssetGroupByName(addressableGroupName, createGroupByName);
            AddressableAssetsConfigurator.ConfigureAssetsAsAddressable(assetGUIDs, addressableAssetGroup);
        }

        private static AddressableAssetGroup GetAddressableAssetGroupByName(string addressableGroupName, bool createGroupByName)
        {
            AddressableAssetGroup group = s_addressableSettings.FindGroup(addressableGroupName);

            if (group == null && createGroupByName)
            {
                return s_addressableSettings.CreateGroup(addressableGroupName,
                    setAsDefaultGroup: false,
                    readOnly: false,
                    postEvent: true,
                    schemasToCopy: null,
                    typeof(BundledAssetGroupSchema),
                    typeof(ContentUpdateGroupSchema));
            }

            return group;
        }

        public static void ClearAddressableGroupsDefinedInSchema()
        {
            if (s_addressableSettings == null || s_schema == null)
            {
                Debug.LogError("Missing Addressable settings or schema.");
                return;
            }

            foreach (var group in s_addressableSettings.groups)
            {
                if (!group.ReadOnly && s_schema.rules.Exists(rule => rule.GroupName == group.Name))
                {
                    ClearAddressableGroup(group);
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log("Addressable groups cleared.");
        }

        private static void ClearAddressableGroup(AddressableAssetGroup group, bool removeGroup = false)
        {
            Undo.RecordObject(s_addressableSettings, "Clear addressable asset group");

            if (removeGroup)
            {
                AddressableAssetSettings.ModificationEvent modificationEvent =
                    AddressableAssetSettings.ModificationEvent.GroupRemoved;
                s_addressableSettings.RemoveGroup(group);
                s_addressableSettings.SetDirty(modificationEvent, group, true);
                return;
            }

            Undo.RecordObject(group, "Clear addressable asset group");

            var assets = new List<AddressableAssetEntry>(group.entries);
            int assetCount = assets.Count;

            for (int i = assetCount - 1; i >= 0; i--)
            {
                if (assets[i] != null)
                {
                    AddressableAssetSettings.ModificationEvent modificationEvent =
                        AddressableAssetSettings.ModificationEvent.EntryRemoved;
                    group.RemoveAssetEntry(assets[i]);
                    s_addressableSettings.SetDirty(modificationEvent, assets[i], true);
                }
            }
        }
    }
}