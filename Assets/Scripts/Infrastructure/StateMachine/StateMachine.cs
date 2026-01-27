using FormForge.Infrastructure.StateMachine.States;

namespace FormForge.Infrastructure.StateMachine
{
    public class StateMachine
    {
        private IState m_CurrentState;

        public IState CurrentState => m_CurrentState;

        public void ChangeState(IState newState)
        {
            if (m_CurrentState == newState)
            {
                return;
            }

            m_CurrentState?.Exit();
            m_CurrentState = newState;
            m_CurrentState.Enter();
        }

        public void Tick()
        {
            m_CurrentState?.Tick();
        }
    }
}