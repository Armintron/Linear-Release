using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private Color color;
    private AudioSource simonSound;
    private MeshRenderer meshRenderer;
    private SimonSaysManager simonSaysParent;


    [HideInInspector]
    public bool currentlyFlashing = false;

    // Sets the parent to the simon says manager!
    private void Start()
    {
        // Creates a new material and sets the color to whatever color is 
        // chosen in the inspector
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshRenderer.material.color = color;
        // Enable emission so we can use it later
        meshRenderer.material.EnableKeyword("_EMISSION");

        // Sets the parent
        simonSaysParent = GetComponentInParent<SimonSaysManager>();
        simonSound = GetComponent<AudioSource>();
    }

    // Flashes this button for the specified duration
    public void FlashColor(float duration, float timeToWait = 0f)
    {
        StartCoroutine(FlashColorCoroutine(duration, timeToWait));
    }

    IEnumerator FlashColorCoroutine(float duration, float timeToWait)
    {
        // Wait the given amount of time before flashing!
        yield return new WaitForSeconds(timeToWait);

        currentlyFlashing = true;

        simonSound.Play();

        // Change the emission on the material
        meshRenderer.material.SetColor("_EmissionColor", color * 10);
        yield return new WaitForSeconds(duration);
        meshRenderer.material.SetColor("_EmissionColor", color * 0);

        simonSound.Stop();

        currentlyFlashing = false;
    }

    // Returns the color of this button
    public Color GetColor()
    {
        return color;
    }

    // On collision, we send to the parent this script as the current button pressed!
    private void OnTriggerEnter(Collider other)
    {
        simonSaysParent.SetCurrentButton(this);

        other.enabled = false;
    }
}
