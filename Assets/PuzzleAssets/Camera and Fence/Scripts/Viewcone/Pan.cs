using UnityEngine;

//Attach this script to a GameObject to rotate around the target position.
public class Pan : MonoBehaviour
{
    public Quaternion startRot, endRot;

    public float speed = 15.0f;

    private void Update()
    {
        if (PauseMenu.gamePaused)
            return;

        float t = (Mathf.Sin(Time.time * speed) + 1) / 2;
        transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
    }
}