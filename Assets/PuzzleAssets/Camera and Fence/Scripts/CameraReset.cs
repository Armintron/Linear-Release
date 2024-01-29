using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour
{
    private bool startState;
    private Viewcone viewcone;
    // Start is called before the first frame update
    void Start()
    {
        viewcone = this.GetComponent<Viewcone>();
        if(viewcone) startState = viewcone.enabled;
    }

    public void Reset()
    {
        if(viewcone) viewcone.enabled = startState;
    }
}
