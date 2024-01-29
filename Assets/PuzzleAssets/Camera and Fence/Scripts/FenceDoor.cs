using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceDoor : MonoBehaviour
{
    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
    }
}
