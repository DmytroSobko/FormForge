using System.IO;
using FormForge.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.Settings.Data
{
    [InitializeOnLoad]
    public class AddressableConfigurationSettingsLoader
    {
        private const string SETTINGS_FILE_NAME = "AddressableConfigurationSettings.json";
        public static AddressableConfigurationSettings ConfigurationSettings  { get; private set; }

        static AddressableConfigurationSettingsLoader()
        {
            LoadSettings();
        }

        public static void SaveSettings()
        {
            string projectSettingsPath = GetStaticsGeneratorsSettingsPath();
            if (!Directory.Exists(Path.GetDirectoryName(projectSettingsPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(projectSettingsPath));
            }
            File.WriteAllText(projectSettingsPath, JsonUtility.ToJson(ConfigurationSettings));
        }

        private static void LoadSettings()
        {
            if (SettingsProviderUtils.TryGetSettingsJson(GetStaticsGeneratorsSettingsPath(),
                    out string projectSettingsJson))
            {
                ConfigurationSettings =
                    JsonUtility.FromJson<AddressableConfigurationSettings>(projectSettingsJson);
            }
            else
            {
                ConfigurationSettings = new AddressableConfigurationSettings();
            }
        }

        private static string GetStaticsGeneratorsSettingsPath()
        {
            return Path.Combine(SettingsProviderUtils.GetProjectSettingsPath(), SETTINGS_FILE_NAME);
        }
    }
}
