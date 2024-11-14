﻿using StatePattern.Main;
using StatePattern.Player;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class ChasingState : IState
    {
        public EnemyController Owner { get; set; }
        private IStateMachine stateMachine;
        private PlayerController target;

        public ChasingState(IStateMachine stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            SetTarget();
            SetStoppingDistance();
        }

        public void Update()
        {
            MoveTowardsTarget();
            if (ReachedTarget())
            {
                ResetPath();
                stateMachine.ChangeState(States.SHOOTING);
            }
        }

        public void OnStateExit() => target = null;


        private void SetTarget() => target = GameService.Instance.PlayerService.GetPlayer();
        private void SetStoppingDistance() => Owner.Agent.stoppingDistance = Owner.Data.PlayerStoppingDistance;

        private bool MoveTowardsTarget() => Owner.Agent.SetDestination(target.Position);

        private bool ReachedTarget() => Vector3.Distance(target.Position,Owner.GetViewTransform().position) <= Owner.Agent.stoppingDistance;

        private void ResetPath()
        {
            Owner.Agent.isStopped = true;
            Owner.Agent.ResetPath();
        }
    }
}