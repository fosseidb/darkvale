using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Merchant : MonoBehaviour, IDamagable
{
    private float _health;
    [SerializeField] private Allegiance damagableType = Allegiance.Order;

    [SerializeField] private Weapon[] weaponsForSale;

    public Allegiance DamagableType => damagableType;
    public float Health { 
        get => _health; 
        set => _health = value; 
    }
    public Weapon[] WeaponsForSale { get => weaponsForSale; set => weaponsForSale = value; }

    public void TakeDamage(float damage, PlayerProfile attacker)
    {
        Debug.Log("Merchant says hey stop it!");
    }
}
