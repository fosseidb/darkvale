using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InGameCanvasManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject player;
    PlayerProfile playerProfile;

    //Panels
    [Header("Navigation: Compass & Minimap")]
    [SerializeField] Compass compass;
    [SerializeField] TMP_Text Text_masterTimer;
    [SerializeField] TMP_Text Text_location;
    [SerializeField] MinimapCameraController minimapCameraController;
    private Camera cam;

    [Header("Stage Info Panel")]
    [SerializeField] GameObject StageInfoPanel;
    [SerializeField] TMP_Text Text_ChapterText;
    [SerializeField] TMP_Text Text_StageText;
    
    [Header("Stage1 Panel")]
    [SerializeField] GameObject Stage1Panel;
    [SerializeField] TMP_Text Text_stage1Timer;
    [SerializeField] Slider _slider;
    [SerializeField] TMP_Text Text_sliderValue;
    [SerializeField] TMP_Text Text_noLeft;

    [Header("Stage2 Panel")]
    [SerializeField] GameObject Stage2Panel;
    [SerializeField] TMP_Text Text_stage2Timer;
    [SerializeField] Slider _woodSlider;
    [SerializeField] TMP_Text Text_sliderWoodValue;
    [SerializeField] TMP_Text Text_noOfWCsLeft;

    [Header("Stage3 Panel")]
    [SerializeField] GameObject Stage3Panel;
    [SerializeField] TMP_Text Text_stage3Timer;
    [SerializeField] Slider _RamSlider;
    [SerializeField] TMP_Text Text_sliderDistanceValue;
    [SerializeField] Image[] waypointIconHolder;
    [SerializeField] Color orderColor;
    [SerializeField] Color destructionColor;

    [Header("Stage4 Panel")]
    [SerializeField] GameObject Stage4Panel;
    [SerializeField] TMP_Text Text_stage4Timer;
    [SerializeField] TMP_Text Text_OrderTicketsLeftValue;

    private TMP_Text[] _stageTimers;


    [Header("Character Status")]
    [SerializeField] Image Img_CharIcon;
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text healthValueText;
    [SerializeField] Slider armorSlider;
    [SerializeField] TMP_Text armorValueText;
    [SerializeField] Slider xpSlider;

    [Header("Skills")]
    [SerializeField] Image skill1;
    //skills here

    [Header("Weapons")]
    //weaps here
    [SerializeField] Image weapon1;

    [Header("MerchantWindow")]
    [SerializeField] GameObject merchantWindow;
    [SerializeField] GameObject confirmSwapWindow;
    [SerializeField] NPC_Merchant[] merchants; // 0 = order, 1 = destruction
    [SerializeField] Transform merchantEquipmentSlotHolder;
    MerchantEquipmentSlot[] merchantEquipmentSlots;
    [SerializeField] GameObject interactionText;
    [SerializeField] TMP_Text goldValueInMerchantWindow, goldValueInGame;



    private void Awake()
    {
        _stageTimers = new TMP_Text[]{null, Text_stage1Timer, Text_stage2Timer, Text_stage3Timer, Text_stage4Timer};

        gameManager.StageChangeEvent += SetActiveStagePanel;
        gameManager.NewUITimes += SetNewUITimes;

        gameManager.CorruptionLevelChangeEvent += UpdateCemetaryCorruptionLevel;
        gameManager.NecromancerNumberChangeEvent += UpdateNecroCount;
        
        gameManager.WoodLevelChangeEvent += UpdateWoodStockLevel;
        gameManager.WoodcutterNumberChangeEvent += UpdateWoodcutterCount;

        gameManager.WaypointReachedChangeEvent += UpdateBatteringRamIconsProgression;
        gameManager.WaypointDistanceChangeEvent += UpdateBatteringRamSliderProgression;
        gameManager.TotalDistanceToGateCalculatedEvent += SetBatteringRamSliderTotalDistance;


        //gameManager.OrderTicketChangedEvent += UpdateOrderTicketValue;
        Debug.Log("GUI has subscribed to delegate messages.");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get player
        playerProfile = player.GetComponent<PlayerProfile>();

        //Character Status UI setup
        armorSlider.maxValue = playerProfile.SelectedCharData.armor;
        armorSlider.value = playerProfile.SelectedCharData.armor;
        armorValueText.text = playerProfile.SelectedCharData.armor.ToString() + " | " + playerProfile.SelectedCharData.armor.ToString();

        healthSlider.maxValue = playerProfile.SelectedCharData.health;
        healthSlider.value = playerProfile.SelectedCharData.health;
        healthValueText.text = playerProfile.SelectedCharData.health.ToString() + " | " + playerProfile.SelectedCharData.health.ToString();

        Img_CharIcon.sprite = playerProfile.SelectedCharData.charIcon;

        PopulateMerchantWindow();

        cam = Camera.main;

        minimapCameraController.SetupMinimap(player.transform, cam);
    }

    private void Update()
    {
        compass.UpdateCompass(cam.transform);
    }

    #region merchant window
    public void ToggleMerchantWindow(bool toState)
    {
        merchantWindow.SetActive(toState);
    }

    public void cancelConfirmation()
    {
        confirmSwapWindow.SetActive(false);
    }

    public void confirmConfirmation()
    {
        confirmSwapWindow.SetActive(false);

        //swap weapon
        Debug.Log("weapon swapped");
    }

    public void CloseMerchantWindow()
    {
        ToggleMerchantWindow(false);
        player.GetComponent<ControllerManager>().CloseInteractWithMerchant();
    }

    public void PopulateMerchantWindow()
    {
        merchantEquipmentSlots = merchantEquipmentSlotHolder.GetComponentsInChildren<MerchantEquipmentSlot>();

        Weapon[] weaponsForSale;

        if (playerProfile.GetAllegiance() == Allegiance.Order)
            weaponsForSale = merchants[0].WeaponsForSale;
        else
            weaponsForSale = merchants[1].WeaponsForSale;

        for (int i = 0; i < merchantEquipmentSlots.Length; i++)
        {
            if(i < weaponsForSale.Length)
            {
                merchantEquipmentSlots[i].PopulateMerchantEquipmentSlot(weaponsForSale[i]);
            }
        }
    }
    #endregion

    private void SetNewUITimes(int stageNr, float stageTime, float masterTime)
    {
        int sMinutes = Mathf.FloorToInt(stageTime / 60F);
        int sSeconds = Mathf.FloorToInt(stageTime % 60F);

        _stageTimers[stageNr].text = sMinutes.ToString("00") + ":" + sSeconds.ToString("00");

        int mMinutes = Mathf.FloorToInt(masterTime / 60F);
        int mSeconds = Mathf.FloorToInt(masterTime % 60F);

        Text_masterTimer.text = mMinutes.ToString("00") + ":" + mSeconds.ToString("00");
    }

    private void SetActiveStagePanel(int stage)
    {
        Debug.Log("Setting Active Panel In UI to: " + stage);

        //Stage info panel
        StageInfoPanel.SetActive(true);        
        Text_ChapterText.text = "Chapter " + stage;
        switch (stage)
        {
            case 1:
                Text_StageText.text = "Cemetary Corruption";
                Stage1Panel.SetActive(true);
                break;
            case 2:
                Text_StageText.text = "The Haunted Woods";
                Stage1Panel.SetActive(false);
                Stage2Panel.SetActive(true);
                break;
            case 3:
                Text_StageText.text = "Assault the Ramparts";
                Stage2Panel.SetActive(false);
                Stage3Panel.SetActive(true);
                break;
            case 4:
                Text_StageText.text = "Village Showdown";
                Stage3Panel.SetActive(false);
                Stage4Panel.SetActive(true);
                break;
        }
        StartCoroutine("InfoPanelTimeout");
        
    }

    private IEnumerator InfoPanelTimeout()
    {
        yield return new WaitForSeconds(5);
        StageInfoPanel.SetActive(false);
    }

    // Stage 1 UI Methods
    private void UpdateCemetaryCorruptionLevel(float value)
    {
        Text_sliderValue.text = value.ToString();
        _slider.value = value;
    }

    private void UpdateNecroCount(int noNecros)
    {
        Text_noLeft.text = noNecros.ToString();
    }

    // Stage 2 UI Methods
    private void UpdateWoodStockLevel(float value)
    {
        Text_sliderWoodValue.text = value.ToString();
        _woodSlider.value = value;
    }

    private void UpdateWoodcutterCount(int noCutters)
    {
        Text_noOfWCsLeft.text = noCutters.ToString();
    }

    //Stage 3 UI Methods
    private void UpdateBatteringRamIconsProgression(int index, bool backwards)
    {
        for (int i = 0; i < waypointIconHolder.Length; i++)
        {
            if(i < index)
                waypointIconHolder[i].color = destructionColor;
            else if (i == index)
            {
                if (backwards)
                    waypointIconHolder[i].color = orderColor;
                else
                    waypointIconHolder[i].color = destructionColor;
            } 
            else
                waypointIconHolder[i].color = orderColor;
        }
    }

    private void UpdateBatteringRamSliderProgression(float distance)
    {
        Text_sliderDistanceValue.text = Mathf.Floor(distance).ToString();
        _RamSlider.value = distance;
    }

    private void SetBatteringRamSliderTotalDistance(float totalDistance)
    {
        _RamSlider.maxValue = totalDistance;
    }

    //Stage 4 UI Methods

    // General UI Methods
    public void SetInteractionText(string text)
    {
        interactionText.SetActive(true);
        interactionText.GetComponent<TMP_Text>().text = text;
    }

    public void ClearInteractionText()
    {
        interactionText.SetActive(false);
        interactionText.GetComponent<TMP_Text>().text = "";
    }

    public void UpdateGold(int newGold)
    {
        goldValueInGame.text = newGold.ToString();
        goldValueInMerchantWindow.text = newGold.ToString();
    }

    public void UpdateXP(int level, int newXP)
    {
        xpSlider.value = newXP;
    }

    public void UpdateMapLocation(string locationName)
    {
        Text_location.text = locationName;
    }
}
