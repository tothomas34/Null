/// <summary>Base class for states that share one guard context.</summary>
public abstract class GuardBaseState : IState
{
    protected readonly Detector Guard;
    protected GuardBaseState(Detector guard) => Guard = guard;
    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();


}
