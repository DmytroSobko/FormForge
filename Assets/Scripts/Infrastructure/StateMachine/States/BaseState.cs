namespace FormForge.Infrastructure.StateMachine.States
{
    public abstract class BaseState : IState
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Tick() { }
    }
}