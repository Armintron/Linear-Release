using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCameraRotation : MonoBehaviour
{
    // Stores information on what the player is doing regarding rotations
    private Vector2 lookVal;
    private Vector2 lookAbsolute;
    private Vector2 smoothLook;

    // Stores the directions of the player and the camera
    private Vector2 targetDirection;
    private Vector2 targetPlayerDirection;

    [Tooltip("Degrees to clamp the camera rotation. 360 means no clamping on that axis")]
    [SerializeField] private Vector2 clampDegrees = new Vector2(360, 180);
    
    [Tooltip("Smoothing to be applies per axis")]
    [SerializeField] private Vector2 lookSmoothing = new Vector2(2, 2);
    
    [Tooltip("Sensitivity to be applies per axis")]
    [SerializeField] private Vector2 lookSensitivity = new Vector2(.1f, .1f);

    [Tooltip("This camera is attached to a player with movement, so assign that here!")]
    [SerializeField] private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        // Locks and makes cursor invisible
        Cursor.lockState = CursorLockMode.Locked;

        // Store the player's initial direction (could be something other than (0,0))
        targetPlayerDirection = Player.transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PauseMenu.gamePaused)
            return;

        // Store the initial rotation of the camera
        Quaternion targetOrientation = Quaternion.Euler(targetDirection);
        // Store the initial rotation of the player
        Quaternion targetPlayerOrientation = Quaternion.Euler(targetPlayerDirection);

        // Scale input against the sensitivity setting and multiply that with the smoothing value.
        Vector2 lookValDelta = Vector2.Scale(lookVal, new Vector2(lookSensitivity.x * lookSmoothing.x, lookSensitivity.y * lookSmoothing.y));

        // Smoothly go between our values to ensure we have a nice looking rotation, instead of it being too snappy and rigid
        smoothLook.x = Mathf.Lerp(smoothLook.x, lookValDelta.x, 1f / lookSmoothing.x);
        smoothLook.y = Mathf.Lerp(smoothLook.y, lookValDelta.y, 1f / lookSmoothing.y);

        // Get the camera rotation value from (0, 0). Since we add every frame, it builds up to
        // our current modified look value. So we can then modify that as a whole and then apply
        // it to whatever, as opposed to using the values generated each frame (wouldn't do much since
        // so small).
        lookAbsolute += smoothLook;

        // Clamp and apply the local x value first
        if (clampDegrees.x < 360)
            lookAbsolute.x = Mathf.Clamp(lookAbsolute.x, -clampDegrees.x * 0.5f, clampDegrees.x * 0.5f);

        // Then do the same for global y value
        if (clampDegrees.y < 360)
            lookAbsolute.y = Mathf.Clamp(lookAbsolute.y, -clampDegrees.y * 0.5f, clampDegrees.y * 0.5f);

        // Apply the rotation to the camera.
        transform.localRotation = Quaternion.AngleAxis(-lookAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        // Apply the rotation to the player.
        Quaternion yRotation = Quaternion.AngleAxis(lookAbsolute.x, Vector3.up);
        Player.transform.localRotation = yRotation * targetPlayerOrientation;
    }

    // Uses input manager to call this function, which assigns the values of the lookVal variable
    public void Look(InputAction.CallbackContext value)
    {
        // read in the player's mouse inputs
        lookVal = value.ReadValue<Vector2>();
    }
}
