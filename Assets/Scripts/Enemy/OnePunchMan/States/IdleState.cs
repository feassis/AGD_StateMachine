using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class IdleState : IState
    {
        public EnemyController Owner { get; set; }
        private IStateMachine stateMachine;
        private float timer;
        private States desiredAfterState;
        public IdleState(IStateMachine stateMachine, States desiredAfterState)
        {
            this.stateMachine = stateMachine;
            this.desiredAfterState = desiredAfterState;
        }
            

        public void OnStateEnter() => ResetTimer();

        public void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                stateMachine.ChangeState(desiredAfterState);
        }

        public void OnStateExit() => timer = 0;

        private void ResetTimer() => timer = Owner.Data.IdleTime;
    }
}