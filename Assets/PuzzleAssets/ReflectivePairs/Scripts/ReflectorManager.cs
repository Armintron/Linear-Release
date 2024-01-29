using UnityEngine;

public class ReflectorManager : MonoBehaviour
{
    [SerializeField]
    private ReflectivePairs[] reflectivePairs;

    public UnityEngine.Events.UnityEvent toTrigger;

    // Provide each reflective pair a reference to this manager
    private void Start()
    {
        foreach (ReflectivePairs reflectivePair in reflectivePairs)
            reflectivePair.SetReflectorManager(this);
    }

    // Upon each success by the reflective pairs, we check if all of them
    // have been successfully completed
    public void CheckReflectorPairs()
    {
        bool reflectorsLocked = true;

        foreach (ReflectivePairs reflectivePair in reflectivePairs)
            reflectorsLocked &= reflectivePair.GetReflectorLocked();

        // If all of the reflectors are locked, invoke the event and turn on the
        // object spawner
        if (reflectorsLocked) toTrigger.Invoke();
    }

    public void TurnAllReflectorsOn()
    {
        foreach (ReflectivePairs reflectivePair in reflectivePairs)
        {
            reflectivePair.GetComponent<Renderer>().material.SetColor("_EmissionColor", reflectivePair.emissiveColor * 1);
            reflectivePair.SetReflectorLocked(true);
        }
    }
}
