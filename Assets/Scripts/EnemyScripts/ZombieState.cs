using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieState : State
{
    public enum Type
    {
        Idle,
        Attacking,
        Following,
        Dead,
    }

    protected ZombieComponent owningZombie;
    protected readonly int movementXHash = Animator.StringToHash("MovementX");
    protected readonly int isAttackingHash = Animator.StringToHash("isAttacking");
    protected readonly int isDeadHash = Animator.StringToHash("isDead");

    public ZombieState(ZombieComponent owningZombie, ZombieStateMachine stateMachine) : base(stateMachine)
    {
        this.owningZombie = owningZombie;
    }
}
