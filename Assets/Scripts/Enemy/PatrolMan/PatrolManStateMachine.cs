using StatePattern.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class PatrolManStateMachine : GenericStateMachine<PatrolManController>, IStateMachine
    {
        public PatrolManStateMachine(PatrolManController Owner) : base(Owner)
        {
            this.Owner = Owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(StateMachine.States.IDLE, new IdleState(this));
            States.Add(StateMachine.States.PATROLLING, new PatrollingState(this));
            States.Add(StateMachine.States.CHASING, new ChasingState(this));
            States.Add(StateMachine.States.SHOOTING, new ShootingState(this));
        }
    }
}