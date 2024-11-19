using StatePattern.Main;
using StatePattern.Player;
using StatePattern.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class PatrolManController : EnemyController
    {
        private PatrolManStateMachine stateMachine;

        private bool playerInRange;
        private float timer;

        public PatrolManController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            enemyView.SetController(this);
            CreateStateMachine();
            stateMachine.ChangeState(States.IDLE);
        }

        private void CreateStateMachine() => stateMachine = new PatrolManStateMachine(this);

        public override void UpdateEnemy()
        {
            if (currentState == EnemyState.DEACTIVE)
                return;

            base.UpdateEnemy();
            stateMachine.Update();

            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }

            if (!playerInRange || !IsPlayerOnSight(GameService.Instance.PlayerService.GetPlayer().GetPlayerView().gameObject))
            {
                return;
            }

            if (!(stateMachine.currentState is ChasingState<PatrolManController>) && !(stateMachine.currentState is ShootingState<PatrolManController>))
            {
                if (timer <= 0)
                {
                    GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ENEMY_ALERT);
                    timer = 1;
                }

                stateMachine.ChangeState(States.CHASING);
            }
        }

        public override void PlayerEnteredRange(PlayerController targetToSet)
        {
            playerInRange = true;
        }

        public override void PlayerExitedRange()
        {
            stateMachine.ChangeState(States.IDLE);
            playerInRange = false;
        }
    }
}