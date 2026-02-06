using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.Utilities
{
    public static class EditorGUIUtils
    {
        public static string TruncateText(string text, GUIStyle style, float maxWidth)
        {
            float width = style.CalcSize(new GUIContent(text)).x;
            if (width > maxWidth)
            {
                float availableWidth = maxWidth - 10;
                int maxLength = Mathf.FloorToInt(availableWidth / width * text.Length);
                if (maxLength < text.Length)
                {
                    return text.Substring(0, maxLength) + "...";
                }
            }
            return text;
        }
    }
}