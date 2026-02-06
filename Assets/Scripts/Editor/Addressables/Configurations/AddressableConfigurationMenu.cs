using FormForge.AddressableConfiguration.Editor.Schema.Helpers;
using FormForge.AddressableConfiguration.Editor.Settings;
using UnityEditor;
using UnityEditor.AddressableAssets;

namespace FormForge.AddressableConfiguration.Editor
{
    public static class AddressableConfigurationMenu
    {
        private const string OPEN_GROUPS_VIEW_MENU_ITEM =
            Constants.MENU_ADDRESSABLES_FOLDER_PREFIX + nameof(OpenGroupsView);
        
        private const string OPEN_CONFIGURATION_SETTINGS_MENU_ITEM =
            Constants.MENU_ADDRESSABLES_FOLDER_PREFIX + nameof(OpenConfigurationSettings);

        private const string FIND_AND_SELECT_SCHEMA_MENU_ITEM =
            Constants.MENU_ADDRESSABLES_FOLDER_PREFIX + nameof(FindAndSelectSchema);

        private const string CONFIGURE_GROUPS_AS_DEFINED_IN_SCHEMA_MENU_ITEM =
            Constants.MENU_FOLDER_CONFIGURATION_GROUPS_PREFIX + nameof(ConfigureGroupsAsDefinedInSchema);

        private const string CLEAR_GROUPS_DEFINED_IN_SCHEMA_MENU_ITEM =
            Constants.MENU_FOLDER_CONFIGURATION_GROUPS_PREFIX + nameof(ClearGroupsDefinedInSchema);

        private const string USE_ASSET_PATHS_AS_ADDRESSES_MENU_ITEM =
            Constants.MENU_FOLDER_NAMING_PREFIX + nameof(UseAssetPathsAsAddresses);

        private const string USE_ASSET_NAMES_AS_ADDRESSES_MENU_ITEM =
            Constants.MENU_FOLDER_NAMING_PREFIX + nameof(UseAssetNamesAsAddresses);

        private const string CONFIGURE_SELECTED_ASSETS_AS_ADDRESSABLE_MENU_ITEM =
            Constants.MENU_FOLDER_CONFIGURATION_ASSETS_PREFIX + nameof(ConfigureSelectedAssetsAsAddressable);

        private const string CLEAR_CONFIGURATION_ON_SELECTED_ASSETS_MENU_ITEM =
            Constants.MENU_FOLDER_CONFIGURATION_ASSETS_PREFIX + nameof(ClearConfigurationOnSelectedAssets);

        [MenuItem(OPEN_GROUPS_VIEW_MENU_ITEM, priority = 1)]
        public static void OpenGroupsView() => 
            EditorApplication.ExecuteMenuItem(Constants.ADDRESSABLE_GROUPS_WINDOW_MENU_ITEM);
        
        [MenuItem(OPEN_CONFIGURATION_SETTINGS_MENU_ITEM, priority = 2)]
        public static void OpenConfigurationSettings() =>
            SettingsService.OpenProjectSettings(AddressableConfigurationSettingsProvider.CONFIGURATION_SETTINGS_PATH);
        
        [MenuItem(FIND_AND_SELECT_SCHEMA_MENU_ITEM, priority = 4)]
        public static void FindAndSelectSchema() =>
            AddressableSchemaHelper.FindAndSelectSchema();
        
        [MenuItem(FIND_AND_SELECT_SCHEMA_MENU_ITEM, validate = true)]
        public static bool ValidateFindAndSelectSchema() =>
            AddressableSchemaHelper.ValidateFindAndSelectSchema();
        
        [MenuItem(CONFIGURE_GROUPS_AS_DEFINED_IN_SCHEMA_MENU_ITEM)]
        public static void ConfigureGroupsAsDefinedInSchema() =>
            AddressableGroupsConfigurator.ConfigureAddressableGroupsAsDefinedInSchema();
        
        [MenuItem(CONFIGURE_GROUPS_AS_DEFINED_IN_SCHEMA_MENU_ITEM, validate = true)]
        public static bool ValidateConfigureAddressableGroupsAsDefinedInSchema() =>
            AddressableSchemaHelper.ValidateFindAndSelectSchema();

        [MenuItem(CLEAR_GROUPS_DEFINED_IN_SCHEMA_MENU_ITEM)]
        public static void ClearGroupsDefinedInSchema() =>
            AddressableGroupsConfigurator.ClearAddressableGroupsDefinedInSchema();
        
        [MenuItem(CLEAR_GROUPS_DEFINED_IN_SCHEMA_MENU_ITEM, validate = true)]
        public static bool ValidateClearAddressableGroupsDefinedInSchema() =>
            AddressableSchemaHelper.ValidateFindAndSelectSchema();
        
        [MenuItem(USE_ASSET_PATHS_AS_ADDRESSES_MENU_ITEM)]
        public static void UseAssetPathsAsAddresses() =>
            AddressableAssetsConfigurator.UseAssetPathsAsAddresses();
        
        [MenuItem(USE_ASSET_PATHS_AS_ADDRESSES_MENU_ITEM, validate = true)]
        public static bool ValidateUseAssetPathsAsAddresses() =>
            AddressableAssetSettingsDefaultObject.Settings != null;
        
        [MenuItem(USE_ASSET_NAMES_AS_ADDRESSES_MENU_ITEM)]
        public static void UseAssetNamesAsAddresses() => 
            AddressableAssetsConfigurator.UseAssetNamesAsAddresses();
        
        [MenuItem(USE_ASSET_NAMES_AS_ADDRESSES_MENU_ITEM, validate = true)]
        public static bool ValidateUseAssetNamesAsAddresses() =>
            AddressableAssetSettingsDefaultObject.Settings != null;
        
        [MenuItem(CONFIGURE_SELECTED_ASSETS_AS_ADDRESSABLE_MENU_ITEM)]
        public static void ConfigureSelectedAssetsAsAddressable() =>
            AddressableAssetsConfigurator.ConfigureSelectedAssetsAsAddressables();

        [MenuItem(CONFIGURE_SELECTED_ASSETS_AS_ADDRESSABLE_MENU_ITEM, validate = true)]
        public static bool ValidateConfigureSelectedAssetsAsAddressable() =>
            AddressableAssetsConfigurator.ValidateConfigureSelectedAssetsAsAddressables();

        [MenuItem(CLEAR_CONFIGURATION_ON_SELECTED_ASSETS_MENU_ITEM)]
        public static void ClearConfigurationOnSelectedAssets() =>
            AddressableAssetsConfigurator.ClearAddressableConfigurationOnSelectedAssets();
        
        [MenuItem(CLEAR_CONFIGURATION_ON_SELECTED_ASSETS_MENU_ITEM, validate = true)]
        public static bool ValidateClearAddressableConfigurationOnSelectedAssets() =>
            AddressableAssetsConfigurator.ValidateClearAddressableConfigurationOnSelectedAssets();
    }
}