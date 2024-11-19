
using StatePattern.Main;
using StatePattern.Player;
using StatePattern.StateMachine;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class HitmanController : EnemyController
    {
        private HitmanStateMachine stateMachine;

        private bool playerIsOnRange = false;

        float timer;
        public HitmanController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            enemyView.SetController(this);
            CreateStateMachine();
            stateMachine.ChangeState(States.IDLE);
        }

        private void CreateStateMachine() => stateMachine = new HitmanStateMachine(this);

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

            if (!playerIsOnRange || !IsPlayerOnSight(GameService.Instance.PlayerService.GetPlayer().GetPlayerView().gameObject))
            {
                return;
            }

            if(!(stateMachine.currentState is ChasingState<HitmanController>) && !(stateMachine.currentState is ShootingState<HitmanController>) && !(stateMachine.currentState is TeleportingState<HitmanController>))
            {
                if(timer <= 0)
                {
                    GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ENEMY_ALERT);
                    timer = 2;
                }

                
                stateMachine.ChangeState(States.CHASING);
            }
        }

        // Initiates shooting and changes the state to TELEPORTING.
        public override void Shoot()
        {
            base.Shoot();
            stateMachine.ChangeState(States.TELEPORTING);
        }

        // Player enters range, change to CHASING state.
        public override void PlayerEnteredRange(PlayerController targetToSet)
        {
            playerIsOnRange = true;
        }

        // Player exits range, change to IDLE state.
        public override void PlayerExitedRange()
        {
            stateMachine.ChangeState(States.IDLE);
            playerIsOnRange = false;
        }
    }
}

