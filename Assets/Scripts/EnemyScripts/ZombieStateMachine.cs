using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStateMachine : MonoBehaviour
{
    public State currentState { get; private set; }
    protected Dictionary<ZombieState.Type, State> states = new Dictionary<ZombieState.Type, State>();
    bool isRunning;
    
    public void Initialize(ZombieState.Type startingState)
    {
        if (states.ContainsKey(startingState))
        {
            ChangeState(startingState);
        }
    }

    public void AddState(ZombieState.Type stateType, State state)
    {
        if (states.ContainsKey(stateType)) return;
        states.Add(stateType, state);
    }

    public void RemoveState(ZombieState.Type stateType)
    {
        if (!states.ContainsKey(stateType)) return;
        states.Remove(stateType);
    }

    public void ChangeState(ZombieState.Type nextState)
    {
        if (isRunning)
        {
            StopRunningState();
        }
        if (!states.ContainsKey(nextState)) return;
        currentState = states[nextState];
        currentState.Enter();

        if (currentState.updateInterval > 0)
        {
            InvokeRepeating(nameof(IntervalUpdate), currentState.updateInterval, currentState.updateInterval);
        }
        isRunning = true;
    }

    private void StopRunningState()
    {
        isRunning = false;
        currentState.Exit();
        CancelInvoke(nameof(IntervalUpdate));
    }

    private void IntervalUpdate()
    {
        if (isRunning)
        {
            currentState.IntervalUpdate();
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            currentState.Update();
        }
    }
}
