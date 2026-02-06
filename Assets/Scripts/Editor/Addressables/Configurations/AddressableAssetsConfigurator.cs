using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FormForge.AddressableConfiguration.Editor.Schema.AddressRules;
using FormForge.AddressableConfiguration.Editor.Schema.Helpers;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using Object = UnityEngine.Object;

namespace FormForge.AddressableConfiguration.Editor
{
    public static class AddressableAssetsConfigurator
    {      
        private static readonly AddressableAssetSettings s_addressableSettings =
            AddressableAssetSettingsDefaultObject.Settings;
        
        public static void UseAssetPathsAsAddresses()
        {
            UpdateAddressableNames(entry => entry.AssetPath);
        }

        public static void UseAssetNamesAsAddresses()
        {
            UpdateAddressableNames(entry => Path.GetFileNameWithoutExtension(entry.AssetPath));
        }

        private static void UpdateAddressableNames(Func<AddressableAssetEntry, string> namingStrategy)
        {
            if (s_addressableSettings == null)
            {
                return;
            }

            foreach (var group in s_addressableSettings.groups)
            {
                foreach (var entry in new List<AddressableAssetEntry>(group.entries))
                {
                    string newAddress = namingStrategy(entry);
                    if (entry.address != newAddress)
                    {
                        entry.SetAddress(newAddress);
                    }
                }
            }

            AssetDatabase.SaveAssets();
        }
        
        public static void ConfigureSelectedAssetsAsAddressables()
        {
            IEnumerable<string> selectedAssetGUIDs = GetSelectedAssetGUIDs();
            ConfigureAssetsAsAddressable(selectedAssetGUIDs);
            RefreshInspector();
        }

        public static bool ValidateConfigureSelectedAssetsAsAddressables()
        {
            return SelectionIsValidForAddressableConfig();
        }

        public static void ClearAddressableConfigurationOnSelectedAssets()
        {
            IEnumerable<string> selectedAssetGUIDs = GetSelectedAssetGUIDs();
            ClearAddressableConfigurationOnAssets(selectedAssetGUIDs);
            RefreshInspector();
        }

        public static bool ValidateClearAddressableConfigurationOnSelectedAssets()
        {
            return SelectionIsValidForAddressableConfig();
        }

        private static bool SelectionIsValidForAddressableConfig()
        {
            if (Selection.objects.Length == 0)
            {
                return false;
            }

            string assetPath = AssetDatabase.GetAssetPath(Selection.objects[0]);
            return !string.IsNullOrEmpty(assetPath);
        }

        private static IEnumerable<string> GetSelectedAssetGUIDs()
        {
            foreach (var selectedObject in Selection.objects)
            {
                string assetPath = AssetDatabase.GetAssetPath(selectedObject);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    yield return AssetDatabase.AssetPathToGUID(assetPath);
                }
            }
        }

        private static void RefreshInspector()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(RefreshInspectorCoroutine());
        }

        private static IEnumerator RefreshInspectorCoroutine()
        {
            Object[] previousSelection = Selection.objects;
            Selection.objects = null;
            yield return null;
            Selection.objects = previousSelection;
        }
        
        private static void ConfigureAssetsAsAddressable(IEnumerable<string> assetGUIDs)
        {
            AddressableAssetGroup addressableGroup = s_addressableSettings.DefaultGroup;
            ConfigureAssetsAsAddressable(assetGUIDs, addressableGroup);
        }

        public static void ConfigureAssetsAsAddressable(IEnumerable<string> assetGUIDs, 
            AddressableAssetGroup addressableGroup)
        {
            AddressConfigurationRule addressConfigurationRule = AddressableSchemaHelper.GetAddressConfigurationRule();
			
            foreach (string assetGUID in assetGUIDs)
            {
                Undo.RecordObject(s_addressableSettings, "Add assets to addressable settings");

                AddressableAssetEntry entry = s_addressableSettings.CreateOrMoveEntry(assetGUID, addressableGroup);

                if (addressConfigurationRule == null)
                {
                    continue;
                }

                entry.address = addressConfigurationRule.GenerateAddress(assetGUID);
            }
        }
		
        private static void ClearAddressableConfigurationOnAssets(IEnumerable<string> assetGUIDs)
        {
            Undo.RecordObject(s_addressableSettings, "Clear assets from addressable settings");

            foreach (string assetGUID in assetGUIDs)
            {
                s_addressableSettings.RemoveAssetEntry(assetGUID);
            }
        }
    }
}
