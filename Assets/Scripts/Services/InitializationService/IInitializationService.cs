using Cysharp.Threading.Tasks;

namespace FormForge.Services.InitializationService
{
    public interface IInitializationService
    {
        UniTask Initialize();
    }
}