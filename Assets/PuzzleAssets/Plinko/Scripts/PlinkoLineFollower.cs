using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoLineFollower : MonoBehaviour
{
    public Transform start, end;
    public Vector3 offset;
    public GameObject plinko;
    public Rigidbody rb = null;
    private Vector3 lineVector;

    void Start()
    {
        if(rb == null) rb = plinko.GetComponent<Rigidbody>();
        lineVector = end.position - start.position;

    }

    void FixedUpdate()
    {
        if (PauseMenu.gamePaused)
            return;

        Vector3 projected = Vector3.Project(plinko.transform.position, lineVector);
        //rb.MovePosition(projected);
        Debug.DrawLine(lineVector, lineVector.normalized * 20, Color.red);
        Debug.DrawLine(projected, projected.normalized * 20, Color.blue);

        rb.velocity = Vector3.Project(rb.velocity, lineVector);
    }
}
