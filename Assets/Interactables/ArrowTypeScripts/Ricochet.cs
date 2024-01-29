using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// REMEMBER: this is attached to the walls that support ricocheting

/*public class Ricochet : MonoBehaviour, IArrowType
{
    public void DoInteraction(Vector3 currentDirection)
    {
        Debug.Log("In Ricochet Class DoInteraction");

        // contains raycast information
        RaycastHit hit;

        // starting point of ray set to box position
        // direction of ray set to arrow direction
        Ray ray = new Ray(transform.position, currentDirection); 

        if (Physics.Raycast(ray, out hit))
        {
            currentDirection = Vector3.Reflect(currentDirection, hit.normal);
        }
    }
}
*/