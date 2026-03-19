using System.Collections.Generic;
using FormForge.Domain.Exercises;

namespace FormForge.Domain.TrainingPlans
{
    public class TrainingDay
    {
        public ETrainingDayOfWeek DayOfWeek { get; }
        public List<PlannedExercise> Exercises { get; }

        public TrainingDay(ETrainingDayOfWeek dayOfWeek, List<PlannedExercise> exercises)
        {
            DayOfWeek = dayOfWeek;
            Exercises = exercises;
        }
    }
}