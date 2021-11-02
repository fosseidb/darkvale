using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiniMapIcon
{
    Player, 
    Merchant,
    Enemy,
    Ally,
    BatteringRam,
};

public class MiniMapIconMaterialController : MonoBehaviour
{
    public MiniMapIcon mmIcon;

    // Set the materials in the inspector
    public Material[] myMiniMapMaterials;

    Renderer renderer;  

    // Use this for initialization
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        renderer.enabled = true;

        switch (mmIcon)
        {
            case MiniMapIcon.Player:
                renderer.material = myMiniMapMaterials[0];
                break;
            case MiniMapIcon.Merchant:
                renderer.material = myMiniMapMaterials[1];
                break;
            case MiniMapIcon.Enemy:
                renderer.material = myMiniMapMaterials[2];
                break;
            case MiniMapIcon.Ally:
                renderer.material = myMiniMapMaterials[3];
                break;
            case MiniMapIcon.BatteringRam:
                renderer.material = myMiniMapMaterials[4];
                break;
        }
    }
}