using UnityEditor;
using System.IO;
using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.Schema.AddressRules
{
	[CreateAssetMenu(menuName = Constants.ADDRESSABLES_RULE_CONTEXT_MENU_NAME + nameof(DefaultAddressConfigurationRule))]
	public class DefaultAddressConfigurationRule : AddressConfigurationRule
	{
		public override string GenerateAddress(string assetGUID)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
			string assetName = Path.GetFileNameWithoutExtension(assetPath);

			string address = assetName;
			return address;
		}
	}
}