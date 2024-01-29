using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressurePadScript : MonoBehaviour
{
    public float timeToActivate = 2f;

    [Tooltip("ID of this pressure pad, which represents its order in the puzzle")]
    public int ID;
    public AudioClip steppedOnSound, steppedOffSound, activatedSound, wrongOrderSound;
    public AudioSource soundPlayer;
    public Image progressCircle;
    public UnityEngine.Events.UnityEvent<PressurePadScript, System.Action<bool>> whenActivated;
    private float steppedOnTime = 0;
    private bool steppedOn = false, isDone = false;
    private Coroutine progressRoutine;

    void Update()
    {
        if (PauseMenu.gamePaused)
            return;

        if (!isDone && steppedOn && Time.time - steppedOnTime >= timeToActivate)
        {
            padActivated();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(!isDone && col.gameObject.CompareTag("capsule"))
        {
            playSound(steppedOnSound);
            steppedOn = true;
            steppedOnTime = Time.time;
            progressRoutine = StartCoroutine(progress());
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(!isDone && col.gameObject.CompareTag("capsule")) 
        {
            steppedOn = false;
            playSound(steppedOffSound);

            StopCoroutine(progressRoutine);
            progressCircle.fillAmount = 0;
        }
    }

    IEnumerator progress()
    {
        while(true)
        {
            progressCircle.fillAmount = (Time.time - steppedOnTime) / timeToActivate;
            //progressCircle.fillAmount = Mathf.Lerp(0, 1, (Time.time - steppedOnTime) / timeToActivate);
            yield return null;
        }
    }

    private void playSound(AudioClip audioClip)
    {
        soundPlayer.clip = audioClip;
        if(soundPlayer.isPlaying) soundPlayer.Stop();
        soundPlayer.Play();
    }

    void padActivated()
    {
        Debug.Log("Activated pressure pad!");
        whenActivated.Invoke(this, checkOrder);        
    }

    public void checkOrder(bool correctOrder)
    {
            Debug.Log("Was correct order? " + correctOrder);
            if(!correctOrder) 
            {
                playSound(wrongOrderSound);
            }
            else
            {
                playSound(activatedSound);
                isDone = true;
                progressCircle.fillAmount = 1;
            }
            steppedOn = false;
            StopCoroutine(progress());
    }
}
