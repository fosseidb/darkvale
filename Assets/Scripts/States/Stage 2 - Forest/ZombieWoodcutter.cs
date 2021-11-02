using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieWoodcutter : MonoBehaviour, IDamagable
{


    [SerializeField] private float _maxHealth = 10f;
    private float _currentHealth = 0f;
    [SerializeField] private int _maxCarry = 20;
    private int _carryingAmount = 0;

    NavMeshAgent agent;
    Animator animator;
    EnemyDetector enemyDetector;
    ParticleSystem takeDamageParticleSystem;
    [SerializeField] private float _rotationSpeed = 10f;

    private StateMachine _stateMachine;

    public AncientTree Target { get; set; }
    public GreaterWoodpile Stockpile { get; set; }

    public Allegiance DamagableType => Allegiance.Destruction;

    public float Health { get => _currentHealth; set => _currentHealth = value; }

    // DELEGATES //
    public delegate void onWoodcutterDeathDelegate(ZombieWoodcutter woodcutter);
    public event onWoodcutterDeathDelegate WoodcutterDeathEvent;

    // Start is called before the first frame update
    void Awake()
    {
        _currentHealth = _maxHealth;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemyDetector = GetComponent<EnemyDetector>();
        takeDamageParticleSystem = GetComponent<ParticleSystem>();

        _stateMachine = new StateMachine();

        //create our states
        var search = new SearchForResource(this);
        var moveToSelected = new MoveToSelectedResource(this, agent, animator);
        var harvest = new HarvestResource(this, animator);
        var returnToStockpile = new ReturnToStockpile(this, agent, animator);
        var placeResourcesInStockpile = new PlaceResourcesInStockpile(this, animator);
        var flee = new Flee(this, agent, enemyDetector, animator);

        // connect our transitions based on conditions
        At(search, moveToSelected, HasTarget());
        At(moveToSelected, search, StuckForOverASecond());
        At(moveToSelected, harvest, ReachedResource());
        At(harvest, search, TargetIsDepletedAndICanCarryMore());
        At(harvest, returnToStockpile, InventoryFull());
        At(returnToStockpile, placeResourcesInStockpile, ReachedStockpile());
        At(placeResourcesInStockpile, search, () => _carryingAmount == 0);

        //Add "From Any"-States
        _stateMachine.AddAnyTransition(flee, () => enemyDetector.EnemyInRange);
        At(flee, search, () => enemyDetector.EnemyInRange == false);

        //Set initial state
        _stateMachine.SetState(search);

        void At(IState from, IState to, Func<bool> condition) =>
            _stateMachine.AddTransition(from, to, condition);

        // Condition Functions
        Func<bool> HasTarget() => () => Target != null;
        Func<bool> StuckForOverASecond() => () => moveToSelected.TimeStuck > 1f;
        Func<bool> ReachedResource() => () => Target != null 
            && Vector3.Distance(transform.position, Target.transform.position) <= 2f;
        Func<bool> TargetIsDepletedAndICanCarryMore() => () => (Target == null || Target.IsDepleted) && !InventoryFull().Invoke();
        Func<bool> InventoryFull() => () => _carryingAmount >= _maxCarry;
        Func<bool> ReachedStockpile() => () => Stockpile != null && Vector3.Distance(transform.position, Stockpile.transform.position) <= 1f;
    }

    public void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
    }

    private void Update() => _stateMachine.Tick();

    public void TakeFromTarget()
    {
        if (Target.Take())
        {
            _carryingAmount++;
        }
    }

    public bool DropOffWood()
    {
        if (_carryingAmount <= 0)
            return false;

        _carryingAmount--;

        return true;
    }

    void Die()
    {
        //notify GameManager
        WoodcutterDeathEvent?.Invoke(this);

        //stop movement
        agent.isStopped = true;

        // animate death
        animator.SetTrigger("Dies");

        //prep decay
        StartCoroutine(DecayCharacter());
    }

    private IEnumerator DecayCharacter()
    {
        //wiat for animation
        yield return new WaitForSeconds(3f);
        agent.enabled = false;

        for (float ft = 8f; ft >= 0; ft -= 0.02f)
        {
            transform.Translate(-Vector3.up * .4f * Time.deltaTime);

            yield return null;
        }

        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage, PlayerProfile attacker)
    {
        _currentHealth -= damage;
        animator.SetTrigger("TakesDamage");
        if (_currentHealth < 0f)
        {
            Die();
        }
    }
}
