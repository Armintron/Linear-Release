using System.Collections;
using UnityEngine;

public class ReflectivePairs : MonoBehaviour
{
    [SerializeField]
    private ReflectivePairs otherReflector;
    [HideInInspector]
    public bool isHit = false;

    [SerializeField]
    private float timeToResetReflector = 2.0f;
    [HideInInspector]
    public GameObject arrow = null;

    private MeshRenderer meshRenderer;
    public Color emissiveColor;
    private GlobalArrowManager globalArrowManager;

    private Coroutine coroutine = null;
    private bool reflectorLocked = false;

    private AudioSource soundOnHit;
    private ReflectorManager reflectorManager;

    private void Awake()
    {
        // Gets the material on this reflector
        meshRenderer = GetComponent<MeshRenderer>();
        emissiveColor = meshRenderer.material.GetColor("_EmissionColor");

        // Set the color of the emission to white
        meshRenderer.material.SetColor("_EmissionColor", emissiveColor * -10);
        // Needed to get the latest arrow shot
        globalArrowManager = FindObjectOfType<GlobalArrowManager>();

        soundOnHit = GetComponent<AudioSource>();
    }

    // After a certain amount of time, reset the state of the reflector
    IEnumerator ResetReflector()
    {
        yield return new WaitForSeconds(timeToResetReflector);

        ResetState();
    }

    public void Success()
    {
        StopCoroutine(coroutine);
        reflectorLocked = true;
        // Checks if all 3 pairs in the level are in a success state (locked)
        reflectorManager.CheckReflectorPairs();
    }

    // Helper function for when the pad is hit
    private void OnHit(GameObject arrow)
    {
        isHit = true;
        this.arrow = arrow;

        // Set the color of the emission to original
        meshRenderer.material.SetColor("_EmissionColor", emissiveColor * 1);

        // Play a sound
        soundOnHit.Play();
    }

    // Helper function for reseting everything back to square "juan" (xD)
    private void ResetState()
    {
        isHit = false;
        this.arrow = null;

        // play a sound
        // Set the color of the emission to white
        meshRenderer.material.SetColor("_EmissionColor", emissiveColor * -10);
    }

    public void SetReflectorManager(ReflectorManager reflectorManager)
    {
        this.reflectorManager = reflectorManager;
    }
    
    public bool GetReflectorLocked()
    {
        return reflectorLocked;
    }

    // for puzzle skip win state - see ReflectorManager
    public void SetReflectorLocked(bool isLocked)
    {
        reflectorLocked = isLocked;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (reflectorLocked) return;

        OnHit(globalArrowManager.latestArrowShot);
        // Wait "x" amount of seconds and reset. If we succeed, this will be canceled
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(ResetReflector());

        // If the other reflector is on and the same arrow hit both reflectors
        // do the thang
        if (otherReflector.isHit && this.arrow.Equals(otherReflector.arrow))
        {
            Success();
            otherReflector.Success();
        }
    }
}
