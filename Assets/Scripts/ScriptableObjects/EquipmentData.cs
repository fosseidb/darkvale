using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Equipment")]
public class EquipmentData : ScriptableObject
{

    public string equipmentName;
    public Sprite equipmentSprite;
    public string equipmentDescription;
    public int equipmentValue;
    public int equipmentWeight;
    public float useSpeed;
    public EquipmentType type;

    public int damage;


    public enum EquipmentType
    {
        Head,
        Torso,
        Arms,
        Legs,
        Feet,
        MainHand,
        OffHand,
        Shield,
        RangedBow,
        RangedRifle,
        RangedPistol
    }
}
