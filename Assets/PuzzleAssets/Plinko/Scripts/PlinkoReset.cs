using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoReset : MonoBehaviour
{
    private Vector3 originalPosition;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = gameObject.transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void resetTile()
    {
        rb.position = originalPosition;
        rb.velocity = Vector3.zero;
    }
}
