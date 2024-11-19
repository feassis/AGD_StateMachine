using CodeMonkey.Utils;
using StatePattern.Enemy.Bullet;
using StatePattern.Main;
using StatePattern.Player;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

namespace StatePattern.Enemy
{
    public class EnemyController
    {
        protected EnemyScriptableObject enemyScriptableObject;
        protected EnemyView enemyView;

        protected int currentHealth;
        protected EnemyState currentState;
        public NavMeshAgent Agent => enemyView.Agent;
        public EnemyScriptableObject Data => enemyScriptableObject;
        public Quaternion Rotation => enemyView.transform.rotation;
        public Vector3 Position => enemyView.transform.position;

        private FieldOFViewController fieldOFViewController;


        public EnemyController(EnemyScriptableObject enemyScriptableObject)
        {
            this.enemyScriptableObject = enemyScriptableObject;
            InitializeView();
            InitializeVariables();
            InitializeFieldOfView();
        }

        public void InitializeFieldOfView()
        {
            fieldOFViewController = new FieldOFViewController(enemyScriptableObject.fieldOfViewSO, enemyScriptableObject.fieldOfViewPrefab);
        }

        private void InitializeView()
        {
            enemyView = Object.Instantiate(enemyScriptableObject.EnemyPrefab);
            enemyView.transform.position = enemyScriptableObject.SpawnPosition;
            enemyView.transform.rotation = Quaternion.Euler(enemyScriptableObject.SpawnRotation);
            enemyView.SetTriggerRadius(enemyScriptableObject.RangeRadius);
        }

        private void InitializeVariables()
        {
            SetState(EnemyState.ACTIVE);
            currentHealth = enemyScriptableObject.MaximumHealth;
        }

        public void InitializeAgent()
        {
            Agent.enabled = true;
            Agent.SetDestination(enemyScriptableObject.SpawnPosition);
            Agent.speed = enemyScriptableObject.MovementSpeed;
        }

        public virtual void Die() 
        {
            GameService.Instance.EnemyService.EnemyDied(this);
            for(int i = 0; i < enemyScriptableObject.coinsToSpawn; i++)
            {
                GameObject.Instantiate(enemyScriptableObject.coinPrefab, enemyView.transform.position, Quaternion.identity);
            }

            fieldOFViewController.Destroy();
            enemyView.Destroy();
        }

        public float GetViewRange() => enemyScriptableObject.fieldOfViewSO.ViewDistance;
        public float GetViewFOV() => enemyScriptableObject.fieldOfViewSO.FOV;
        public int GetObstacleLayer() => enemyScriptableObject.fieldOfViewSO.ColisionMask;

        public bool IsPlayerOnSight(GameObject other)
        {
            var dir = other.transform.position - enemyView.transform.position;

            Debug.DrawRay(enemyView.transform.position, dir, Color.red, 1f);
            Debug.DrawRay(enemyView.transform.position, enemyView.transform.forward, Color.white, 1f);


            bool IsObstructed = Physics.Raycast(enemyView.transform.position, dir, GetViewRange(), GetObstacleLayer());
            bool isOnCorrectAngle = Vector3.Angle(enemyView.transform.forward, dir) < GetViewFOV() / 2;

            return isOnCorrectAngle && !IsObstructed;
        }

        public void ToggleKillOverlay(bool value) => GameService.Instance.UIService.ToggleKillOverlay(value);

        public void ShakeCamera() => GameService.Instance.UIService.ShakeCamera();

        public void SetRotation(Vector3 eulerAngles) => enemyView.transform.rotation = Quaternion.Euler(eulerAngles);

        public void SetRotation(Quaternion desiredRotation) => enemyView.transform.rotation = desiredRotation;

        public void ToggleEnemyColor(bool value)=>  enemyView.ToggleColor(value);
        

        public virtual void Shoot()
        {
            enemyView.PlayShootingEffect();
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ENEMY_SHOOT);
            BulletController bullet = new BulletController(enemyView.transform, enemyScriptableObject.BulletData);
        }

        public void SetState(EnemyState stateToSet) => currentState = stateToSet;

        public virtual void PlayerEnteredRange(PlayerController targetToSet) => GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ENEMY_ALERT);

        public virtual void PlayerExitedRange() { }

        public virtual void UpdateEnemy()
        {
            fieldOFViewController.SetAimDirection(enemyView.transform.right * -1);
            fieldOFViewController.SetOrigin(enemyView.transform.position);
        }
    }

    public enum EnemyState
    {
        ACTIVE,
        DEACTIVE
    }
}