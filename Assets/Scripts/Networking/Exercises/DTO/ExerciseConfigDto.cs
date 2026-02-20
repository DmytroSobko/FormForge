using System;
using System.Collections.Generic;
using FormForge.Networking.Common.DTO;

namespace FormForge.Networking.Exercises.DTO
{
    [Serializable]
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
        public HashSet<EIntensityTypeDto> AllowedIntensities;
    }
}