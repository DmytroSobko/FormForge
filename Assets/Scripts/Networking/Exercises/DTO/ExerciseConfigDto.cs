using System.Collections.Generic;
using FormForge.Networking.Athletes.DTO;

namespace FormForge.Networking.Configs.DTO
{
    [System.Serializable]
    public class ExerciseConfigDto
    {
        public EExerciseTypeDto Type;
        public string DisplayName;
        public string Description;
        public EStatTypeDto PrimaryStat;
        public EStatTypeDto SecondaryStat;
        public float SecondaryStatWeight;
        public float BaseGain;
        public float FatigueCost;
        public int DurationMinutes;
        public List<string> AllowedIntensities;
    }
}