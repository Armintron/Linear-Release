using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour, IArrowType
{
    public Transform whereToTeleport;
    public void DoInteraction(GameObject player)
    {
        // Debug.Log("In Teleportation Class DoInteraction");
        player.GetComponent<Rigidbody>().position = whereToTeleport.position;
    }
}
