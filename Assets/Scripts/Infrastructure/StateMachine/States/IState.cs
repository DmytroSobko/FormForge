using System.Threading.Tasks;

namespace FormForge.Infrastructure.StateMachine.States
{
    public interface IState
    {
        Task EnterAsync();
        Task ExitAsync();
    }
}