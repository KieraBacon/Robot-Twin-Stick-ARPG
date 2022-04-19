using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealthComponent : HealthComponent
{
    private ZombieStateMachine zombieStateMachine;
    private ZombieComponent zombieComponent;

    private void Awake()
    {
        zombieStateMachine = GetComponent<ZombieStateMachine>();
        zombieComponent = GetComponent<ZombieComponent>();
    }

    public override void Destroy()
    {
        base.Destroy();
        zombieStateMachine.ChangeState(ZombieState.Type.Dead);
    }

    public override void TakeDamage(GameObject damageDealer, float value)
    {
        base.TakeDamage(damageDealer, value);
        if (CurrentHealth > 0 && !zombieComponent.followTarget)
        {
            zombieComponent.followTarget = damageDealer;
            zombieStateMachine.ChangeState(ZombieState.Type.Following);
        }
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        if (CurrentHealth > 0)
            zombieStateMachine.ChangeState(ZombieState.Type.Idle);
    }
}
