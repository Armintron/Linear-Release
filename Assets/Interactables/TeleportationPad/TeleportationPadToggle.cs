using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationPadToggle : MonoBehaviour
{
    public bool padOn = true;
    public GameObject halo;
    // sound to play when the pad is turned on
    public AudioSource turnOnSound;
    void Start()
    {
        // if we initially set it to be off, toggle the pad so it
        // seems turned off and then manually turn padOn var back to false
        // i know we should prob call turnPadOff(), but i dont want the sound to be played
        if(!padOn)
            halo.SetActive(false);
    }
    public void turnPadOn()
    {
        padOn = true;
        halo.SetActive(true);
        playOnSound();
    }
    public void turnPadOff()
    {
        padOn = false;
        halo.SetActive(false);
        playOffSound();
    }
    public void togglePad()
    {
        if(padOn) turnPadOff();
        else turnPadOn();
    }

    void playOffSound()
    {
        // jank way of playing in reverse
        turnOnSound.pitch = -1;
        turnOnSound.timeSamples = turnOnSound.clip.samples - 1;
        turnOnSound.Play();
    }

    void playOnSound()
    {
        // make sure to revert reverse playing changes
        turnOnSound.pitch = 1;
        turnOnSound.timeSamples = 0;
        turnOnSound.Play();
    }
}
