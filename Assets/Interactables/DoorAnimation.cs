using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public Animator myAnimator = null;
    public string stateNameToPlay = "DoorOpen";
    public void DoInteraction()
    {
        myAnimator.Play(stateNameToPlay, 0, 0.0f);
    }
}
