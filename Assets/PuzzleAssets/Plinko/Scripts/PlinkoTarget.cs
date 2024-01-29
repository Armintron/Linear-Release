using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoTarget : MonoBehaviour
{
    public GameObject plinkoBall;
    public UnityEngine.Events.UnityEvent toTrigger;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains(plinkoBall.name))
        {
            toTrigger.Invoke();
        }
    }
}
