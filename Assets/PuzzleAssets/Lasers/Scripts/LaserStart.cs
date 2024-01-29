using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStart : MonoBehaviour
{
    public MoveLaser moveLaserScript;
    private bool laserStarted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("capsule"))
            if (!laserStarted)
            {
                moveLaserScript.enabled = true;
                laserStarted = true;
            }
    }
}
