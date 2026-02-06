using System.Collections.Generic;
using FormForge.AddressableConfiguration.Editor.Settings.Data;
using FormForge.Editor.Utilities;
using UnityEditor;
using UnityEngine.UIElements;

namespace FormForge.AddressableConfiguration.Editor.Settings
{
    public class AddressableConfigurationSettingsProvider : SettingsProvider
    {
        internal static readonly string CONFIGURATION_SETTINGS_PATH = string.Join("/",
            SettingsProviderUtils.PROJECT_SETTINGS_LOCATION, "Addressable Configuration Settings");

        private AddressableConfigurationSettingsDrawer m_drawer;

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new AddressableConfigurationSettingsProvider(CONFIGURATION_SETTINGS_PATH,
                SettingsScope.Project);
        }

        private AddressableConfigurationSettingsProvider(string path, SettingsScope scopes, 
            IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
 
            m_drawer = new AddressableConfigurationSettingsDrawer();
            m_drawer.SaveSettings += AddressableConfigurationSettingsLoader.SaveSettings;
        }
        
        public override void OnDeactivate()
        {
            if (m_drawer != null)
            {
                m_drawer.SaveSettings -= AddressableConfigurationSettingsLoader.SaveSettings;
            }
            base.OnDeactivate();
        }

        public override void OnGUI(string searchContext)
        {
            m_drawer.Draw();
        }
    }
}
