using System.Collections.Generic;

namespace FormForge.Configs.DTO
{
    [System.Serializable]
    public class AthleteTypesEnvelopeDto
    {
        public string Version;
        public List<AthleteTypeDto> Athletes;
    }

    [System.Serializable]
    public class AthleteTypeDto
    {
        public string Id;
        public string DisplayName;
        public string Description;
        public StatBlockDto BaseStats;
        public float MaxFatigue;
        public float RecoveryMultiplier;
        public float FatigueSensitivity;
    }

    [System.Serializable]
    public class StatBlockDto
    {
        public float Strength;
        public float Endurance;
        public float Mobility;
    }
}