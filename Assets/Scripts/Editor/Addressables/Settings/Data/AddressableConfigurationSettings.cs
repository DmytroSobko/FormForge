using System;
using System.Collections.Generic;
using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.Settings.Data
{
    [Serializable]
    public class AddressableConfigurationSettings
    {
        public bool ShowGroupNames = true;
        public bool ShowLabels = true;
        
        public string FoldertLabel = "AF";
        public Color FolderLabelColor = Color.green;
        
        public string DefaultLabel = "A";
        public Color DefaultLabelColor = Color.red;
        
        public List<AddressableGroupLabelConfig> GroupLabelConfigs = new List<AddressableGroupLabelConfig>();

        public AddressableGroupLabelConfig GetGroupLabelConfig(string groupName)
        {
            foreach (var config in GroupLabelConfigs)
            {
                if (config.GroupName == groupName)
                {
                    return config;
                }
            }
            return null;
        }
    }

    [Serializable]
    public class AddressableGroupLabelConfig
    {
        public string GroupName;
        public string Label;
        public Color LabelColor;
    }
}
