using System.Collections.Generic;

public class OnePunchManStateMachine
{
    // Reference to the owner of the state machine
   public OnePunchManController Owner { get; private set;}

    // Dictionary to store available states, mapped to enum values
    protected Dictionary<OnePunchManStates, OnePunchManBaseState> States = new Dictionary<OnePunchManStates, OnePunchManBaseState>();

    // Reference to the current state
    private OnePunchManBaseState currentState;

    public OnePunchManStateMachine(OnePunchManController Owner)
    {
        this.Owner = Owner;
        CreateStates();
        SetOwner();
    }

    // Update the current state (if it exists)
    public void Update() => currentState?.Update();

    // Create and initialize the states
    private void CreateStates()
    {
        States.Add(OnePunchManStates.IDLE, new IdleState(this));
        States.Add(OnePunchManStates.ROTATING, new RotatingState(this));
        States.Add(OnePunchManStates.SHOOTING, new ShootingState(this));
    }

    // Set the owner for each state
    private void SetOwner()
    {
        foreach (OnePunchManBaseState state in States.Values)
        {
            state.Owner = Owner;
        }
    }

    // Change the current state to a new state
    protected void ChangeState(OnePunchManBaseState newState)
    {
        currentState?.OnStateExit(); // Exit the current state (if it exists)
        currentState = newState; // Set the new state
        currentState?.OnStateEnter(); // Enter the new state
    }

    // Change the current state to a new state by providing a state enum
    public void ChangeState(OnePunchManStates newState) => ChangeState(States[newState]);
}
