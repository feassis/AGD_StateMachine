using StatePattern.Main;
using StatePattern.Player;
using StatePattern.StateMachine;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class ClonemanController : EnemyController
    {
        private ClonemanStateMachine stateMachine;
        public int CloneCountLeft { get; private set; }

        private bool playerInRange;
        private float timer;

        public ClonemanController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            CreateStateMachine();
            SetCloneCount(enemyScriptableObject.CloneCount);
            enemyView.SetController(this);
            ChangeColor(EnemyColorType.Default);
            
            stateMachine.ChangeState(States.IDLE);
        }

        public void SetCloneCount(int cloneCountToSet) => CloneCountLeft = cloneCountToSet;

        private void CreateStateMachine() => stateMachine = new ClonemanStateMachine(this);

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

            if (!playerInRange && !IsPlayerOnSight(GameService.Instance.PlayerService.GetPlayer().GetPlayerView().gameObject))
            {
                return;
            }

            if (!(stateMachine.currentState is ChasingState<ClonemanController>) && !(stateMachine.currentState is CloningState<ClonemanController>) && !(stateMachine.currentState is ShootingState<ClonemanController>))
            {
                if(timer <= 0)
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

        public override void Die()
        {
            if (CloneCountLeft > 0)
                stateMachine.ChangeState(States.CLONING);
            base.Die();
        }

        public void Teleport() => stateMachine.ChangeState(States.TELEPORTING);

        public void SetDefaultColor(EnemyColorType colorType) => enemyView.SetDefaultColor(colorType);

        public void ChangeColor(EnemyColorType colorType) => enemyView.ChangeColor(colorType);
    }
}


