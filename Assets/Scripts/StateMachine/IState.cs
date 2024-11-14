using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
    // Called when the owner enters this state.
    public void OnStateEnter();

    // Called during each game update cycle to update the state's behavior.
    public void Update();

    // Called when the owner exits this state.
    public void OnStateExit();
}
