using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, IArrowType
{
    // The callback functions to invoke on gameobjet scripts in the scene when this gameobject is interacted with
    public UnityEngine.Events.UnityEvent whatToTrigger;

    public void DoInteraction()
    {
        // Debug.Log("In Interaction Arrow Class");
        whatToTrigger.Invoke();
    }
}
