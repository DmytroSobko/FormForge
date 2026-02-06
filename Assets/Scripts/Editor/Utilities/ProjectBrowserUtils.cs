using System;
using System.Reflection;
using UnityEngine;

namespace FormForge.Editor.Utilities
{
    public static class ProjectBrowserUtils
    {
        private static Type GetProjectBrowser()
        {
            return typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ProjectBrowser");;
        }
        
        public static bool IsOneColumnMode()
        {
            Type projectBrowserType = GetProjectBrowser();
            if (projectBrowserType == null)
            {
                return false;
            }

            FieldInfo lastInteractedField = projectBrowserType.GetField("s_LastInteractedProjectBrowser", 
                BindingFlags.Public | BindingFlags.Static);
            if (lastInteractedField == null)
            {
                return false;
            }

            object projectBrowserInstance = lastInteractedField.GetValue(null);
            if (projectBrowserInstance == null)
            {
                return false;
            }

            FieldInfo viewModeField = projectBrowserType.GetField("m_ViewMode", 
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (viewModeField == null)
            {
                return false;
            }

            int viewMode = (int)viewModeField.GetValue(projectBrowserInstance);
           
            // 0 = One Column (List View)
            // 1 = Two Columns (Grid View)
            return viewMode == 0;
        }

        public static float GetListAreaScaleFactor()
        {
            Type projectBrowserType = GetProjectBrowser();
            if (projectBrowserType == null)
            {
                return 0;
            }

            FieldInfo lastInteractedField = projectBrowserType.GetField("s_LastInteractedProjectBrowser",
                BindingFlags.Public | BindingFlags.Static);
            FieldInfo gridSizeField = projectBrowserType.GetField("m_LastFoldersGridSize",
                BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo listAreaField = projectBrowserType.GetField("m_ListArea", 
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (lastInteractedField == null || gridSizeField == null || listAreaField == null)
            {
                return 0;
            }

            object projectBrowserInstance = lastInteractedField.GetValue(null);
            if (projectBrowserInstance == null)
            {
                return 0;
            }

            object listAreaInstance = listAreaField.GetValue(projectBrowserInstance);
            if (listAreaInstance == null)
            {
                return 0;
            }

            Type listAreaType = listAreaInstance.GetType();
            FieldInfo maxGridSizeField = listAreaType.GetField("m_MaxGridSize",
                BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo minGridSizeField = listAreaType.GetField("m_MinGridSize", 
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (maxGridSizeField == null || minGridSizeField == null)
            {
                return 0;
            }

            float maxGridSize = Convert.ToSingle(maxGridSizeField.GetValue(listAreaInstance));
            float minGridSize = Convert.ToSingle(minGridSizeField.GetValue(listAreaInstance));
            float currentGridSize = Convert.ToSingle(gridSizeField.GetValue(projectBrowserInstance));

            return Mathf.Approximately(maxGridSize, minGridSize) ? 1f : 
                (currentGridSize - minGridSize) / (maxGridSize - minGridSize);
        }
    }
}