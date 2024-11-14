using UnityEngine;
// Define a state where the character is idle.
public class IdleState : OnePunchManBaseState
{
    private OnePunchManStateMachine stateMachine;
    private float timer;

    public IdleState(OnePunchManStateMachine stateMachine) => this.stateMachine = stateMachine;

    public override void OnStateEnter() => ResetTimer();

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            stateMachine.ChangeState(OnePunchManStates.ROTATING);
    }

    public override void OnStateExit() => timer = 0;

    private void ResetTimer() => timer = Owner.Data.IdleTime;
}


