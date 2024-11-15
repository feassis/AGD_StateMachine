using StatePattern.Main;
using StatePattern.StateMachine;

namespace StatePattern.Enemy
{
    public class CloningState<T> : IState where T : EnemyController
    {
        public EnemyController Owner { get; set; }
        private GenericStateMachine<T> stateMachine;

        public CloningState(GenericStateMachine<T> stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            CreateClone();
            CreateClone();
        }

        public void OnStateExit()
        {
            
        }

        public void Update()
        {
            
        }

        private void CreateClone()
        {
            ClonemanController clone = GameService.Instance.EnemyService.CreateEnemy(Owner.Data) as ClonemanController;
            clone.SetCloneCount((Owner as ClonemanController).CloneCountLeft - 1);
            clone.Teleport();
            clone.SetDefaultColor(EnemyColorType.Clone);
            clone.ChangeColor(EnemyColorType.Clone);
            GameService.Instance.EnemyService.AddEnemy(clone);
        }
    }
}