using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPain : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup blackScreen;
    [SerializeField]
    private float secondsUntilStartFade = .2f;
    [Tooltip("Speed at which we fade out. NOTE: this is a dividing value, so higher is slower, closer to 0 is faster.")]
    [SerializeField]
    private float fadeOutSpeedDivider = 2.0f;

    // Used for getting that "hit" effect when touching an electric fence
    private Rigidbody rb;
    private Image image;

    private void Start()
    {
        Death();

        rb = GetComponentInChildren<Rigidbody>();
        image = blackScreen.GetComponentInChildren<Image>();
    }

    // Over time, fade out the black screen
    IEnumerator FadeOut(bool death)
    {
        // If we died, wait before fading, otherwise, go straight to the fade
        if (death) yield return new WaitForSeconds(secondsUntilStartFade);

        while (blackScreen.alpha > 0)
        {
            blackScreen.alpha -= Time.deltaTime / fadeOutSpeedDivider;
            yield return null;
        }

        // In the case we use the red screen to 
        image.color = Color.black;

        yield return null;
    }

    private void CutToBlack(float opacity)
    {
        blackScreen.alpha = opacity;
    }

    // Called when we want to play the death "animation"
    // Cuts to black, waits a little bit, then fades out
    public void Death(Vector3 respawnPoint = new Vector3(), AudioClip audio = null)
    {
        PainHelper(true);
        
        if (respawnPoint != Vector3.zero) 
            rb.position = respawnPoint;

        if (audio != null)
            AudioSource.PlayClipAtPoint(audio, transform.position);


    }

    // Allows us to reuse this function for both an "ouch" and also a "death"
    private void PainHelper(bool death)
    {
        if (death) CutToBlack(1);
        else CutToBlack(.7f);

        StartCoroutine(FadeOut(death));
    }

    // Does a slight modification on the death animation, with a much softer opacity
    // on the black screen, and changing the black to red
    private void Ouch()
    {
        // Change the color to red for pain!
        image.color = Color.red;
        PainHelper(false);
    }

    public void PushFromPain(Vector3 direction, AudioClip audio)
    {
        rb.AddForce(-direction * 400, ForceMode.Impulse);
        // play the zapping sound at the position they hit the collider
        AudioSource.PlayClipAtPoint(audio, transform.position);

        Ouch();
    }
}

