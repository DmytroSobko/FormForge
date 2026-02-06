using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.Schema.GroupRules
{
	[Serializable]
	public class AddressableSettingsGroupRule
	{
#if UNITY_EDITOR
		public const string GROUP_NAME_PROPERTY_NAME = nameof(m_groupName);
		public const string FOLDER_REFERENCE_PROPERTY_NAME = nameof(m_folderReference);
		public const string INCLUDE_SUBFOLDERS_PROPERTY_NAME = nameof(m_includeSubFolders);
#endif

		// AssetDatabase.GetAssetPath seems to return paths with forward slashes instead of respecting the environment's directory path separator
		private const string ASSET_PATH_SEPARATOR = "/";

		[SerializeField]
		private string m_groupName;

		[SerializeField]
		private DefaultAsset m_folderReference;

		[SerializeField]
		private bool m_includeSubFolders = true;

		public string GroupName => m_groupName;
		public DefaultAsset FolderReference => m_folderReference;
		public string FolderReferencePath => AssetDatabase.GetAssetPath(FolderReference) + ASSET_PATH_SEPARATOR;

		public bool AssetPathMatchesGroup(string assetPath)
		{
			return m_includeSubFolders ? 
				assetPath.StartsWith(FolderReferencePath) : 
				MatchesDirectoryName(assetPath, FolderReferencePath);
		}

		private bool MatchesDirectoryName(string assetPath, string folderReferencePath)
		{
			string assetDirectoryName = Path.GetDirectoryName(assetPath);
			string folderReferenceDirectoryName = Path.GetDirectoryName(folderReferencePath);

			return assetDirectoryName == folderReferenceDirectoryName;
		}
	}
}
