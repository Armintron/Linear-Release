using UnityEngine;

public class Force : MonoBehaviour, IArrowType
{
    private Rigidbody rb;
    [SerializeField] private float forceMultiplier = 100f;

    public void DoInteraction(Vector3 arrowVelocity)
    {
        // Debug.Log("In Force Class DoInteraction");

        // this is attached to the force interactable (or cube)
        // Debug.Log("Arrow velocity: " + arrowVelocity);

        rb = GetComponent<Rigidbody>();

        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.AddForce(forceMultiplier * arrowVelocity, ForceMode.Force);
    }
}
