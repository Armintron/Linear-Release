using UnityEngine;

public class ArrowInfiniteRicochet : MonoBehaviour
{
    public ArrowManager managerRef;
    public float arrowSpeed;
    public Rigidbody rb;

    public bool hasWireSpawned = false;

    public FenceManager first, second;
    // Start is called before the first frame update
    void Start()
    {
        // "shoot" the arrow
        ShootProjectile();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gamePaused)
            return;

        managerRef.standardizedProjectile.isArrowShot = true;
        // just some arbitrary number to make sure it always stays above 0
        managerRef.numBounces = 3;
    }

    public void ShootProjectile()
    {
        // make arrow think it was shot
        this.transform.parent = null;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;


        // Axis switch for different models
       
        this.transform.localEulerAngles = this.transform.localEulerAngles + Vector3.up + Vector3.right;                
        this.transform.parent = null;
        rb.AddForce(this.transform.forward * arrowSpeed, ForceMode.VelocityChange);


    }
}
