using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    private int selectedCharacterIndex;
    private CharacterData selectedCharData;
    [SerializeField] private GameObject[] characterModels;
    [SerializeField] private CharacterData[] characterDatas;

    [SerializeField] private InGameCanvasManager inGameCanvasManager;

    private int experiencePoints;
    private int gold;
    private int level;
    [SerializeField] private int[] levelXpBands;

    public CharacterData SelectedCharData { get => selectedCharData; }

    // Start is called before the first frame update
    void Start()
    {
        selectedCharacterIndex = PlayerPrefs.GetInt("characterIndex");
        SetCharacter(selectedCharacterIndex);
        level = 0;
    }

    private void SetCharacter(int index)
    {
        //sets the character model
        //for (int i = 0; i < characterModels.Length; i++)
        //{
        //    characterModels[i].SetActive(index == i);
        //}

        //sets the character data
        selectedCharData = characterDatas[index];
    }

    public Allegiance GetAllegiance()
    {
        return selectedCharData.allegiance;
    }

    public void EarnXP(int xPoints)
    {
        experiencePoints += xPoints;
        inGameCanvasManager.UpdateXP(level, experiencePoints);
    }

    public void AdjustGold(int goldIncrement)
    {
        gold += goldIncrement;
        inGameCanvasManager.UpdateGold(gold);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AreaZoneTrigger")
        {
            inGameCanvasManager.UpdateMapLocation(other.GetComponent<AreaZoneTrigger>().LocationName);
        }
        
    }
}
