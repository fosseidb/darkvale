using UnityEngine;
using Invector.vCharacterController;

public class ControllerManager : MonoBehaviour
{
    // How far you can interact with the object from
    public float interactionDistance = 10f;

    EquipmentManager eqManager;
    SkillManager skillManager;
    PlayerProfile playerProfile;
    [SerializeField] InGameCanvasManager canvasManager;
    [SerializeField] vThirdPersonCamera thirdPersonCameraManager;
    [SerializeField] vThirdPersonInput thirdPersonInput;

    private bool canMove = true;

    void Start()
    {
        // Turns the text off if it isn't already.
        canvasManager.ClearInteractionText();

        eqManager = GetComponent<EquipmentManager>();
        skillManager = GetComponent<SkillManager>();
        playerProfile = GetComponent<PlayerProfile>();
    }

    void Update()
    {
        if (!canMove)
            return;

        #region Interaction
        // Creates a ray going from the camera.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Stores information about what the Raycast hit.
        RaycastHit hit;

        // Raycast for detecting the object.
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {

            // Checks for tag or other condition for recognising the object.
            if (hit.collider.tag == "Merchant")
            {
                NPC_Merchant merchant = hit.collider.GetComponent<NPC_Merchant>();
                if (merchant.DamagableType == playerProfile.GetAllegiance())
                {
                    // Turns on the interaction prompt.
                    canvasManager.SetInteractionText("Buy gear (F)");

                    // Interacts with the object upon button press.
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        InteractWithMerchant(merchant);
                    }
                }
            } else if (hit.collider.tag == "InteractiveObject")
            {

                // Turns on the interaction prompt.
                canvasManager.SetInteractionText("Pick Up item!");

                // Interacts with the object upon button press.
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("Interacting with object. WIP");
                }

            } else
            {
                // Turns the prompt back off when you're not looking at the object.
                canvasManager.ClearInteractionText();
            }
        }
        #endregion

        #region mouseControls
        // leftMouseButtonAttack i.e. right hand
        if (Input.GetMouseButtonDown(0))
        {
            eqManager.StartRightHand();
        }
        if (Input.GetMouseButtonUp(0))
        {
            eqManager.EndRightHand();
        }

        // right mouse button attack i.e. left hand
        if (Input.GetMouseButtonDown(1))
        {
            //start right mouse equipment effect
            eqManager.StartLeftHand();
        }
        if (Input.GetMouseButtonUp(1))
        {
            //stop right mouse equipment effect
            eqManager.EndLeftHand();
        }
        #endregion

        #region Skills
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //trigger skill1
            skillManager.ActivateSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //trigger skill1
            skillManager.ActivateSkill(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //trigger skill1
            skillManager.ActivateSkill(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //trigger skill1
            skillManager.ActivateSkill(4);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //trigger skill1
            eqManager.ActivateItem("Q");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //trigger skill1
            eqManager.ActivateItem("E");
        }
        #endregion
    }

    private void InteractWithMerchant(NPC_Merchant merchant)
    {
        Debug.Log("Interact!");
        //stop movement
        thirdPersonInput.ToggleCanMove(false);
        canMove = false;

        //free mouse
        thirdPersonCameraManager.ToggleCameraLock(false);

        //open merchant window
        canvasManager.ToggleMerchantWindow(true);
    }

    public void CloseInteractWithMerchant()
    {
        Debug.Log("Close Interact!");
        //stop movement
        thirdPersonInput.ToggleCanMove(true);
        canMove = true;

        //free mouse
        thirdPersonCameraManager.ToggleCameraLock(true);
    }
}
