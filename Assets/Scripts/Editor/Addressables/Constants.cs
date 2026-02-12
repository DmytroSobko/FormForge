namespace FormForge.AddressableConfiguration.Editor
{
    public static class Constants
    {
        public const string MENU_FOLDER_PREFIX = "FormForge/";
        public const string MENU_ADDRESSABLES_FOLDER_PREFIX = MENU_FOLDER_PREFIX + "Addressables/";
        
        public const string MENU_FOLDER_NAMING_PREFIX = MENU_ADDRESSABLES_FOLDER_PREFIX + "Naming/";
        public const string MENU_FOLDER_CONFIGURATION_PREFIX = MENU_ADDRESSABLES_FOLDER_PREFIX + "Configuration/";
        public const string MENU_FOLDER_CONFIGURATION_GROUPS_PREFIX = MENU_FOLDER_CONFIGURATION_PREFIX + "Groups/";
        public const string MENU_FOLDER_CONFIGURATION_ASSETS_PREFIX = MENU_FOLDER_CONFIGURATION_PREFIX + "Assets/";
        
        public const string ADDRESSABLES_SCRIPTABLE_OBJECTS_CONTEXT_MENU_NAME = "Scriptable Objects/Addressables/";
        public const string ADDRESSABLES_SCHEMA_CONTEXT_MENU_NAME = ADDRESSABLES_SCRIPTABLE_OBJECTS_CONTEXT_MENU_NAME + "Schema";
        public const string ADDRESSABLES_RULE_CONTEXT_MENU_NAME = ADDRESSABLES_SCRIPTABLE_OBJECTS_CONTEXT_MENU_NAME + "AddressConfigurationRules/";
        
        public const string ADDRESSABLE_GROUPS_WINDOW_MENU_ITEM = "Window/Asset Management/Addressables/Groups";
    }
}