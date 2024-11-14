using System.Collections.Generic;

namespace StatePattern.Enemy
{
    public class PatrolManStateMachine : IStateMachine
    {
        private PatrolManController Owner;
        private IState currentState;
        protected Dictionary<States, IState> states = new Dictionary<States, IState>();

        public PatrolManStateMachine(PatrolManController Owner)
        {
            this.Owner = Owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            states.Add(States.IDLE, new IdleState(this, States.PATROLLING));
            states.Add(States.PATROLLING, new PatrollingState(this));
            states.Add(States.CHASING, new ChasingState(this));
            states.Add(States.SHOOTING, new ShootingState(this));
        }

        private void SetOwner()
        {
            foreach (IState state in states.Values)
            {
                state.Owner = Owner;
            }
        }

        protected void ChangeState(IState newState)
        {
            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }

        public void ChangeState(States newState) => ChangeState(states[newState]);

        public void Update() => currentState?.Update();
    }
}