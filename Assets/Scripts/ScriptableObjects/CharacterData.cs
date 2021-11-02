using UnityEngine;

[CreateAssetMenu(fileName ="Character", menuName ="ScriptableObjects/Character")]
public class CharacterData : ScriptableObject
{
    public Sprite charIcon;
    public string charName;
    public string charDescription;
    public float armor;
    public float health;
    public float speed;

    public Allegiance allegiance;

    public SkillData[] starterSkills;
    public EquipmentData[] starterEquipment;
}
