using FormForge.AddressableConfiguration.Editor.Schema.AddressRules;
using UnityEngine;
using UnityEditor;

namespace FormForge.AddressableConfiguration.Editor.Schema.Helpers
{
	public static class AddressableSchemaHelper
	{
		public static void FindAndSelectSchema()
		{
			var schema = GetSchema();
			if (schema == null)
			{
				return;
			}
            
			Selection.activeObject = schema;
			EditorGUIUtility.PingObject(schema);
			Debug.Log($"Schema found at: {AssetDatabase.GetAssetPath(schema)}");
		}
        
		public static bool ValidateFindAndSelectSchema()
		{ 
			var schema = GetSchema();
			return schema != null;
		}
		
		public static AddressableSchema GetSchema()
		{
			string[] guids = AssetDatabase.FindAssets($"t:{nameof(AddressableSchema)}");
			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				AddressableSchema asset = AssetDatabase.LoadAssetAtPath<AddressableSchema>(path);
				if (asset != null)
				{
					return asset;
				}
			}
			Debug.LogWarning($"No {nameof(AddressableSchema)} asset found.");
			return null;
		}
		
		public static AddressConfigurationRule GetAddressConfigurationRule()
		{
			AddressableSchema schema = GetSchema();
			AddressConfigurationRule addressConfigurationRule = null;
			if (schema != null)
			{
				addressConfigurationRule = schema.addressConfigurationRule;
			}
			return addressConfigurationRule;
		}
	}
}