using System.IO;
using UnityEngine;

namespace FormForge.Editor.Utilities
{
    /// <summary>
    /// A collection of static functions to aid with the creation and use of SettingsProviders
    /// <br/>
    /// See: https://docs.unity3d.com/ScriptReference/SettingsProvider.html
    /// </summary>
    public static class SettingsProviderUtils
    {
        public const string PROJECT_SETTINGS_LOCATION = "Project/FormForge";
        
        /// <summary>
        /// Retrieves Unity's ProjectSettings path appended with the Prime sub-folder.
        /// </summary>
        /// <returns>
        /// Return the ProjectSettings path.
        /// <br/>
        /// Example: .../UnityProject/ProjectSettings/FormForge/
        /// </returns>
        public static string GetProjectSettingsPath()
        {
            string projectPath = Directory.GetParent(Application.dataPath).FullName;
            string projectSettingsPath = Path.Combine(projectPath, "ProjectSettings/FormForge");

            return projectSettingsPath;
        }

        /// <summary>
        /// Retrieves the path of the FormForge sub-folder in Application.persistentDataPath
        /// </summary>
        /// <returns>Returns {Application.persistentDataPath}/FormForge</returns>
        public static string GetUserSettingsPath()
        {
            return Path.Combine(Application.persistentDataPath, "FormForge");
        }

        /// <summary>
        /// Attempts to read a JSON string from a file at the specified path.
        /// </summary>
        /// <param name="path">Path to the JSON file</param>
        /// <param name="settingJson">The JSON string read from the file</param>
        /// <returns>True if the file was successfully opened and read</returns>
        public static bool TryGetSettingsJson(string path, out string settingJson)
        {
            settingJson = string.Empty;
            
            string projectSettingsPath = path;
            if (File.Exists(projectSettingsPath))
            {
                settingJson = File.ReadAllText(projectSettingsPath);
                return true;
            }

            return false;
        }
    }
}