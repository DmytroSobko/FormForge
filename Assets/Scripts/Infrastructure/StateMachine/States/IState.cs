using Cysharp.Threading.Tasks;

namespace FormForge.Infrastructure.StateMachine.States
{
    public interface IState
    {
        UniTask EnterAsync();
        UniTask ExitAsync();
    }
}