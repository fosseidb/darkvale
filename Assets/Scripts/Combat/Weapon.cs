using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public EquipmentData data;
    public PlayerProfile wielder;

    public int GetDamage()
    {
        return data.damage;
    }

    public void SetWielder(PlayerProfile w)
    {
        wielder = w;
    }
}
