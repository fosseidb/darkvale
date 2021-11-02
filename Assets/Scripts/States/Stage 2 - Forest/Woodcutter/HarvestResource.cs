using UnityEngine;

public class HarvestResource : IState
{
    private readonly ZombieWoodcutter _woodcutter;
    private readonly Animator _animator;
    private float _resourcesPerSecond =0.5f;

    private float _nextTakeResourceTime;
    private static readonly int Harvest = Animator.StringToHash("Harvest");

    public HarvestResource(ZombieWoodcutter woodcutter, Animator animator)
    {
        _woodcutter = woodcutter;
        _animator = animator;
    }

    public void OnEnter()
    {
        if (_woodcutter.Target != null)
            _woodcutter.RotateTowards(_woodcutter.Target.transform);
    }

    public void OnExit()
    {    }

    public void Tick()
    {
        if (_woodcutter.Target != null)
        {
            if(_nextTakeResourceTime <= Time.time)
            {
                _nextTakeResourceTime = Time.time + (1f / _resourcesPerSecond);
                _woodcutter.TakeFromTarget();
                _animator.SetTrigger(Harvest);
            }
        }
    }


}
