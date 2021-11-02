using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC_Necromancer : MonoBehaviour, IDamagable
{

    public Transform summoningCrystal;

    public delegate void castSummonDelegate();
    public event castSummonDelegate CastSummonEvent;
    
    public delegate void onNecromancerDeathDelegate(NPC_Necromancer n);
    public event onNecromancerDeathDelegate NecromancerDeathEvent;

    [SerializeField]    private float maxHealth = 10f;
    private float health = 0f;

    [SerializeField]    private float rotationSpeed = 10f;

    private bool isSummoning = false; 
    [SerializeField] private float summonCastTime = 3f;

    private float castTimer = 0f;
    private Transform summoningPoint;
    private int necroID = -1;

    NavMeshAgent agent;
    Animator animator;

    public int NecroID { get => necroID; set => necroID = value; }

    private Allegiance damagableType = Allegiance.Destruction;
    public Allegiance DamagableType => damagableType;
    public float Health { 
        get => health;
        set => health = value;
    }

    [SerializeField] private Slider healthbar;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpStats();

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;

        animator = GetComponentInChildren<Animator>();
    }

    public void SetUpStats()
    {
        health = maxHealth;
        healthbar.maxValue = maxHealth;
        healthbar.value = maxHealth;
    }

    private void Update()
    {
        float dist = Vector3.Distance(summoningPoint.position, transform.position);

        if (!agent.pathPending && dist < 0.5f)
        {
            RotateTowards(summoningCrystal);
            isSummoning = true;
            animator.SetBool("IsCasting", true);
        }

        if (isSummoning)
        {
            castTimer -= Time.deltaTime;
            if (castTimer <= 0f) castSummonSpell();
        } 
    }
    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void castSummonSpell()
    {
        castTimer = summonCastTime;

        // cast summonspell
        animator.SetTrigger("CastSummon");

        //notify GameManager
        CastSummonEvent?.Invoke();

        // fire particle effect
    }

    public void SetDestinationSummonPoint(Transform summoningPoint)
    {
        agent.enabled = true;
        this.summoningPoint = summoningPoint;
        if (!agent)
            Debug.Log("no agent found");

        agent.SetDestination(summoningPoint.position);
    }


    public void TakeDamage(float damage, PlayerProfile attacker)
    {
        if (health <= 0f) return;

        Debug.Log("Necromancer says ouch!");
        health -= damage;
        UpdateUI();
        if (health <= 0f)
        {
            Die();
            if(attacker != null)
            {
                attacker.AdjustGold(10);
                //attacker.EarnXP(15);
            }
        } else
            animator.SetTrigger("TakesDamage");
    }

    private void UpdateUI()
    {
        healthbar.value = health;
    }

    void Die()
    {
        //notify GameManager
        NecromancerDeathEvent?.Invoke(this);

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
}
