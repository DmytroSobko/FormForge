using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.Domain.TrainingPlans;

namespace FormForge.Services.TrainingPlansService
{
    public interface ITrainingPlansService
    {
        UniTask<TrainingPlan> CreateTrainingPlan(string name);
        UniTask<IReadOnlyList<TrainingPlan>> GetTrainingPlans();
    }
}