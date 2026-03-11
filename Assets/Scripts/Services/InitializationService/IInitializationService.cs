using Cysharp.Threading.Tasks;

namespace FormForge.Services.InitializationService
{
    public interface IInitializationService
    {
        bool IsInitialized { get; }
        UniTask Initialize();
    }
}