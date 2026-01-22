using UnityEngine;

namespace FormForge.Data.Athletes
{
    [CreateAssetMenu(fileName = "AthleteDefinitionSO", menuName = "Scriptable Objects/AthleteDefinitionSO")]
    public class AthleteDefinitionSO : ScriptableObject
    {
        [SerializeField] private string m_Id;
        [SerializeField] private string m_DisplayName;
        [Range(0f, 100f)]
        [SerializeField] private float m_BaseStrength;
        [Range(0f, 100f)]
        [SerializeField] private float m_BaseEndurance;
        [Range(0f, 100f)]
        [SerializeField] private float m_BaseMobility;
        [Range(1f, 15f)]
        [SerializeField] private float m_BaseRecoveryRate;
        [Range(80f, 150f)]
        [SerializeField] private float m_MaxFatigue;
        [Range(0.8f, 1.2f)]
        [SerializeField] private float m_ConsistencyModifier;

        public string Id => m_Id;
        public string DisplayName => m_DisplayName;
        public float BaseStrength => m_BaseStrength;
        public float BaseEndurance => m_BaseEndurance;
        public float BaseMobility => m_BaseMobility;
        public float BaseRecoveryRate => m_BaseRecoveryRate;
        public float MaxFatigue => m_MaxFatigue;
        public float ConsistencyModifier => m_ConsistencyModifier;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(m_Id))
            {
                Debug.LogWarning($"[{name}] AthleteDefinitionSO has an empty Id. " +
                                 "This should be a unique, non-empty identifier.", this);
            }

            if (string.IsNullOrWhiteSpace(m_DisplayName))
            {
                Debug.LogWarning($"[{name}] AthleteDefinitionSO has an empty DisplayName.", this);
            }

            m_BaseStrength = 
                ClampWithWarning(m_BaseStrength, 0f, 100f, nameof(m_BaseStrength));
            m_BaseEndurance = 
                ClampWithWarning(m_BaseEndurance, 0f, 100f, nameof(m_BaseEndurance));
            m_BaseMobility = 
                ClampWithWarning(m_BaseMobility, 0f, 100f, nameof(m_BaseMobility));
            m_BaseRecoveryRate = 
                ClampWithWarning(m_BaseRecoveryRate, 1f, 15f, nameof(m_BaseRecoveryRate));
            m_MaxFatigue = 
                ClampWithWarning(m_MaxFatigue, 80f, 150f, nameof(m_MaxFatigue));
            m_ConsistencyModifier = 
                ClampWithWarning(m_ConsistencyModifier, 0.8f, 1.2f, nameof(m_ConsistencyModifier));
        }

        private float ClampWithWarning(float value, float min, float max, string fieldName)
        {
            if (value < min || value > max)
            {
                Debug.LogWarning($"[{name}] {fieldName} value ({value}) is outside " +
                                 $"the recommended range [{min}, {max}] and will be clamped.", this);
            }

            return Mathf.Clamp(value, min, max);
        }
#endif
    }
}