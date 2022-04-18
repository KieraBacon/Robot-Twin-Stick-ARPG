using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieState_Dead : ZombieState
{
    public ZombieState_Dead(ZombieComponent owningZombie, ZombieStateMachine stateMachine) : base(owningZombie, stateMachine)
    {
        updateInterval = 0;
    }

    public override void Enter()
    {
        base.Enter();
        owningZombie.navMeshAgent.isStopped = true;
        owningZombie.navMeshAgent.ResetPath();
        owningZombie.animator.SetFloat(movementZHash, 0);
        owningZombie.animator.SetBool(isDeadHash, true);
        owningZombie.GetComponent<CapsuleCollider>().enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
        owningZombie.navMeshAgent.isStopped = false;
        owningZombie.animator.SetBool(isDeadHash, false);
        owningZombie.GetComponent<CapsuleCollider>().enabled = true;
    }
}
