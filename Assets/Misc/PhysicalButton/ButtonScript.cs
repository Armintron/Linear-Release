using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider obj)
    {
        Debug.Log("Entered");
    }

    private void OnTriggerExit(Collider obj)
    {
        Debug.Log("Exited");
    }
}
