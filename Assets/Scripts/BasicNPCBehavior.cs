using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BasicNPCBehavior : MonoBehaviour
{
    [Range(0,1)]
    public float hungerRate = 0.001f;

    [Range(0, 1)]
    public float sleepRate = 0.001f;
    
    [Range(0, 1)]
    public float workRate = 0.001f;


    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    Animator animator;

    bool isMoving = false;
    bool isIdle = true;

    float needSleep = 0.0f;
    float needFood = 0.0f;
    float needWork = 0.0f;
    int goingToNeedType = -1;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsMoving", isMoving);

        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        //GotoNextPoint();
    }


    void Update()
    {

        IncrementNeeds();
        if (isIdle)
            CheckMostPressingNeed();


        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoIdle(goingToNeedType);

        animator.SetBool("IsMoving", isMoving);
        
    }

    private void CheckMostPressingNeed()
    {
        //priority food, sleep, work

        //check food
        if (needFood > 1f)
            //needs to food
            GoToNeed(1);

        //check sleep
        else if (needSleep > 1f)
            //needs to sleep
            GoToNeed(0);

        //check work
        else if (needWork > 1f)
            GoToNeed(2);

        //go idle
        else
            GoIdle(-1);
    }

    private void GoIdle(int need)
    {
        isIdle = true;
        isMoving = false;

        if (need == 0)
            needSleep = 0f;
        if (need == 1)
            needFood = 0f;
        if (need == 2)
            needWork = 0f;
    }

    private void GoToNeed(int needType)
    {
        if (!isMoving)
            isMoving = true;

        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[needType].position;

        goingToNeedType = needType;

    }

    private void IncrementNeeds()
    {
        needSleep += sleepRate;
        needFood += hungerRate;
        needWork += workRate;
    }



    void GotoNextPoint()
    {
        if (!isMoving)
            isMoving = true;

        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

}