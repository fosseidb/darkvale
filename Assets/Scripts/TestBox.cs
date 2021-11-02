using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour, IDamagable
{

    public Allegiance damageType;
    public float health; 

    public Allegiance DamagableType => damageType;

    public float Health {
        get => health;
        set => health = value;
    }

    public void TakeDamage(float damage, PlayerProfile attacker)
    {
        Debug.Log("Box takes {damage} damage.");
        Debug.Log(health - damage + " life left");
    }
}
