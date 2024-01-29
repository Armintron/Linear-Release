using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    enum ArrowTypes { Interaction, Teleportation, Force, Tether };

    [SerializeField]
    ArrowTypes[] arrowTypes;

    [HideInInspector]
    public GameObject playerReference;

    private Rigidbody rigidbody;

    #region Electricity
    public bool electricEnabled = false;
    public GameObject wirePrefab;
    // Used to turn the wire on and off, based on how we like the current line renderer
    public bool enableWire = true;

    [SerializeField]
    private GameObject electricEffect;

    [HideInInspector]
    public Vector3[] latestPointsHit;
    [HideInInspector]
    public FenceManager lastFenceHit;
    #endregion

    #region Ricochet Variables
    public int numBounces = 0;

    // The length of the arrow, and thus, the length of the raycast
    private float distance = 3.5f;
    private bool lastCollisionHappened = false;

    // Stores the arrow upon colliding (raycast hit) with something
    private GlobalArrowManager globalArrowManager;
    [HideInInspector] public StandardizedProjectile standardizedProjectile;
    #endregion

    #region Contact Sounds
    [SerializeField]
    private AudioSource ricochetSound;
    [SerializeField]
    private AudioSource landedSound;
    #endregion

    // Initializes the variables related to ricocheting
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        globalArrowManager = FindObjectOfType<GlobalArrowManager>();
        standardizedProjectile = GetComponent<StandardizedProjectile>();

        latestPointsHit = new Vector3[2];
    }

    public void CheckCollision(GameObject collision)
    {
        foreach (ArrowTypes currType in arrowTypes)
        {
            switch (currType)
            {
                case ArrowTypes.Interaction:
                    Interaction interactionComponent = collision.GetComponent<Interaction>();

                    if (interactionComponent != null)
                        interactionComponent.DoInteraction();

                    // Debug.Log("In Arrow Manager: Interaction");
                    break;

                case ArrowTypes.Teleportation:
                    Teleportation teleportComponent = collision.GetComponent<Teleportation>();

                    if (teleportComponent != null)
                        teleportComponent.DoInteraction(playerReference);

                    // Debug.Log("In Arrow Manager: Teleportation");
                    break;

                case ArrowTypes.Force:
                    Force forceComponent = collision.GetComponent<Force>();

                    if (forceComponent != null)
                        forceComponent.DoInteraction(rigidbody.velocity);

                    // Debug.Log("In Arrow Manager: Force");
                    break;

                default:
                    // Debug.Log("No script attached to this object");
                    break;
            }
        }
    }

    // Necessary since the bow contains the amount of times we want to bounce in general,
    // but once shot, arrow bounces by itself.
    public void SetNumBounces()
    {
        numBounces = standardizedProjectile.bowScript.numBounces;
    }

    private void Update()
    {
        if (PauseMenu.gamePaused)
            return;

        Debug.DrawRay(transform.position, transform.forward * distance);

        // We dont want to do any of this until the arrow has left the bow
        if (!standardizedProjectile.isArrowShot)
            return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        /*// ignore PlinkoInvisStopper layer
        LayerMask mask = 1 << 11;
        // ignore the "Ignore Raycast" layer
        mask |= 4;
        mask = ~mask;
        Debug.Log("Mask: " + mask.value);*/

        if (!lastCollisionHappened && Physics.Raycast(ray, out hit, distance/*, mask*/))
        {
            // Send THIS arrow to a global accessible class
            globalArrowManager.latestArrowShot = gameObject;
            globalArrowManager.latestObjectHit = hit.transform.gameObject;

            SetLatestPoints(hit);

            // Calls the trigger and collision functions since we are using a raycast now instead
            // of a regular collision
            hit.collider.gameObject.SendMessage("OnTriggerEnter", GetComponent<BoxCollider>(), SendMessageOptions.DontRequireReceiver);
            hit.collider.gameObject.SendMessage("OnCollisionEnter", new Collision(), SendMessageOptions.DontRequireReceiver);

            // Only reflect when we dont hit a trigger
            if (!hit.collider.isTrigger)
            {
                // Check for the specific arrow type on a collision
                CheckCollision(hit.collider.gameObject);

                // When we are not able to ricochet anymore (ie. ran out of bounces)
                // Turn the collision back on
                if (--numBounces < 0)
                {
                    lastCollisionHappened = true;
                    standardizedProjectile.DoCollision();
                    landedSound.Play();
                    // GetComponent<BoxCollider>().enabled = lastCollisionHappened;
                    return;
                }

                // Actually do the ricocheting
                Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
                rigidbody.velocity = reflectDir * rigidbody.velocity.magnitude;
                
                if (!hit.collider.gameObject.CompareTag("Reflector"))
                    ricochetSound.Play();
            }
        }

        /*if (rigidbody.velocity.magnitude == 0)
            landedSound.Play();*/
    }

    private void SetLatestPoints(RaycastHit hit)
    {
        FenceManager currFence = hit.collider.GetComponent<FenceManager>();

        if (currFence != null)
        {
            if (lastFenceHit == null)
                latestPointsHit[0] = hit.point;

            // If we have (non-E -> ??) either replace the fence if we have another 
            // non-E, or set the next point since we have an E
            else if (!lastFenceHit.electricIsOn)
            {
                if (currFence.electricIsOn)
                    latestPointsHit[1] = hit.point;
                else
                    latestPointsHit[0] = hit.point;
            }

            // Electric fence will always be in position 1
            else if (lastFenceHit.electricIsOn)
                latestPointsHit[1] = hit.point;
        }
    }

    // Change whether the electricity is on or off
    public void SetElectricity(bool state)
    {
        electricEnabled = state;
        electricEffect.SetActive(state);
    }
}