
using StatePattern.Player;
using StatePattern.StateMachine;

namespace StatePattern.Enemy
{
    public class HitmanStateMachine : GenericStateMachine<HitmanController>
    {
        public HitmanStateMachine(HitmanController Owner) : base(Owner)
        {
            this.Owner = Owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(StateMachine.States.IDLE, new IdleState<HitmanController>(this));
            States.Add(StateMachine.States.PATROLLING, new PatrollingState<HitmanController>(this));
            States.Add(StateMachine.States.CHASING, new ChasingState<HitmanController>(this));
            States.Add(StateMachine.States.SHOOTING, new ShootingState<HitmanController>(this));
            States.Add(StateMachine.States.TELEPORTING, new TeleportingState<HitmanController>(this));
        }
    }

    public class HitmanController : EnemyController
    {
        private HitmanStateMachine stateMachine;

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

            stateMachine.Update();
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
            base.PlayerEnteredRange(targetToSet);
            stateMachine.ChangeState(States.CHASING);
        }

        // Player exits range, change to IDLE state.
        public override void PlayerExitedRange() => stateMachine.ChangeState(States.IDLE);
    }
}

