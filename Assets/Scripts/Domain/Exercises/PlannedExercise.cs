using FormForge.Domain.Intensities;

namespace FormForge.Domain.Exercises
{
    public class PlannedExercise
    {
        public EExerciseType Type { get; }
        public EIntensityType Intensity { get; }

        public PlannedExercise(EExerciseType type, EIntensityType intensity)
        {
            Type = type;
            Intensity = intensity;
        }
    }
}