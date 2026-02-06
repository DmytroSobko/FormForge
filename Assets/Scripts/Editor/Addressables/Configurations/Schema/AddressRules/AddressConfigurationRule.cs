using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.Schema.AddressRules
{
	public abstract class AddressConfigurationRule : ScriptableObject
	{
		public abstract string GenerateAddress(string assetGUID);
	}
}