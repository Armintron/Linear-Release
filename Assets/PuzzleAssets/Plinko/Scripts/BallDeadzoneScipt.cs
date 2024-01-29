using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDeadzoneScipt : MonoBehaviour
{
    public GameObject ballRef;
    public ObjectSpawner spawnerRef;
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Is Name: " + col.gameObject.name);
        Debug.Log("Should be Name: " + ballRef.name);

        if(col.gameObject.name.Contains(ballRef.name))
        {
            spawnerRef.SpawnObject();
        }
    }
}
