using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieState_Idle : ZombieState
{
    public ZombieState_Idle(ZombieComponent owningZombie, ZombieStateMachine stateMachine) : base(owningZombie, stateMachine)
    {
        updateInterval = 5;
    }

    public override void Enter()
    {
        base.Enter();
        owningZombie.navMeshAgent.isStopped = true;
        owningZombie.navMeshAgent.ResetPath();
        owningZombie.animator.SetFloat(movementXHash, 0);

        if (!owningZombie.followTarget)
            GetNearestTarget();

        owningZombie.targetDetector.onNewTargetInRange += OnNewTargetInRange;
    }

    public override void Exit()
    {
        base.Exit();
        owningZombie.navMeshAgent.isStopped = false;

        owningZombie.targetDetector.onNewTargetInRange -= OnNewTargetInRange;
    }

    private void GetNearestTarget()
    {
        GameObject target = owningZombie.targetDetector.GetNearestTarget();
        if (target)
        {
            owningZombie.followTarget = target;
            stateMachine.ChangeState(ZombieState.Type.Following);
        }
    }

    public override void IntervalUpdate()
    {
        base.IntervalUpdate();
        if (owningZombie.followTarget)
        {
            stateMachine.ChangeState(ZombieState.Type.Following);
        }
        else
        {
            GetNearestTarget();
        }
    }

    private void OnNewTargetInRange(GameObject target)
    {
        if (!owningZombie.followTarget)
        {
            owningZombie.followTarget = target;
            stateMachine.ChangeState(ZombieState.Type.Following);
            return;
        }
    }
}
