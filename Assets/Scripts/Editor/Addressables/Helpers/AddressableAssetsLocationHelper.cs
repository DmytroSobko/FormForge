using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.Helpers
{
    public static class AddressableAssetsLocationHelper
    {    
        private static readonly AddressableAssetSettings s_addressableSettings =
            AddressableAssetSettingsDefaultObject.Settings;

        public static bool CheckIfFolderContainsAddressables(string folderPath)
        {
            if (s_addressableSettings == null)
            {
                return false;
            }
            
            if (!folderPath.StartsWith("Assets/"))
            {
                return false;
            }

            folderPath = folderPath.Replace("\\", "/");

            foreach (var entry in s_addressableSettings.groups.SelectMany(g => g.entries))
            {
                if (entry.AssetPath.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Counts the number of Addressable assets located within the specified folder.
        /// </summary>
        /// <param name="folderPath">The folder path to search for Addressable assets.</param>
        /// <returns>The total number of Addressable assets found in the folder.</returns>
        public static int GetAddressableAssetCount(string folderPath)
        {
            if (s_addressableSettings == null)
            {
                return 0;
            }

            int assetCount = 0;
            var addressableGroups = s_addressableSettings.groups;
            foreach (var group in addressableGroups)
            {
                foreach (var entry in group.entries)
                {
                    if (entry.AssetPath.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase))
                    {
                        assetCount++;
                    }
                }
            }
            return assetCount;
        }
        
        /// <summary>
        /// Retrieves addressable asset entries while excluding entries from the specified groups.
        /// If <paramref name="folderPaths"/> is null or empty, all non-ignored entries are returned.
        /// </summary>
        /// <param name="folderPaths">Optional list of folder paths to filter entries by location.</param>
        /// <param name="ignoredGroups">Optional list of group names to exclude from the results.</param>
        /// <returns>A list of addressable asset entries that match the filtering criteria.</returns>
        public static List<AddressableAssetEntry> GetFilteredAddressableEntries(List<string> folderPaths = null, 
            List<string> ignoredGroups = null)
        {
            var addressableEntriesList = new List<AddressableAssetEntry>();
            if (s_addressableSettings == null)
            {
                return addressableEntriesList;
            }

            bool filterByFolder = folderPaths!= null && folderPaths.Count > 0;
            HashSet<string> folderSet = filterByFolder ? 
                new HashSet<string>(folderPaths, StringComparer.OrdinalIgnoreCase) : null;

            foreach (var group in s_addressableSettings.groups)
            {
                if (ignoredGroups != null && ignoredGroups.Contains(group.Name))
                {
                    continue;
                }

                foreach (var entry in group.entries)
                {
                    if (!filterByFolder || folderSet.Contains(entry.AssetPath) || IsChildOfAnyFolder(entry.AssetPath, folderSet))
                    {
                        addressableEntriesList.Add(entry);
                    }
                }
            }

            return addressableEntriesList;
        }
        
        /// <summary>
        /// Validates addressable asset entries, ensuring they have correct address formatting.
        /// Entries from ignored groups are skipped. If <paramref name="folderPaths"/> is null or empty, all entries are validated.
        /// Fixes detected inconsistencies where possible.
        /// </summary>
        /// <typeparam name="TRequester">The type of the class requesting validation, used for logging.</typeparam>
        /// <param name="folderPaths">Optional list of folder paths to filter which entries are validated.</param>
        /// <param name="ignoredGroups">Optional list of group names to exclude from validation.</param>
        /// <returns>True if all entries are consistent; False if any inconsistencies were found and fixed.</returns>
        public static bool AreAddressablesConsistent<TRequester>(List<string> folderPaths = null, 
            List<string> ignoredGroups = null)
        {
            Debug.Log("Starting address validation...");
            bool inconsistencyFound = false;

            if (s_addressableSettings == null)
            {
                Debug.LogError("AddressableAssetSettings not found.");
                return false;
            }

            bool filterByFolder = folderPaths != null && folderPaths.Count > 0;
            HashSet<string> folderSet = filterByFolder ? new HashSet<string>(folderPaths, StringComparer.OrdinalIgnoreCase) : null;

            foreach (var group in s_addressableSettings.groups)
            {
                if (ignoredGroups != null && ignoredGroups.Contains(group.Name))
                {
                    continue;
                }
                
                Debug.Log($"Validating group: {group.Name}");
                foreach (var entry in group.entries)
                {
                    if (!filterByFolder || folderSet.Contains(entry.AssetPath) || IsChildOfAnyFolder(entry.AssetPath, folderSet))
                    {
                        inconsistencyFound |= FixAddressableEntry<TRequester>(entry);
                    }
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log("Address validation complete.");
            return !inconsistencyFound;
        }

        /// <summary>
        /// Ensures an addressable asset entry follows proper formatting by:
        /// - Prepending "Assets/" if the address is not structured under a folder.
        /// - Replacing backslashes (\) with forward slashes (/).
        /// </summary>
        /// <typeparam name="TRequester">The type requesting the fix, used for logging.</typeparam>
        /// <param name="entry">The Addressable asset entry to validate and potentially fix.</param>
        /// <returns>True if any modifications were made; otherwise, false.</returns>
        private static bool FixAddressableEntry<TRequester>(AddressableAssetEntry entry)
        {
            bool fixedSomething = false;
            string assetInfo = $"Asset: {entry.AssetPath}, Address: {entry.address}";

            if (!entry.address.Contains("/"))
            {
                Debug.LogWarning($"{assetInfo} is not structured under a folder. Fixing...");
                entry.address = $"Assets/{entry.address}";
                fixedSomething = true;
            }

            if (entry.address.Contains("\\"))
            {
                Debug.LogWarning($"{assetInfo} contains backslashes (\\). Replacing...");
                entry.address = entry.address.Replace('\\', '/');
                fixedSomething = true;
            }

            return fixedSomething;
        }

        /// <summary>
        /// Determines whether the specified asset path is a descendant of any folder in the given set.
        /// </summary>
        /// <param name="assetPath">The asset path to check.</param>
        /// <param name="folders">A set of folder paths to compare against.</param>
        /// <returns>True if the asset path starts with any folder path in the set; otherwise, false.</returns>
        private static bool IsChildOfAnyFolder(string assetPath, HashSet<string> folders)
        {
            if (folders == null)
            {
                return false;
            }
            
            foreach (var folder in folders)
            {
                if (assetPath.StartsWith(folder, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
