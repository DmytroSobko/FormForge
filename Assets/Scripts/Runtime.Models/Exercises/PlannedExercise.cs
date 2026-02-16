using System;
using FormForge.Domain;

namespace FormForge.Runtime.Models.Exercises
{
    [Serializable]
    public class PlannedExercise
    {
        public Exercise Exercise;
        public EIntensityType Intensity;
    }
}