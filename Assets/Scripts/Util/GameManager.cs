using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    //Master
    [Header("Master State Machine")]

    private StateMachine _stateMachine;

    public float MasterTimer { get; set; }
    public float StageTimer { get; set; }

    [Header("Stage Scenery Groups")]
    [SerializeField] GameObject[] PreStageSceneryGroups;
    [SerializeField] GameObject[] LiveStageSceneryGroups;
    [SerializeField] GameObject[] PostStageSceneryGroups;

    //Stage 1 - cemetary
    [Header("Stage 1: The Cemetary")]
    //[SerializeField] float stage1DurationInMinutes = 15f;
    public TMP_Text Text_Stage1CountdownTimer;
    [SerializeField] float _corruptionThreshold = 100f;

    private float _corruptionLevel = 0f;
    private int necroCount;
    
    
    //Stage 2 - woods
    [Header("Stage 2: The Woods")]
    private float _collectedWoodAmount;
    [SerializeField] float _TotalWoodNeeded = 100f;
    private int cutterCount;

    //Stage 3 - Battlefield
    [Header("Stage 3: Battlefield")]
    private int waypointIndex;
    private bool _gatesReached = false;

    //Stage 3 - Village Proper
    [Header("Stage 4: Village Proper")]
    private int orderTickets = 100;

    #region Delegates
    public delegate void onStageChangeDelegate (int i);
    public event onStageChangeDelegate StageChangeEvent;

    public delegate void onTimersChangeDelegate(int stageNr, float stageTime, float masterTime);
    public event onTimersChangeDelegate NewUITimes;

    // PT1 - Cemetary
    public delegate void onCorruptionChangeDelegate(float corruptionLevel);
    public event onCorruptionChangeDelegate CorruptionLevelChangeEvent;

    public delegate void onNecromancerNumberChangeDelegate(int noAliveNecros);
    public event onNecromancerNumberChangeDelegate NecromancerNumberChangeEvent;

    //PT2 - Forest
    public delegate void onDroppedOffWoodDelegate(float collectedWoodLevel);
    public event onDroppedOffWoodDelegate WoodLevelChangeEvent;

    public delegate void onWoodcutterNumberChangeDelegate(int noAliveNecros);
    public event onWoodcutterNumberChangeDelegate WoodcutterNumberChangeEvent;

    //PT3 - Siege
    public delegate void onWaypointReachedChangeDelegate(int waypointIndex, bool backwards);
    public event onWaypointReachedChangeDelegate WaypointReachedChangeEvent;

    public delegate void onWaypointDistanceChangeDelegate(float waypointDistance);
    public event onWaypointDistanceChangeDelegate WaypointDistanceChangeEvent;

    public delegate void onTotalDistanceToGateCalculatedDelegate(float totalDistanceToGate);
    public event onTotalDistanceToGateCalculatedDelegate TotalDistanceToGateCalculatedEvent;
    

    //PT4 - Assault
    public delegate void onOrderTicketLostDelegate();
    public event onOrderTicketLostDelegate OrderTicketLostEvent;

    public delegate void onCityLostDelegate();
    public event onCityLostDelegate CityLostEvent;

    #endregion

    #region Main State Machine Methods

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new StateMachine();

        //create our states
        var cemetary = new CemetaryStage(this);
        var forest = new ForestStage(this);
        var siege = new SiegeStage(this);
        var assault = new AssaultStage(this);
        var orderWon = new OrderWon(this);
        var destructionWon = new DestructionWon(this);

        // connect our transitions based on conditions
        At(cemetary, forest, CemetaryIsCorrupted());
        At(cemetary, orderWon, CemetaryStageTimeOut());
        At(forest, siege, WoodIsGathered());
        At(forest, orderWon, ForestStageTimeOut());
        At(siege, assault, GatesReached());
        At(siege, orderWon, SiegeStageTimeOut());
        At(assault, orderWon, AssaultStageTimeOut());
        At(assault, destructionWon, CityLost());

        //connect "From Any" states

        //Set initial State
        _stateMachine.SetState(cemetary);


        void At(IState from, IState to, Func<bool> condition) =>
            _stateMachine.AddTransition(from, to, condition);

        //condition functions
        Func<bool> CemetaryIsCorrupted() => () => _corruptionLevel >= _corruptionThreshold;
        Func<bool> WoodIsGathered() => () => _collectedWoodAmount >= _TotalWoodNeeded;
        Func<bool> GatesReached() => () => _gatesReached;
        Func<bool> CityLost() => () => orderTickets <= 0;
        Func<bool> CemetaryStageTimeOut() => () => StageTimer <= 0f ;
        Func<bool> ForestStageTimeOut() => () => StageTimer <= 0f;
        Func<bool> SiegeStageTimeOut() => () => StageTimer <= 0f;
        Func<bool> AssaultStageTimeOut() => () => StageTimer <= 0f;
    }

    // Update is called once per frame
    void Update() => _stateMachine.Tick();

    public void InvokeNewStage(int stage)
    {
        Debug.Log("In GM Stage changed to:" + stage);
        StageChangeEvent?.Invoke(stage);
    }

    public void UpdateUITimers(int stageNr)
    {
        NewUITimes?.Invoke(stageNr, StageTimer, MasterTimer);
    }

    public void SetLiveStage(int stageNr)
    {
        PreStageSceneryGroups[stageNr-1].SetActive(false);
        LiveStageSceneryGroups[stageNr-1].SetActive(true);
    }

    public void SetPostStage(int stageNr)
    {
        LiveStageSceneryGroups[stageNr-1].SetActive(false);
        PostStageSceneryGroups[stageNr-1].SetActive(true);
    }

    #endregion

    #region Stage 1 Methods
    public void SubscribeToNecromancer(NPC_Necromancer necro)
    {
        //subscribe to all Necromancers
        necro.CastSummonEvent += OnSummonCast;
        necro.NecromancerDeathEvent += OnNecroDeath;

        //increment necro counter
        necroCount++;

        // Notify UI
        NecromancerNumberChangeEvent?.Invoke(necroCount);
    }

    private void OnSummonCast()
    {
        _corruptionLevel += 4f;
        
        // Notify UI
        CorruptionLevelChangeEvent?.Invoke(_corruptionLevel);
    }

    private void OnNecroDeath(NPC_Necromancer necro)
    {
        necro.CastSummonEvent -= OnSummonCast;
        necro.NecromancerDeathEvent -= OnNecroDeath;

        //increment necro counter
        necroCount--;

        // Notify UI
        NecromancerNumberChangeEvent?.Invoke(necroCount);
    }

    //public void KillNecro()
    //{
    //    FindObjectOfType<NPC_Necromancer>().TakeDamage(999, null);
    //}
    #endregion

    #region Stage 2 Methods
    public void SubscribeToWoodcutter(ZombieWoodcutter cutter)
    {
        //subscribe to all woodcutters
        cutter.WoodcutterDeathEvent += OnCutterDeath;

        //increment necro counter
        cutterCount++;

        // Notify UI
        WoodcutterNumberChangeEvent?.Invoke(cutterCount);
    }

    private void OnCutterDeath(ZombieWoodcutter cutter)
    {
        cutter.WoodcutterDeathEvent -= OnCutterDeath;

        //increment necro counter
        cutterCount--;

        // Notify UI
        WoodcutterNumberChangeEvent?.Invoke(cutterCount);
    }

    public void UpdateWoodPile(int amount)
    {
        _collectedWoodAmount = amount;

        // Notify UI
        WoodLevelChangeEvent?.Invoke(_collectedWoodAmount);
    }

    public void KillCutter()
    {
        FindObjectOfType<ZombieWoodcutter>().TakeDamage(999, null);
    }

    #endregion

    #region Stage 3 Methods
    public void GetTotalDistanceToGate(float totalDistance)
    {
        TotalDistanceToGateCalculatedEvent?.Invoke(totalDistance);
    }

    public void UpdateWaypointReached(int waypointIndex, bool backwards)
    {
        this.waypointIndex = waypointIndex;
        WaypointReachedChangeEvent?.Invoke(waypointIndex, backwards);
    }

    public void FinalWaypointReached()
    {
        Debug.Log("Battering ram arrived at gate");

        _gatesReached = true;
    }

    public void UpdateRamDistanceUI(float distanceValue)
    {
        WaypointDistanceChangeEvent?.Invoke(distanceValue);
    }
    #endregion

    #region Stage 4 Methods
    public void GameEnded()
    {
        Debug.Log("Game Ended;");
    }

    #endregion

}
