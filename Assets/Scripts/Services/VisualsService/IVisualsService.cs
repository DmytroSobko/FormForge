using Cysharp.Threading.Tasks;
using FormForge.Domain.Athletes;
using FormForge.Domain.Exercises;
using FormForge.Infrastructure.UI.Toast;
using FormForge.ScriptableObjects.Visuals.Athletes;
using FormForge.ScriptableObjects.Visuals.Exercises;
using FormForge.ScriptableObjects.Visuals.Toasts;

namespace FormForge.Services.VisualsService
{
    public interface IVisualsService
    {
        UniTask InitializeAsync();
        AthleteTypeVisualsConfig GetAthleteTypeVisuals(EAthleteType type);
        ExerciseVisualsConfig GetExerciseVisuals(EExerciseType type);
        ToastVisualsConfig GetToastVisuals(EToastType type);
    }
}