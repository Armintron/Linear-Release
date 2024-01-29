using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoomReset : MonoBehaviour
{
    
    public ToggleElectricity toggle;
    public CameraDisable cameraDisable;

    public void ResetRoom()
    {
        toggle.SetOnOffFences();
        cameraDisable.EnableCameraDetection();
    }
}
