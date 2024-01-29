using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLaser : MonoBehaviour
{
    public float rotationSpeed = 60.0f;
    public float translationSpeed = -.05f;
    public Transform whereToTeleOnDeath;

    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gamePaused)
            return;

        transform.Rotate(rotationSpeed * Time.deltaTime, 0.0f, 0.0f);
        transform.Translate(translationSpeed, 0f, 0f);
    }

    public void Reset(GameObject player)
    {
        player.GetComponentInParent<PlayerPain>().Death(whereToTeleOnDeath.position);
        this.transform.position = startPos;
    }
}
