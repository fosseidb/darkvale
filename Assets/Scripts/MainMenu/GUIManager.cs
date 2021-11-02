using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{

    //CHARACTER SELECTION
    [Header("Character Selection")]
    [SerializeField] private TMP_Text charName;
    [SerializeField] private TMP_Text charDescription;

    [SerializeField] private TMP_Text equipmentTitle;
    [SerializeField] private TMP_Text equipmentDescription;
    [SerializeField] private Image equipmentImage;

    [SerializeField] private TMP_Text skill1Title;
    [SerializeField] private TMP_Text skill1Description;
    [SerializeField] private Image skill1Image;

    [SerializeField] private TMP_Text skill2Title;
    [SerializeField] private TMP_Text skill2Description;
    [SerializeField] private Image skill2Image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateActiveCharacter(CharacterData data)
    {
        charName.text = data.charName.ToString();
        charDescription.text = data.charDescription.ToString();

        equipmentTitle.text = data.starterEquipment[0].equipmentName;
        equipmentDescription.text = data.starterEquipment[0].equipmentDescription; ;
        equipmentImage.sprite = data.starterEquipment[0].equipmentSprite;

        skill1Title.text = data.starterSkills[0].skillName;
        skill1Description.text = data.starterSkills[0].skillDescription; ;
        skill1Image.sprite = data.starterSkills[0].skillSprite;

        skill2Title.text = data.starterSkills[1].skillName;
        skill2Description.text = data.starterSkills[1].skillDescription; ;
        skill2Image.sprite = data.starterSkills[1].skillSprite;
    }
}
