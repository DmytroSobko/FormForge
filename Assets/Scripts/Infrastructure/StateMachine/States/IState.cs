namespace FormForge.Infrastructure.StateMachine.States
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Tick();
    }
}