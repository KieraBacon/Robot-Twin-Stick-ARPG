using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieState_Idle : ZombieState
{
    public ZombieState_Idle(ZombieComponent owningZombie, ZombieStateMachine stateMachine) : base(owningZombie, stateMachine)
    {
        updateInterval = 0;
    }

    public override void Enter()
    {
        base.Enter();
        owningZombie.navMeshAgent.isStopped = true;
        owningZombie.navMeshAgent.ResetPath();
        owningZombie.animator.SetFloat(movementZHash, 0);
    }

    public override void Exit()
    {
        base.Exit();
        owningZombie.navMeshAgent.isStopped = false;
    }
}
