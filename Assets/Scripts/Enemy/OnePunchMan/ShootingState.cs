using StatePattern.Main;
using StatePattern.Player;
using UnityEngine;

public class ShootingState : OnePunchManBaseState
{
    private OnePunchManStateMachine stateMachine;
    private float targetRotation;
    private PlayerController target;
    private float shootTimer;

    public ShootingState(OnePunchManStateMachine stateMachine) => this.stateMachine = stateMachine;

    public override void OnStateEnter()
    {
        SetTarget();
        shootTimer = 0;
    }

    public override void Update()
    {
        Quaternion desiredRotation = CalculateRotationTowardsPlayer();
        Owner.SetRotation(RotateTowards(desiredRotation));

        if (IsRotationComplete(desiredRotation))
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                ResetTimer();
                Owner.Shoot();
            }
        }
    }

    public override void OnStateExit()
    {
        target = null;
    }

    private void ResetTimer() => shootTimer = Owner.Data.RateOfFire;

    private void SetTarget() => target = GameService.Instance.PlayerService.GetPlayer();

    private Quaternion CalculateRotationTowardsPlayer()
    {
        Vector3 directionToPlayer = target.Position - Owner.Position;
        directionToPlayer.y = 0f;
        return Quaternion.LookRotation(directionToPlayer, Vector3.up);
    }

    private Quaternion RotateTowards(Quaternion desiredRotation) => Quaternion.LerpUnclamped(Owner.Rotation, desiredRotation, Owner.Data.RotationSpeed / 30 * Time.deltaTime);

    private bool IsRotationComplete(Quaternion desiredRotation) => Quaternion.Angle(Owner.Rotation, desiredRotation) < Owner.Data.RotationThreshold;
}