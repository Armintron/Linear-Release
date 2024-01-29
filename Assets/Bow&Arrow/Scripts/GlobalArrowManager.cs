using UnityEngine;

public class GlobalArrowManager : MonoBehaviour
{
    private GlobalArrowManager globalArrowManager;
    public GameObject latestArrowShot { get; set; }
    public GameObject latestObjectHit { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        // Prevent more than one instance of this global manager from spawning
        if (globalArrowManager == null)
            globalArrowManager = this;
        else
            Destroy(gameObject);
    }
}
