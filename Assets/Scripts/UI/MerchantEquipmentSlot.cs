using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MerchantEquipmentSlot: MonoBehaviour
{
    // Equipment
    Weapon weapon;

    //Main card
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text itemTitle, itemValue;
    [SerializeField] GameObject leftPanel, rightPanel;

    //Hint Card
    //TODO

    public void PopulateMerchantEquipmentSlot(Weapon weapon)
    {
        this.weapon = weapon;

        itemImage.sprite = weapon.data.equipmentSprite;
        itemTitle.text = weapon.data.equipmentName;
        itemValue.text = weapon.data.equipmentValue.ToString();

        leftPanel.SetActive(true);
        rightPanel.SetActive(true);
    }

    public void ClearSlot()
    {
        weapon = null;

        leftPanel.SetActive(false);
        rightPanel.SetActive(false);
    }
}
