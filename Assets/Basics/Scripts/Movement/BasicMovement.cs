using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovement : MonoBehaviour
{
    // The component that allows for physics interactions
    private Rigidbody rb;

    // Stores the Input of the player
    private BasicInput inputSystem;
    Vector2 moveVal = new Vector2(0, 0);

    // Values to be adjusted that will determine the speed of player
    [SerializeField] private float baseSpeed = 10.0f;
    [SerializeField] private float sprintSpeed = 15.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [Header("   Footsteps")]
    // how long a single "stride" is, aka distance for one footstep
    public float strideLength = 6f;
    public float maxVelocityChangePerStep = 10f;
    [SerializeField] private AudioSource footstep;
    public AudioClip[] footsteps;
    public float lowerFootstepPitch = .8f, upperFootstepPitch = 1.4f;
    private float currentSpeed;
    private Vector3 lastPos;
    private float deltaDistanceSinceLastFootstep = 0;

    // Tells us whether or not we pressed the jump button
    private bool bJumpButtonPressed = false;
    private float distToGround = 100.0f;

    // The maximum amount the velocity can change by each frame
    private float maxVelocityChange = 10.0f;

    private void Awake()
    {
        // Instantiate a new InputSystem
        inputSystem = new BasicInput();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Assigns the rigidbody component
        rb = gameObject.GetComponent<Rigidbody>();
        // Freeze the rotation, otherwise, we roll on the sides!
        rb.freezeRotation = true;

        // Gets the size of the collider to know when the player isnt grounded
        distToGround = GetComponent<Collider>().bounds.extents.y;

        // The speed starts at the base speed
        currentSpeed = baseSpeed;
        lastPos = transform.position;
    }

    // Using fixed update since physics needs to be the same across all systems.
    // Using regular update doesnt account for different frame rates. Using FixedUpdate
    // makes it easier because we don't have to multiply by the frame rate.
    private void FixedUpdate()
    {
        if (PauseMenu.gamePaused)
            return;

        // Get the direction of the velocity vector based on current inputs
        Vector3 targetVelocity = new Vector3(moveVal.x, 0, moveVal.y);
        // Transform it to local space (relative to where the player is looking)
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= currentSpeed;

        // Now we continuously add a force till we reach the targetVelocity
            
        Vector3 currVelocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - currVelocity);

        // Clamp the velocity between the maximum and minimum change value defined above
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        // We don't want vertical movement based on the WASD keys
        velocityChange.y = 0;

        // Applies the force we calculated
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        
        // only account for footstep movement when we are on the ground
        if(IsGrounded()) 
        {
            deltaDistanceSinceLastFootstep += Vector3.Distance(lastPos, transform.position);
            lastPos = transform.position;
            HandleFootstep();
        }
        
        // If we press the jump button, apply the jump force. This will only execute if we are on the ground.
        // We could do something more complex, but this is simple enough for what we need.
        if (bJumpButtonPressed && IsGrounded())
        {
            rb.AddForce(new Vector3(currVelocity.x, jumpForce, currVelocity.y), ForceMode.Impulse);

            bJumpButtonPressed = false;
        }
    }

    // Read in the values from the input manager specified and store in moveVal variable
    public void Move(InputAction.CallbackContext value)
    {
        moveVal = value.ReadValue<Vector2>();
    }

    // Change if the player is jumping when they press the jump button
    public void Jump()
    { 
        bJumpButtonPressed = true;
    }

    // If we are "staying" on the ground, we can set the grounded variable and canJump to true
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    // Changes whether or not the player is sprinting
    public void Sprint(InputAction.CallbackContext value)
    {
        // Gets whether or not the player is sprinting.
        // This converts float to bool.
        bool bSprintEnabled = value.ReadValue<float>() > 0 ? true : false;

        // Based on whether or not the sprint button is pressed,
        // change the player's speed.
        if (bSprintEnabled)
            currentSpeed = sprintSpeed;
        else
            currentSpeed = baseSpeed;
    }

    private void HandleFootstep()
    {
        if(deltaDistanceSinceLastFootstep >= strideLength)
        {
            footstep.pitch = Random.Range(lowerFootstepPitch, upperFootstepPitch);
            // Play clip from footsteps array
            footstep.clip = footsteps[Random.Range(0, footsteps.Length - 1)];
            footstep.Play();
            // Reset to accumulate up to next footst
            deltaDistanceSinceLastFootstep = 0f;
        }
    }
}
