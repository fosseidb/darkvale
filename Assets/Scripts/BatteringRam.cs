using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Payloads such as Battering Rams don’t move by themselves! 
/// Players need to be close to them in order for them to start moving. 
/// The more players that are near the payload, the faster it moves. 
/// In DV, the fastest move speed involves three or more players, so you’ll want to have three near it while the rest of your team is spread out and clearing the way for the payload to advance. 
/// Attacking teams have a set time limit to reach each checkpoint but when you do, a set amount of time is added to the clock.
/// For defenders, if you happen to wipe out the opposing team, you can stand near the payload and gradually push it backwards forcing the attacking team to do more work to make up any lost ground. 
/// However, the movement speed is significantly slower compared to when attackers are pushing.
/// </summary>


[RequireComponent(typeof(NavMeshAgent))]
public class BatteringRam : MonoBehaviour
{

    NavMeshAgent agent;
    GameManager gameManager;
    
    [Header("Navigation")]
    public bool setupMode; // flag to set if the ram is setting up from spawn point to start point (i.e rolling out of the workshop onto playing field).
    [SerializeField] Transform spawnPosition; // permanent start position.
    [SerializeField] Transform[] waypoints;
    private float[] waypointDistances;
    private float totalDistanceToGate;
    [SerializeField] float baseSpeed, destructionPlayerSpeedMultiplier, orderPlayerSpeedMultiplier;
    
    bool goingBackwards, changeDirFlag;
    int nextWaypoint;
    int noOrderPlayers, noDestructionPlayers;


    [Header("Sensor")]
    [SerializeField] Collider playersNearbySensor;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;

        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        goingBackwards = false;
        changeDirFlag = false;
        setupMode = true;

        agent.Warp(spawnPosition.position);

        nextWaypoint = 0;
        
        totalDistanceToGate = GenerateDistanceToGates();
        gameManager.GetTotalDistanceToGate(totalDistanceToGate);
        
        SetNextWaypoint(nextWaypoint);

    }

    // Update is called once per frame
    void Update()
    {

        //if in setupMode we override the speed to ensure that the ram arrives at the start pos
        if (setupMode)
            agent.speed = baseSpeed * 3 * orderPlayerSpeedMultiplier;
        
        //otherwise normal mode
        else
        {
            //check status of players around
            //if both or none around -> speed = 0
            if (noOrderPlayers > 0 && noDestructionPlayers > 0 || noOrderPlayers <= 0 && noDestructionPlayers <= 0)
                agent.speed = 0f;

            //if only dest. around move forward with multiple speed
            else if (noOrderPlayers <= 0 && noDestructionPlayers > 0)
            {
                changeDirFlag = (goingBackwards == true); //triggers true when the condition turns the direction
                goingBackwards = false;
                agent.speed = baseSpeed * noDestructionPlayers * destructionPlayerSpeedMultiplier;
            }
            //if only order around move forward with fractal speed
            else if (noOrderPlayers > 0 && noDestructionPlayers <= 0)
            {
                changeDirFlag = (goingBackwards == false);
                goingBackwards = true;
                agent.speed = baseSpeed * noOrderPlayers * orderPlayerSpeedMultiplier;
            }
        }

        // if changing direction
        if (changeDirFlag)
        {
            if (goingBackwards)
            {
                DecrementWaypoint();
            }
            else
            {
                IncrementWaypoint();
            }

            //set next waypoint
            SetNextWaypoint(nextWaypoint);

        }

        // Check if we've reached the destination
        //path is ready
        if (!agent.pathPending)
        {
            //we are on waypoint
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (nextWaypoint == 0 && setupMode)
                    setupMode = false; //arrived at start, leave setup mode.

                //if we havent set the path yet, or our speed is 0
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Battering ram reached a waypoint.");

                    //TODO
                    Debug.Log("PING Game Manager of location");

                    if (goingBackwards)
                    {
                        DecrementWaypoint();
                    }
                    else
                    {
                        IncrementWaypoint();
                    }

                    //set next waypoint
                    SetNextWaypoint(nextWaypoint);
                }
            }
        }

        // update GameManager about position
        if(!setupMode)
            gameManager.UpdateRamDistanceUI(UpdateBatteringRamDistanceUI());
    }

    private void SetNextWaypoint(int index)
    {
        agent.destination = waypoints[index].position;
        changeDirFlag = false;
    }

    private void DecrementWaypoint()
    {
        if (nextWaypoint >= 1)
        {
            gameManager.UpdateWaypointReached(nextWaypoint, goingBackwards);
            nextWaypoint--;
        }
        else
            Debug.Log("Battering ram back to start pos");
    }

    private void IncrementWaypoint()
    {
        if (waypoints.Length-1 > nextWaypoint)
        {
            gameManager.UpdateWaypointReached(nextWaypoint, goingBackwards);
            nextWaypoint++;
        }
        else
        {
            gameManager.FinalWaypointReached();
        }
    }

    private float GenerateDistanceToGates()
    {
        float distance = 0f;
        waypointDistances = new float[waypoints.Length-1];

        for (int i = 0; i < waypoints.Length-1; i++)
        {
            float increment = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            waypointDistances[i] = increment;
            Debug.Log("Increment" + i + ":"+ increment);
            distance += increment;
        }
        Debug.Log("Total Distance: " + distance);
        return distance;
    }

    public float UpdateBatteringRamDistanceUI()
    {
        float distancetravelled = 0f;
        for (int i = 0; i < waypointDistances.Length; i++)
        {
            Debug.Log("waypointDistance index: " + i +", nextWaypoint=" + nextWaypoint);
            if (i < nextWaypoint-1)
            {
                distancetravelled += waypointDistances[i];
                Debug.Log("Incremented distance travelled= "+ distancetravelled);
            }
            if(i == nextWaypoint-1)
            {
                if(goingBackwards)
                    distancetravelled += Vector3.Distance(this.transform.position, waypoints[i].position);
                else
                    distancetravelled += Vector3.Distance(this.transform.position, waypoints[i].position);
                Debug.Log("Partial waypoint distance travelled:" + distancetravelled);
            }
        }
        Debug.Log("returning total distance travelled: "+  distancetravelled);
        return distancetravelled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (setupMode)
            return;

        if (other.GetComponent<PlayerProfile>() == null)
            return;

        if (other.GetComponent<PlayerProfile>().GetAllegiance() == Allegiance.Order)
            noOrderPlayers++;

        if (other.GetComponent<PlayerProfile>().GetAllegiance() == Allegiance.Destruction)
            noDestructionPlayers++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (setupMode)
            return;

        if (other.GetComponent<PlayerProfile>() == null)
            return;

        if (other.GetComponent<PlayerProfile>().GetAllegiance() == Allegiance.Order)
            Math.Max(0, noOrderPlayers--);
        if (other.GetComponent<PlayerProfile>().GetAllegiance() == Allegiance.Destruction)
            Math.Max(0, noDestructionPlayers--);
    }
}
