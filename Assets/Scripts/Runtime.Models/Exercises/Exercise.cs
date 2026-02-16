using System;
using System.Collections.Generic;
using FormForge.Domain;

namespace FormForge.Runtime.Models.Exercises
{
    [Serializable]
    public class Exercise
    {
        public string Id;
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