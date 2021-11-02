using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHitbox : MonoBehaviour
{
    bool lethal;

    Allegiance wielderAllegiance;
    Weapon weapon;

    public bool Lethal { get => lethal; set => lethal = value; }

    private void Start()
    {
        weapon = GetComponent<Weapon>();
        wielderAllegiance = weapon.wielder.GetAllegiance();
        Lethal = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Lethal)
            return;

        Debug.Log("Weapon triggered with " + other.name);
        if(wielderAllegiance == Allegiance.Order)
        {
            if (other.GetComponent<IDamagable>() == null)
                Debug.Log("No Damagable.");
            else if(other.GetComponent<IDamagable>() != null && (other.GetComponent<IDamagable>().DamagableType == Allegiance.DestructableObject || other.GetComponent<IDamagable>().DamagableType == Allegiance.Destruction))
            {
                Debug.Log("Will Trigger TakeDMG");
                other.GetComponent<IDamagable>().TakeDamage(weapon.GetDamage(), weapon.wielder);
            }
            else
            {
                Debug.Log("Same Team! No DMG");
            }
        } 
        else if (wielderAllegiance == Allegiance.Destruction)
        {
            if (other.GetComponent<IDamagable>() != null && (other.GetComponent<IDamagable>().DamagableType == Allegiance.DestructableObject || other.GetComponent<IDamagable>().DamagableType == Allegiance.Order))
            {
                Debug.Log("Will Trigger TakeDMG 2");
                other.GetComponent<IDamagable>().TakeDamage(weapon.GetDamage(), weapon.wielder);
            }
            else
            {
                Debug.Log("Same Team! No DMG");
            }
        } 
    }
}
