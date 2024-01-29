using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHinge : MonoBehaviour
{
    public float xRotateAmount = 0f;

    public void DoRotate()
    {
        this.gameObject.transform.Rotate(new Vector3(xRotateAmount, 0, 0));
    }
}
