using System.Collections.Generic;

namespace FormForge.Networking.Configs.DTO
{
    [System.Serializable]
    public class ExerciseEnvelopeDto
    {
        public string Version;
        public List<ExerciseDto> Exercises;
    }

    [System.Serializable]
    public class ExerciseDto
    {
        public string Id;
        public string DisplayName;
        public string Description;
        public string PrimaryStat;
        public string SecondaryStat;
        public float SecondaryStatWeight;
        public float BaseGain;
        public float FatigueCost;
        public int DurationMinutes;
        public List<string> AllowedIntensities;
    }
}