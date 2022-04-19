using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieComponent : MonoBehaviour
{
    public int damage = 5;
    private NavMeshAgent _navMeshAgent;
    public NavMeshAgent navMeshAgent => _navMeshAgent;
    private Animator _animator;
    public Animator animator => _animator;
    private ZombieStateMachine _stateMachine;
    public ZombieStateMachine stateMachine => _stateMachine;
    public GameObject followTarget;
    public TargetDetector targetDetector;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _stateMachine = GetComponent<ZombieStateMachine>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        ZombieState_Idle idleState = new ZombieState_Idle(this, stateMachine);
        stateMachine.AddState(ZombieState.Type.Idle, idleState);

        ZombieState_Dead deadState = new ZombieState_Dead(this, stateMachine);
        stateMachine.AddState(ZombieState.Type.Dead, deadState);
        
        ZombieState_Following followState = new ZombieState_Following(this, stateMachine);
        stateMachine.AddState(ZombieState.Type.Following, followState);
     
        ZombieState_Attacking attackState = new ZombieState_Attacking(this, stateMachine);
        stateMachine.AddState(ZombieState.Type.Attacking, attackState);

        stateMachine.Initialize(ZombieState.Type.Idle);
    }
}
