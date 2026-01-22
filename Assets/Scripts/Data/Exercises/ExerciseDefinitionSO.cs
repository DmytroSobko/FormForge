using FormForge.Core.Domain;
using UnityEngine;

namespace FormForge.Data.Exercises
{
    [CreateAssetMenu(fileName = "ExerciseDefinitionSO", menuName = "Scriptable Objects/ExerciseDefinitionSO")]
    public class ExerciseDefinitionSO : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string m_Id;
        [SerializeField] private string m_DisplayName;
        [SerializeField] private string m_Description;

        [Header("Stat Impact")]
        [SerializeField] private EStatType m_PrimaryStat;
        [SerializeField] private EStatType m_SecondaryStat;
        [SerializeField] private float m_SecondaryStatWeight;
        
        [Header("Intensity & Cost")]
        [SerializeField] private EIntensityType m_Intensity;
        [SerializeField] private float m_BaseGain;
        [SerializeField] private float m_FatigueCost;
        [SerializeField] private int m_DurationMinutes;
        
        [Header("Presentation")]
        [SerializeField] private Sprite m_Icon;

        public string Id => m_Id;
        public string DisplayName => m_DisplayName;
        public string Description => m_Description;
        public EStatType PrimaryStat => m_PrimaryStat;
        public EStatType SecondaryStat => m_SecondaryStat;
        public float SecondaryStatWeight => m_SecondaryStatWeight;
        public EIntensityType Intensity => m_Intensity;
        public float BaseGain => m_BaseGain;
        public float FatigueCost => m_FatigueCost;
        public int DurationMinutes => m_DurationMinutes;
        public Sprite Icon => m_Icon;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(m_Id))
        {
            Debug.LogWarning(
                $"[{name}] ExerciseDefinitionSO has an empty Id. Ids must be unique and non-empty.",
                this);
        }

        if (string.IsNullOrWhiteSpace(m_DisplayName))
        {
            Debug.LogWarning($"[{name}] ExerciseDefinitionSO has an empty DisplayName.", this);
        }
        
        if (string.IsNullOrWhiteSpace(m_Description))
        {
            Debug.LogWarning($"[{name}] ExerciseDefinitionSO has an empty Description.", this);
        }

        m_SecondaryStatWeight = ClampWithWarning(m_SecondaryStatWeight, 0f, 0.5f, 
            nameof(m_SecondaryStatWeight), "Secondary stat impact should be subtle.");

        m_BaseGain = ClampWithWarning(m_BaseGain, 0.5f, 10f, nameof(m_BaseGain),
            "BaseGain represents raw stat growth before modifiers.");

        m_FatigueCost = ClampWithWarning(m_FatigueCost, 1f, 25f, nameof(m_FatigueCost),
            "FatigueCost represents fatigue added per execution.");

        if (m_DurationMinutes <= 0 || m_DurationMinutes > 180)
        {
            Debug.LogWarning($"[{name}] DurationMinutes ({m_DurationMinutes}) should be in range [1, 180].",
                this);

            m_DurationMinutes = Mathf.Clamp(m_DurationMinutes, 1, 180);
        }

        ValidateIntensityBalance();
    }

    private void ValidateIntensityBalance()
    {
        switch (m_Intensity)
        {
            case EIntensityType.Low:
                WarnIf(m_BaseGain > 4f, "Low intensity exercises should not have high BaseGain.");
                WarnIf(m_FatigueCost > 6f, "Low intensity exercises should not cause high fatigue.");
                break;

            case EIntensityType.Medium:
                WarnIf(m_BaseGain < 2f || m_BaseGain > 7f,
                    "Medium intensity BaseGain is usually in range [2–7].");
                break;

            case EIntensityType.High:
                WarnIf(m_BaseGain < 4f,
                    "High intensity exercises should provide noticeable gains.");
                WarnIf(m_FatigueCost < 8f,
                    "High intensity exercises should meaningfully increase fatigue.");
                break;
        }
    }

    private float ClampWithWarning(float value, float min, float max, string fieldName, string context = null)
    {
        if (value < min || value > max)
        {
            Debug.LogWarning(
                $"[{name}] {fieldName} value ({value}) is outside " +
                $"recommended range [{min}, {max}]. " +
                $"{context}",
                this
            );
        }

        return Mathf.Clamp(value, min, max);
    }

    private void WarnIf(bool condition, string message)
    {
        if (condition)
        {
            Debug.LogWarning($"[{name}] {message}", this);
        }
    }
#endif
    }
}