using System.Collections.Generic;

namespace FormForge.Domain.TrainingPlans
{
    public class TrainingPlan
    {
        public string Id { get; set; }
        public string Name { get; }
        public List<TrainingDay> Days { get; }

        public TrainingPlan(string name)
        {
            Name = name;
        }

        public void AddDay(TrainingDay day)
        {
            Days.Add(day);
        }
    }
}