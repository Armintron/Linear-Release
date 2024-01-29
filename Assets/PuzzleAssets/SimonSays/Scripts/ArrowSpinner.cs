using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpinner : MonoBehaviour
{

    public float deltaRot = 1;
    IEnumerator SpinArrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void Start()
    {
        //StartCoroutine(SpinArrow());
    }

    void Update()
    {
        if (PauseMenu.gamePaused)
            return;

        transform.rotation *= Quaternion.AngleAxis(-deltaRot, Vector3.right);
    }
}
