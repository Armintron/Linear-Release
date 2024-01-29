using UnityEngine;

public class FenceManager : MonoBehaviour
{
    [SerializeField]
    private bool electricityCanBeChanged = false;
    [SerializeField]
    private GameObject[] electricEffect;

    [HideInInspector]
    public bool electricIsOn;

    // Arrow variables to detect if its the same arrow, etc.
    private GameObject lastArrowThatHitThisFence = null;

    // manual override for the little subroom so a single 
    // arrow can keep toggling it
    public bool infiniteFence;
    private int numBouncesOnFenceHit;

    // Generator variables
    [SerializeField]
    public bool isGenerator = false;
    [HideInInspector]
    public bool justChangedFromGenerator = false;

    // Used in puzzles where there are a certain amount of fences that need to be on/off
    [HideInInspector]
    public FencePuzzleCounter fencePuzzleCounter;

    [SerializeField]
    private Rigidbody rigidbody;

    public LineRenderer wire;

    private void Awake()
    {
        // Determine whether or not the electricity is on
        electricIsOn = electricEffect[0].activeSelf;

        // Set the state if we have a door
        if (rigidbody != null) rigidbody.isKinematic = electricIsOn;
    }

    // When we hit a fence with the arrow, charge the arrow, and then uncharge the fence,
    // depending on if that fence is allowed to do that
    private void OnTriggerEnter(Collider other)
    {
        ArrowManager currentArrow = other.gameObject.GetComponent<ArrowManager>();

        if ((other.gameObject == lastArrowThatHitThisFence      // Using the same arrow
            && numBouncesOnFenceHit == currentArrow.numBounces) // We havent ricocheted yet
            || currentArrow == null                             // Theres no arrowManager
            || !electricityCanBeChanged)                        // Electricity not allowed to change
        {
            return;
        }

        if(!infiniteFence) lastArrowThatHitThisFence = other.gameObject;
        numBouncesOnFenceHit = currentArrow.numBounces;

        // If fence is on and arrow is not electrified
        if (!isGenerator && electricIsOn && !currentArrow.electricEnabled)
            currentArrow.SetElectricity(true);

        // If we hit a fence before, make a wire connection!
        if (currentArrow.lastFenceHit != null && !currentArrow.lastFenceHit.Equals(this))
        {
            // If true, we transfered power (yay!)
            // otherwise, we blew a fuse (oh no!)
            int fencesCompared = CompareFences(currentArrow.lastFenceHit);

            if (fencesCompared >= 0)
            {
                Debug.LogError("currentArrow.enableWire: " + currentArrow.enableWire);
                // If we want to have a wire showing
                if (currentArrow.enableWire)
                {
                    // Create a new line renderer for each segment. This is because lineRenderer is a P.O.S and cant
                    // easily separate it's segments
                    wire = Instantiate(currentArrow.wirePrefab, currentArrow.gameObject.transform).GetComponent<LineRenderer>();
                    // Set the positions
                    wire.positionCount = 2;
                    wire.SetPosition(0, currentArrow.latestPointsHit[0]);
                    wire.SetPosition(1, currentArrow.latestPointsHit[1]);
                }

                // A connection was made for a transfer of power
                if (fencesCompared >= 1)
                {
                    if (currentArrow.enableWire)
                    {
                        // We have a connection, so change the wire to yellow gradient
                        wire.startColor = wire.endColor = Color.yellow;
                        wire.enabled = true;
                    }

                    // Shift the second point to be our first when we swap fences that have electricity
                    currentArrow.latestPointsHit[0] = currentArrow.latestPointsHit[1];


                    if (fencesCompared == 2)
                    {
                        currentArrow.SetElectricity(false);
                        currentArrow.lastFenceHit = null;
                    }
                }
                // A connection was made for a destruction of power
                else if (fencesCompared == 0)
                {
                    if (currentArrow.enableWire)
                    {
                        // We have blown a fuse, so change the wire to red
                        wire.startColor = wire.endColor = Color.red;
                        wire.enabled = true;
                    }

                    // Since the previous fence will now be dull, we can start fresh
                    if (!isGenerator)
                        currentArrow.SetElectricity(false);
                   
                    currentArrow.lastFenceHit = null;
                }
            }

            if (fencePuzzleCounter) fencePuzzleCounter.CheckFences();
        }

        // Set the last fence to the current, so we can compare the states when hitting a new fence
        if (!isGenerator)
            currentArrow.lastFenceHit = this;
    }

    private int CompareFences(FenceManager other)
    {
        #region Generator case
        if (isGenerator)
        {
            if (other.electricIsOn)
            {
                other.SetFenceState(false);

                // If the generator will be toggling this again anyways, we can tell it not to. We want
                // to keep it off when used to power the generator
                if (GetComponent<ToggleElectricity>().CheckIfFenceInGenerator(other))
                    other.justChangedFromGenerator = true;

                // Blew a fuse! Oh no!
                return 0;
            }

            return -1;
        }
        #endregion

        #region Fence to Fence cases
        // If THIS fence is OFF and the other is ON
        if (!electricIsOn && other.electricIsOn)
        {
            SetFenceState(true);
            other.SetFenceState(false);

            // We transfered powah!
            return 1;
        }

        // If THIS fence is ON and the other is OFF
        else if (electricIsOn && !other.electricIsOn)
        {
            SetFenceState(false);
            other.SetFenceState(true);

            // We transfered powah!
            return 2;
        }

        // If THIS fence is ON and the other is ON
        else if (electricIsOn && other.electricIsOn)
        {
            SetFenceState(false);
            other.SetFenceState(false);

            // Blew a fuse! Oh no!
            return 0;
        }

        return -1;

        #endregion
    }

    // Allows us to set the state of the fence to be either on or off
    public void SetFenceState(bool state)
    {
        foreach (GameObject effect in electricEffect)
            effect.SetActive(state);

        electricIsOn = state;

        // For the door, we dont want to be able to push it if its electrified
        if (rigidbody != null)
            rigidbody.isKinematic = state;
    }
}
