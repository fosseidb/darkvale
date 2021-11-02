using UnityEngine;
using UnityEngine.AI;

public class ReturnToStockpile : IState
{
    private readonly ZombieWoodcutter _woodcutter;
    private readonly NavMeshAgent _agent;
    private readonly Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    public ReturnToStockpile(ZombieWoodcutter woodcutter, NavMeshAgent agent, Animator anim)
    {
        _woodcutter = woodcutter;
        _agent = agent;
        _animator = anim;
    }

    public void OnEnter()
    {
        _woodcutter.Stockpile = Object.FindObjectOfType<GreaterWoodpile>();
        _agent.enabled = true;
        _agent.SetDestination(_woodcutter.Stockpile.transform.position);
        _animator.SetFloat(Speed, 1f);

    }

    public void OnExit()
    {
        _agent.enabled = false;
        _animator.SetFloat(Speed, 0f);
    }

    public void Tick()
    {
    }
}
