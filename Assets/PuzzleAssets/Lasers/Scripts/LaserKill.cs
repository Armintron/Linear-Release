using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserKill : MonoBehaviour
{
    public MoveLaser laserParent;
    private Vector3 startPos;

    void Start()
    {
        startPos = laserParent.transform.position;
    }
    void OnTriggerEnter(Collider col)
    {

        if(col.gameObject.CompareTag("Arrow"))
        {
            Debug.Log("AYOOOO!");
            Debug.Log(col.gameObject.name);
            Destroy(col.gameObject);
        }
        if(col.gameObject.CompareTag("capsule"))
        {
            PlayerHit(col.gameObject);
        }
    }

    void PlayerHit(GameObject player)
    {
        laserParent.Reset(player);
    }
}
