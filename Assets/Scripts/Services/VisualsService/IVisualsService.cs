using Cysharp.Threading.Tasks;
using FormForge.Domain.Athletes;
using FormForge.ScriptableObjects.Athletes;

namespace FormForge.Services.VisualsService
{
    public interface IVisualsService
    {
        UniTask InitializeAsync();
        AthleteTypeVisualsConfig GetAthleteTypeVisuals(EAthleteType type);
    }
}