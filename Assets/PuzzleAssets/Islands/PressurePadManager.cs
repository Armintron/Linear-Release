using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadManager : MonoBehaviour
{
    private int lastPressed = -1;
    private int finalID = 0;
    private PressurePadScript[] pads;
    public AudioSource winSound;
    public UnityEngine.Events.UnityEvent whenWon;
    // Start is called before the first frame update
    void Start()
    {
        pads = GetComponentsInChildren<PressurePadScript>();
        foreach(PressurePadScript pad in pads)
        {
            if(pad.ID > finalID) finalID = pad.ID;
            pad.whenActivated.AddListener(padActivated);
        }
    }

    public void padActivated(PressurePadScript pad, System.Action<bool> callback)
    {
        if(lastPressed == pad.ID - 1)
        {
            lastPressed = pad.ID;
            callback(true);
            if(lastPressed == finalID)
            {
                win();
            }
        }
        else callback(false);
    }

    void win()
    {
        winSound.Play();
        whenWon.Invoke();
    }
}
