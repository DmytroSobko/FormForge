using FormForge.Domain.TrainingPlans;
using FormForge.Infrastructure.UI.Pagination;

namespace FormForge.UI.Screens.ViewModels.TrainingPlansScreen
{
    public class TrainingPlanItemViewModel : IPaginatedItemViewModel
    {
        public TrainingPlan TrainingPlan { get; }
        
        public TrainingPlanItemViewModel(TrainingPlan trainingPlan)
        {
            TrainingPlan = trainingPlan;
        }
    }
}