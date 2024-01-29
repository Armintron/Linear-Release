using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Animations;

public class TurretScript : MonoBehaviour
{
    [Header("     Reference Manager (no touch)")]

    [Tooltip("What we are trying to shoot (the player's capsule by default)")]
    public GameObject target;

    [Tooltip("Beginning of the barrel")]    
    public Transform barrelStart;

    [Tooltip("End of the barrel (aka where the bullet is shot from)")]
    public Transform shootPosition;

    [Tooltip("The part of the turret that rotates")]
    public GameObject weapon;

    [Tooltip("Sound to play when the turret shoots")]
    public AudioSource shootSound;

    [Header("     Aiming")]
    
    [Tooltip("Reference of line renderer we use for the aiming line")]
    public LineRenderer aimLine;

    [Tooltip("An offset of where to aim relative to the target transform")]
    public Vector3 aimOffset; 

    [Tooltip("Width of the line when when charge = 100%")]
    public float chargedLineWidth = .075f;

    [Tooltip("Color to start blending from when charge = 0%")]
    public Color startColor = Color.green;

    [Tooltip("Color to end blending to when charge = 100%")]
    public Color endColor = Color.red;

    [Header("     Charging")]

    [Tooltip("How much to charge the turret in terms of % per second. 1 = 100% charge per second")]
    public float chargeSpeed = .5f;

    [Tooltip("Base sound when charging (we pitch it as we charge)")]
    public AudioSource chargeSound;
    private float startVolume;

    [Tooltip("Pitch when charge = 100%")]
    public float chargedPitch = 3f;

    [Header("     Bullet Vars")]

    [Tooltip("Bullet the turret shoots")]
    public GameObject bullet;

    [Tooltip("Velocity of the bullet when its shot")]
    public float bulletSpeed = 10f;

    private float currCharge;

    // Reused / overriden each frame (just here for optimization)
    private Vector3 shootDirection, whereToLook;
    private Color lineColor;
    private bool playerHit;
    private RaycastHit hit;

    [SerializeField]
    public bool turretOn = true;

    public Transform whereToTeleOnDeath;

    public AudioSource disableSound;

    void Awake()
    {
        // find the part of the player we want in the scene if it isnt set
        if(target == null)
            target = GameObject.FindGameObjectWithTag("capsule");
        // make sure we have 2 positions in the array to set for the line start and end
        aimLine.positionCount = 2;
        startVolume = chargeSound.volume;
    }

    void Update()
    {
        if (PauseMenu.gamePaused)
            return;

        if (turretOn == false) return;
        // Make turret look at player
        whereToLook = target.transform.position + aimOffset;
        weapon.transform.LookAt(target.transform, Vector3.up);
        shootDirection = (whereToLook - barrelStart.position).normalized;

        // Draw aim line from barrelStart towards player
        aimLine.SetPositions(new Vector3[] {barrelStart.position, whereToLook});
        aimLine.widthMultiplier = chargedLineWidth * currCharge;
        // Interpolate color based on currCharge
        lineColor = Color.Lerp(startColor, endColor, currCharge);
        aimLine.startColor = lineColor;
        aimLine.endColor = lineColor;
        
        // Interpolate charging sound pitch
        chargeSound.pitch = Mathf.Lerp(1f, chargedPitch, currCharge);
        if(Mathf.Approximately(0f, currCharge)) chargeSound.volume = 0f;
        else chargeSound.volume = startVolume;

        //Debug.DrawLine(barrelStart.position, barrelStart.position + shootDirection * Vector3.Distance(barrelStart.position, whereToLook), Color.black);
        
        playerHit = false;
        // we hit something
        LayerMask mask = 1 << 10;
        mask = ~mask;

        if(Physics.Raycast(barrelStart.position, shootDirection, out hit, Mathf.Infinity, mask))
        {
            if(hit.collider.gameObject.CompareTag("capsule")) playerHit = true;     
        }
        // Decrease charge if this ray didnt hit the player, increase if it did
        currCharge += (playerHit ? chargeSpeed : -chargeSpeed) * Time.deltaTime;
        currCharge = Mathf.Clamp(currCharge, 0f, 1f);

        // Dont draw the aim line if we didnt hit the player this frame
        if(!playerHit) aimLine.enabled = false;
        else aimLine.enabled = true;

        // We are fully charged, so shoot and reset charge
        if(Mathf.Approximately(1f, currCharge))
        {
            currCharge = 0f;

            shootSound.Play();

            GameObject bullRef = Instantiate(bullet, shootPosition.position, Quaternion.identity);
            bullRef.GetComponent<BulletScript>().teleportLocationOnDeath = this.whereToTeleOnDeath.position;
            Rigidbody rb = bullRef.GetComponent<Rigidbody>();
            rb.AddForce(shootDirection * bulletSpeed, ForceMode.VelocityChange);
        }
    }

    public void DisableTurret()
    {
        Debug.Log("Disabling turret!");
        turretOn = false;
        aimLine.widthMultiplier = 0;
        chargeSound.volume = 0f;
        currCharge = 0;
        disableSound.Play();
    }

    public void EnableTurret()
    {
        turretOn = true;
    }
}
