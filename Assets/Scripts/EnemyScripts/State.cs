using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected ZombieStateMachine stateMachine;
    public float updateInterval { get; protected set; } = 1f;

    public State(ZombieStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void IntervalUpdate()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }
}
