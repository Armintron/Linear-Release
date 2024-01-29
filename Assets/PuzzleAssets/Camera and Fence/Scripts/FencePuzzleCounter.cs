using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FencePuzzleCounter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] fencesToCheck;
    [SerializeField]
    private GameObject[] otherFences;

    [SerializeField]
    private TextMeshPro counter;

    public UnityEvent OnPuzzleFinish;

    private void Start()
    {
        // Provide each fence a reference to this counter so when they change,
        // the number changes too
        foreach (GameObject fence in fencesToCheck)
        {
            FenceManager fenceManager = fence.GetComponentInChildren<FenceManager>();
            fenceManager.fencePuzzleCounter = this;
        }

        foreach (GameObject fence in otherFences)
        {
            FenceManager fenceManager = fence.GetComponentInChildren<FenceManager>();
            fenceManager.fencePuzzleCounter = this;
        }

        CheckFences();
    }

    // Get the total number of on fences and send that number to the counter.
    public void CheckFences()
    {
        int numFencesOff = 0;

        // Count the number of on fences
        foreach (GameObject fence in fencesToCheck)
        {
            FenceManager fenceManager = fence.GetComponentInChildren<FenceManager>();

            if (!fenceManager.isGenerator)
                numFencesOff += fenceManager.electricIsOn ? 0 : 1;
        }

        Debug.Log("numFencesOff: " +numFencesOff);

        // Set the text to the number of on fences
        counter.text = numFencesOff.ToString();

        // If we have no electric fences on, we win!
        if (numFencesOff == 0)
            AllFencesOn();
    }

    // Called when we finish the puzzle by electrifying all supplied fences
    private void AllFencesOn()
    {
        OnPuzzleFinish.Invoke();
    }
}
