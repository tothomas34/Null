using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class GuardPatrolState : GuardBaseState
{


    public GuardPatrolState(Detector guard) : base(guard)
    {
    }

    public override void Enter()
    {
        Guard.agent.autoBraking = false;
        Guard.NextStep();
        Guard.PatrolMod();

    }
    public override void Tick()
    {
       if (!Guard.agent.pathPending && Guard.agent.remainingDistance < 0.5f)
            Guard.NextStep();

        if(Guard.Detection() == true) {
            Guard.StaterChanger(Guard.alertState);
        }
    }


    public override void Exit()
    {

    }
}


public class GuardAlertState : GuardBaseState
{

    public GuardAlertState(Detector guard) : base(guard)
    {

    }

    public override void Enter()
    {
        Guard.Chase();
        Guard.ChaseMod();

        Debug.Log("ALERTE");
    }
    public override void Tick()
    {
        float distance = Guard.GetDistanceToPlayer();
        if( distance > 5f) { 
            Guard.Chase();
        }
        else
        {
            //Guard.StopAgent();
        }
        if (Guard.Detection() == false) {
            Guard.StaterChanger(Guard.susState);
        }

    }
    public override void Exit()
    {

    }
}

public class GuardShootingState : GuardBaseState
{

    public GuardShootingState(Detector guard) : base(guard)
    {

    }

    public override void Enter()
    {

    }
    public override void Tick()
    {

    }
    public override void Exit()
    {

    }
}

public class GuardSusState : GuardBaseState
{

    public GuardSusState(Detector guard) : base(guard)
    {

    }

    public override void Enter()
    {
        Vector3 playerPosition = Guard.WhereIsPlayer();
        Guard.isSuspicious(playerPosition);
    }
    public override void Tick()
    {
        if (Guard.Detection() == true)
        {
            Guard.StaterChanger(Guard.alertState);
        }

    }
    public override void Exit()
    {

    }
}
