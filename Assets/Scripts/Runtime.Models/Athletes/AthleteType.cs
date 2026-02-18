using FormForge.Domain;

namespace FormForge.Runtime.Models.Athletes
{
    public class AthleteType
    {
        public EAthleteType Type;
        public string DisplayName;
        public string Description;
        public StatBlock StatBlock;
        public float MaxFatigue;
        public float RecoveryMultiplier;
        public float FatigueSensitivity;
    }
    
    public class StatBlock
    {
        public float Strength;
        public float Endurance;
        public float Mobility;
    }
}