using System;
using System.Text;
using UnityEngine;

namespace FormForge.Utils
{
    /// <summary>
    /// Provides utility functions for string operations.
    /// </summary>
    public static class StringUtils
    {
        private static StringBuilder m_formattedNumberStringBuilder = new StringBuilder();

        /// <summary>
        /// Cleans up text by removing comments and unnecessary whitespace.
        /// </summary>
        static public string StripText(string source)
        {
            // Clear tabs, leading spaces, end-of-line, etc.
            string strippedScript = source.Replace("\t", "");

            // Get rid of comments and empty lines
            int start = 0;
            int end = 0;

            // Strip comment blocks /* + */
            while (start >= 0)
            {
                start = strippedScript.IndexOf("/*");
                if (start >= 0)
                {
                    end = strippedScript.IndexOf("*/");
                    if (end >= 0)
                    {
                        strippedScript = strippedScript.Substring(0, start) + strippedScript.Substring(end + 2);
                    }
                    else
                    {
                        Debug.LogError("Unclosed */ comment block.");
                        return "";
                    }
                    start = 0;
                }
            }

            // Strip comment //
            string[] lines = strippedScript.Split(new char[] { '\n', '\r' });
            strippedScript = "";
            for (int i = 0; i < lines.Length; i++)
            {
                int commentStart = lines[i].IndexOf("//");
                if (commentStart >= 0)
                {
                    strippedScript += lines[i].Substring(0, commentStart);
                }
                else
                {
                    strippedScript += lines[i];
                }
            }

            // Get rid of line feeds
            strippedScript = strippedScript.Replace("\n", "");
            strippedScript = strippedScript.Replace("\r", "");

            return strippedScript;
        }

        /// <summary>
        /// Formats integers with spaces for readability.
        /// </summary>
        public static string IntToFormattedString(int quantity)
        {
            String number = quantity.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (number.Length <= 3) return number;

            int i = number.Length % 3;
            if (i != 0) m_formattedNumberStringBuilder.Append(number, 0, i);
            for (; i < number.Length; i += 3)
            {
                m_formattedNumberStringBuilder.Append(' ');
                m_formattedNumberStringBuilder.Append(number, i, 3);
            }

            number = m_formattedNumberStringBuilder.ToString();
            m_formattedNumberStringBuilder.Remove(0, m_formattedNumberStringBuilder.Length);
            return number;
        }
    }
}
