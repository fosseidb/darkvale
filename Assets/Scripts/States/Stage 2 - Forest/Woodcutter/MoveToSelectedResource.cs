using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToSelectedResource : IState
{

    private readonly ZombieWoodcutter _woodcutter;
    private readonly NavMeshAgent _agent;
    private readonly Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed"); //reduces risk for type error to animator string

    private Vector3 _lastPosition = Vector3.zero;

    public float TimeStuck;

    public MoveToSelectedResource(ZombieWoodcutter woodcutter, NavMeshAgent agent, Animator animator)
    {
        _woodcutter = woodcutter;
        _agent = agent;
        _animator = animator;
    }


    public void OnEnter()
    {
        //reset time stuck timer
        TimeStuck = 0f;

        //ensure agent is enabled
        _agent.enabled = true;

        // set agent destination
        _agent.SetDestination(_woodcutter.Target.transform.position);

        // set animator speed
        _animator.SetFloat(Speed, 1f);

    }

    public void OnExit()
    {
        //disable agent
        _agent.enabled = false;

        //set animator speed to 0
        _animator.SetFloat(Speed, 0f);
    }

    public void Tick()
    {
        if (Vector3.Distance(_woodcutter.transform.position, _lastPosition) <= 0f)
            TimeStuck = Time.deltaTime;

        _lastPosition = _woodcutter.transform.position;
    }
}
