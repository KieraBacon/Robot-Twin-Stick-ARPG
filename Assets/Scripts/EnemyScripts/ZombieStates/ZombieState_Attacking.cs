using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieState_Attacking : ZombieState
{
    float attackRange = 2;
    private IDamageable damageableTarget;

    public ZombieState_Attacking(ZombieComponent zombie, ZombieStateMachine stateMachine) : base(zombie, stateMachine)
    {
        updateInterval = 2;
    }

    public override void Enter()
    {
        base.Enter();
        owningZombie.navMeshAgent.isStopped = true;
        owningZombie.navMeshAgent.ResetPath();
        owningZombie.animator.SetFloat(movementXHash, 0);
        owningZombie.animator.SetBool(isAttackingHash, true);
        damageableTarget = owningZombie.followTarget.GetComponent<IDamageable>();
    }

    public override void IntervalUpdate()
    {
        base.IntervalUpdate();
        if (!owningZombie.followTarget)
        {
            stateMachine.ChangeState(ZombieState.Type.Idle);
            return;
        }

        damageableTarget?.TakeDamage(owningZombie.gameObject, owningZombie.damage);
    }

    public override void Update()
    {
        base.Update();
        if (!owningZombie.followTarget)
        {
            stateMachine.ChangeState(ZombieState.Type.Idle);
            return;
        }

        owningZombie.transform.LookAt(new Vector3(owningZombie.followTarget.transform.position.x, owningZombie.transform.position.y, owningZombie.followTarget.transform.position.z), Vector3.up);
        
        float distance = Vector3.Distance(owningZombie.transform.position, owningZombie.followTarget.transform.position);
        if (distance > attackRange)
        {
            stateMachine.ChangeState(ZombieState.Type.Following);
        }
    }

    public override void Exit()
    {
        base.Exit();
        owningZombie.navMeshAgent.isStopped = false;
        owningZombie.animator.SetBool(isAttackingHash, false);
    }
}
