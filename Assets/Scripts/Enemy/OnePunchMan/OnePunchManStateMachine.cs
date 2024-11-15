using StatePattern.StateMachine;
using System.Collections.Generic;

namespace StatePattern.Enemy
{
    public class OnePunchManStateMachine : GenericStateMachine<OnePunchManController> , IStateMachine
    {
         public OnePunchManStateMachine(OnePunchManController Owner) : base(Owner)
        {
            this.Owner = Owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(StateMachine.States.IDLE, new IdleState(this));
            States.Add(StateMachine.States.ROTATING, new RotatingState(this));
            States.Add(StateMachine.States.SHOOTING, new ShootingState(this));
        }
    }
}