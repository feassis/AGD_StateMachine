using UnityEngine;
using StatePattern.StateMachine;
using StatePattern.Enemy.Bullet;
using StatePattern.Main;
using StatePattern.Player;

namespace StatePattern.Enemy
{
    public class OnePunchManController : EnemyController
    {
        private OnePunchManStateMachine stateMachine;

        private bool isPlayerInRange;

        public OnePunchManController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            enemyView.SetController(this);
            CreateStateMachine();
            stateMachine.ChangeState(States.IDLE);
        }

        private void CreateStateMachine() => stateMachine = new OnePunchManStateMachine(this);

        public override void UpdateEnemy()
        {
            if (currentState == EnemyState.DEACTIVE)
                return;

            base.UpdateEnemy();
            stateMachine.Update();

            if (!isPlayerInRange)
            {
                return;
            }

            if (IsPlayerOnSight(GameService.Instance.PlayerService.GetPlayer().GetPlayerView().gameObject))
            {
                if(!(stateMachine.currentState is ShootingState<OnePunchManController>))
                {
                    GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ENEMY_ALERT);
                }
                stateMachine.ChangeState(States.SHOOTING);
            }
        }

        public override void PlayerEnteredRange(PlayerController targetToSet)
        {
            isPlayerInRange = true;
        }

        public override void PlayerExitedRange()
        {
            isPlayerInRange = false;
            stateMachine.ChangeState(States.IDLE);
        }
    }
}