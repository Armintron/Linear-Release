using UnityEngine;

public class ToggleElectricity : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ElectricFencesToggle;
    [SerializeField]
    private GameObject[] ElectricFencesOn;
    [SerializeField]
    private GameObject[] ElectricFencesOff;

    [SerializeField]
    private FencePuzzleCounter fencePuzzleCounter;

    private GlobalArrowManager globalArrowManager;
    [SerializeField]
    private bool needElectricityToPress = false;

    public CameraRoomReset roomReset;

    void Start()
    {
        // Needed to get the latest arrow shot
        globalArrowManager = FindObjectOfType<GlobalArrowManager>();
    }

    // When we press the button, toggle all fences attached
    private void OnCollisionEnter(Collision collision)
    {
        if(!arrowCausedCollision(collision)) return;
        if(roomReset)
        {
            roomReset.ResetRoom();
        }
        else
        {
            DefaultButtonAction();
        }
        if (fencePuzzleCounter) fencePuzzleCounter.CheckFences();   
    }
    
    // returns true if an arrow caused the collision
    private bool arrowCausedCollision(Collision collision)
    {
        return collision.contactCount == 0;
    }

    private void DefaultButtonAction()
    {
        SetOnOffFences();

        ArrowManager lastArrowShot = globalArrowManager.latestArrowShot.GetComponent<ArrowManager>();

        // If we dont need electricty to press, or we do and the electricity is on, do the toggling and such.
        if (!needElectricityToPress || lastArrowShot == null || !lastArrowShot.electricEnabled)
            return;

        SetToggleFences();
    }

    public void SetOnOffFences()
    {
        foreach (GameObject fence in ElectricFencesOn)
            fence.GetComponentInChildren<FenceManager>().SetFenceState(true);

        foreach (GameObject fence in ElectricFencesOff)
            fence.GetComponentInChildren<FenceManager>().SetFenceState(false);
    }

    private void SetToggleFences()
    {
        foreach (GameObject fence in ElectricFencesToggle)
        {
            FenceManager fenceManager = fence.GetComponentInChildren<FenceManager>();

            // If we used an electric fence thats part of this generator, we want to keep it off, not toggle it
            if (fenceManager.justChangedFromGenerator)
            {
                fenceManager.justChangedFromGenerator = false;
                continue;
            }

            fenceManager.SetFenceState(!fenceManager.electricIsOn);
        }
    }

    public bool CheckIfFenceInGenerator(FenceManager fenceToFind)
    {
        foreach (GameObject fence in ElectricFencesToggle)
        {
            FenceManager fenceManager = fence.GetComponentInChildren<FenceManager>();

            if (fenceManager.Equals(fenceToFind))
                return true;
        }

        return false;
    }
}
