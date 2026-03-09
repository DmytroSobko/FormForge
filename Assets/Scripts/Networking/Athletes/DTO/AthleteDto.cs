using FormForge.Networking.Common.DTO;

namespace FormForge.Networking.Athletes.DTO
{
    [System.Serializable]
    public class AthleteDto
    {
        public string Id;
        public EAthleteTypeDto Type;
        public string Name;

        public int Strength;
        public int Endurance;
        public int Mobility;

        public int Fatigue;
        public int MaxFatigue;

        public int Week;
    }
}