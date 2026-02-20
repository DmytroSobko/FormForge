using System;
using FormForge.Networking.Common.DTO;

namespace FormForge.Networking.AthleteTypeConfigs.DTO
{
    [Serializable]
    public class AthleteTypeConfigDto
    {
        public EAthleteTypeDto Type;
        public string DisplayName;
        public string Description;
        public StatBlockDto BaseStats;
        public float MaxFatigue;
        public float RecoveryMultiplier;
        public float FatigueSensitivity;
    }
}