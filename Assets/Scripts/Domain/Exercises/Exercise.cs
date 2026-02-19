using System.Collections.Generic;
using FormForge.Domain.Intensities;

namespace FormForge.Domain.Exercises
{
    public class Exercise
    {
        public EExerciseType Type;
        
        public string DisplayName;
        public string Description;

        public EStatType PrimaryStat;
        public EStatType? SecondaryStat;
        public float SecondaryStatWeight;

        public float BaseGain;
        public float FatigueCost;
        public int DurationMinutes;

        public HashSet<EIntensityType> AllowedIntensities;
    }
}