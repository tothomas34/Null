using UnityEngine;

/// <summary>Owns one active state and guarantees orderly exit/enter transitions.</summary>
public sealed class StateMachine
{
    public IState CurrentState { get; private set; }

    public void ChangeState(IState next)
    {
        if (next == null)
        {
            Debug.LogError("StateMachine: cannot change to a null state.");
            return;
        }
        if (ReferenceEquals(CurrentState, next)) return;
        CurrentState?.Exit();
        CurrentState = next;
        CurrentState.Enter();
    }

    public void Tick() => CurrentState?.Tick();

    public void Stop()
    {
        CurrentState?.Exit();
        CurrentState = null;
    }

   
}
