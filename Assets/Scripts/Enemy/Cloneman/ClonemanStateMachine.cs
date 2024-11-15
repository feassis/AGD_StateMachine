using StatePattern.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class ClonemanStateMachine : GenericStateMachine<ClonemanController>
    {
        public ClonemanStateMachine(ClonemanController Owner) : base(Owner)
        {
            this.Owner = Owner;
            CreateStates();
            SetOwner();
        }


        private void CreateStates()
        {
            States.Add(StateMachine.States.IDLE, new IdleState<ClonemanController>(this));
            States.Add(StateMachine.States.PATROLLING, new PatrollingState<ClonemanController>(this));
            States.Add(StateMachine.States.CHASING, new ChasingState<ClonemanController>(this));
            States.Add(StateMachine.States.SHOOTING, new ShootingState<ClonemanController>(this));
            States.Add(StateMachine.States.CLONING, new CloningState<ClonemanController>(this));
            States.Add(StateMachine.States.TELEPORTING, new TeleportingState<ClonemanController>(this));
        }
    }
}


