using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    Transform player;
    Camera mainCamera;

    public bool RotateWithPlayer = true;

    public void SetupMinimap(Transform player, Camera camera)
    {
        this.player = player;
        mainCamera = camera;

        SetMiniMapCameraPosition();
        SetMinimapCameraRotation();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player != null)
        {
            SetMiniMapCameraPosition();
            if (RotateWithPlayer && mainCamera)
            {
                SetMinimapCameraRotation();
            }
        }
    }

    private void SetMiniMapCameraPosition()
    {
        var newPos = player.position;
        newPos.y = transform.position.y;

        transform.position = newPos;
    }

    private void SetMinimapCameraRotation()
    {
        transform.rotation = Quaternion.Euler(90.0f, mainCamera.transform.eulerAngles.y, 0.0f);
    }
}
