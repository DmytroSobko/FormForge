using FormForge.Networking.Athletes.DTO;

namespace FormForge.Networking.Configs.DTO
{
    [System.Serializable]
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