using UnityEngine;

/// <summary>Contract shared by all concrete guard states.</summary>
public interface IState
{
    void Enter();
    void Tick();
    void Exit();
}
