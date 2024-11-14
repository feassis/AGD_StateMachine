using System.Collections;
using Unity.IO.LowLevel.Unsafe;

public abstract class OnePunchManBaseState : IState
{
    public OnePunchManController Owner { get; set; }

    public virtual void OnStateEnter()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}
