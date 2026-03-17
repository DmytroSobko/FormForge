using System.Collections.Generic;

namespace FormForge.Domain.TrainingPlans
{
    public class TrainingPlan
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TrainingDay> Days{ get; set; }
    }
}