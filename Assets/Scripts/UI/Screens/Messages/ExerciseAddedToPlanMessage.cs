using FormForge.Domain.Exercises;
using FormForge.Domain.Intensities;

namespace FormForge.UI.Screens.Messages
{
    public class ExerciseAddedToPlanMessage
    {
        public EExerciseType Type
        {
            get;
        }
        
        public EIntensityType Intensity 
        {
            get;
        }
        
        public ExerciseAddedToPlanMessage(EExerciseType type, EIntensityType intensity)
        {
            Type = type;
            Intensity = intensity;
        }
    }
}