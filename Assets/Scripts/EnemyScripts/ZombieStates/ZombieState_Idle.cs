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
        owningZombie.animator.SetFloat(movementZHash, 0);

        owningZombie.targetDetector.onNewTargetInRange += OnNewTargetInRange;
    }

    public override void Exit()
    {
        base.Exit();
        owningZombie.navMeshAgent.isStopped = false;

        owningZombie.targetDetector.onNewTargetInRange -= OnNewTargetInRange;
    }

    public override void IntervalUpdate()
    {
        base.IntervalUpdate();
        if (!owningZombie.followTarget)
        {
            GameObject target = owningZombie.targetDetector.GetNearestTarget();
            if (target)
            {
                owningZombie.followTarget = target;
                stateMachine.ChangeState(ZombieState.Type.Following);
                return;
            }
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
