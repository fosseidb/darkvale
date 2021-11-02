using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Note on terminology.
/// Equipment - group terminology for loot
/// Item / Utility - equipment that serve as power ups: potions and extra armor
/// Weapon - equippable equipment
/// </summary>




public class EquipmentManager : MonoBehaviour
{
    public Transform rightHandTransform;
    public Transform leftHandTransform;
    public Transform rightHipItemTransform;
    public Transform leftHipItemTransform;

    public GameObject rightHandItemPrefab;
    public GameObject leftHandItemPrefab;
    public GameObject leftHipItemPrefab;
    public GameObject rightHipItemPrefab;

    private GameObject rightHandItem, leftHandItem, rightHipItem, leftHipItem;

    Animator anim;

    private float leftHandReloadTime, rightHandReloadTime, rightItemCooldownTime, leftItemCooldownTime;
    private float leftHandReloadTimer, rightHandReloadTimer, rightItemCooldownTimer, leftItemCooldownTimer;
    private bool inAttack;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        rightHandItem = Instantiate(rightHandItemPrefab);
        rightHandItem.transform.parent = rightHandTransform;
        rightHandItem.transform.position = rightHandTransform.position;
        rightHandItem.transform.rotation = rightHandTransform.rotation;
        rightHandItem.layer = 8;

        leftHandItem = Instantiate(leftHandItemPrefab);
        leftHandItem.transform.parent = leftHandTransform;
        leftHandItem.transform.position = leftHandTransform.position;
        leftHandItem.transform.rotation = leftHandTransform.rotation;
        leftHandItem.layer = 8;

        rightHipItem = Instantiate(rightHipItemPrefab);
        rightHipItem.transform.parent = rightHipItemTransform;
        rightHipItem.transform.position = rightHipItemTransform.position;
        rightHipItem.transform.rotation = rightHipItemTransform.rotation;
        rightHipItem.layer = 8;

        leftHipItem = Instantiate(leftHipItemPrefab);
        leftHipItem.transform.parent = leftHipItemTransform;
        leftHipItem.transform.position = leftHipItemTransform.position;
        leftHipItem.transform.rotation = leftHipItemTransform.rotation;
        leftHipItem.layer = 8;

        rightHandReloadTime = 1f;
        rightHandReloadTimer = 0f;
        inAttack = false;

        rightHandItem.GetComponent<Weapon>().SetWielder(GetComponent<PlayerProfile>());
        leftHandItem.GetComponent<Weapon>().SetWielder(GetComponent<PlayerProfile>());
    }

    private void FixedUpdate()
    {
        if (inAttack && rightHandReloadTimer < rightHandReloadTime)
        {
            rightHandReloadTimer += Time.deltaTime;
        }
        else
        {
            inAttack = false;
            rightHandReloadTimer = 0f;

            if (rightHandItem.GetComponent<MeleeWeaponHitbox>() != null)
            {
                rightHandItem.GetComponent<MeleeWeaponHitbox>().Lethal = false;
            }
        }
    }

    public void Equip(GameObject equipment)
    {

    }

    public void StartRightHand()
    {
        if (inAttack)
            return;

        inAttack = true;

        //melee
        anim.SetTrigger("LeftMouseAttack");
        if (rightHandItem.GetComponent<MeleeWeaponHitbox>() != null)
        {
            rightHandItem.GetComponent<MeleeWeaponHitbox>().Lethal = true;
        }


        //ranged
        //anim.SetBool("LeftMouseAttack", true);

    }

    public void EndRightHand()
    {
        //anim.SetBool("LeftMouseAttack", false);
        //if (leftItem.GetComponent<MeleeWeaponHitbox>() != null)
    }

    public void StartLeftHand()
    {
        //if melee

        //if ranged rifle

        //if ranged bow

        //if pistol

        //if shield
        anim.SetBool("RightMouseHold", true);

    }

    public void EndLeftHand()
    {
        //if melee

        //if ranged rifle

        //if ranged bow

        //if pistol

        //if shield
        anim.SetBool("RightMouseHold", false);
    }

    //Item
    public void ActivateItem(string letter)
    {
        return;
    }


}
