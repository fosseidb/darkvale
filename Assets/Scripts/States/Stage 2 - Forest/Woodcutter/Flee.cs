using UnityEngine;
using UnityEngine.AI;

public class Flee : IState
{

    private readonly ZombieWoodcutter _woodcutter;
    private NavMeshAgent _agent;
    private readonly EnemyDetector _enemyDetector;
    private Animator _animator;
    private static readonly int FleeHash = Animator.StringToHash("Flee");

    private float _initialSpeed;
    private const float FLEE_SPEED = 6f;
    private const float FLEE_DISTANCE = 5f;


    public Flee(ZombieWoodcutter woodcutter, NavMeshAgent agent, EnemyDetector detector, Animator animator)
    {
        _woodcutter = woodcutter;
        _agent = agent;
        _enemyDetector = detector;
        _animator = animator;
    }

    public void OnEnter()
    {
        _agent.enabled = true;

        //consider dropping all resources
        //_woodcutter.DropOffAllWood();
        // see how here: https://www.youtube.com/watch?v=V75hgcsCGOM&t=1013s&ab_channel=JasonWeimann

        _animator.SetBool(FleeHash, true);
        _initialSpeed = _agent.speed;
        _agent.speed = FLEE_SPEED;
    }

    public void OnExit()
    {
        _agent.speed = _initialSpeed;
        _agent.enabled = false;
        _animator.SetBool(FleeHash, false);

    }

    public void Tick()
    {
        if(_agent.remainingDistance < 1f)
        {
            var away = GetRandomPoint();
            _agent.SetDestination(away);
        }
    }

    private Vector3 GetRandomPoint()
    {
        var directionFromEnemy = _woodcutter.transform.position + _enemyDetector.GetNearestEnemyPosition();
        directionFromEnemy.Normalize();

        var endPoint = _woodcutter.transform.position + (directionFromEnemy * FLEE_DISTANCE);
        if(NavMesh.SamplePosition(endPoint, out var hit, 19f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return _woodcutter.transform.position;
    }
}
