using System.Collections.Generic;
using FormForge.AddressableConfiguration.Editor.Schema.AddressRules;
using FormForge.AddressableConfiguration.Editor.Schema.GroupRules;
using UnityEngine;

namespace FormForge.AddressableConfiguration.Editor.Schema
{
	[CreateAssetMenu(menuName = Constants.ADDRESSABLES_SCHEMA_CONTEXT_MENU_NAME)]
    public class AddressableSchema : ScriptableObject
    {
        public List<AddressableSettingsGroupRule> rules = new List<AddressableSettingsGroupRule>();
        
        [Tooltip("Rule that, when using the configuration functions from the package, will set the address " +
                 "of an asset (one or multiple that are going to be configured), in the Addressable settings " +
                 "according to the logic implemented in the rule.")]
        public AddressConfigurationRule addressConfigurationRule;
    }    
}
