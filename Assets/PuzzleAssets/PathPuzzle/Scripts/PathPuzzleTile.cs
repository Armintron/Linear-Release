using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPuzzleTile : MonoBehaviour
{
    public bool steppedOn = false;
    Material tileMaterial;
    Color emissiveColor;
    public AudioClip onStepSound;

    // Used to communicate between this tile, and the big daddy manager
    [HideInInspector]
    public PathPuzzle pathPuzzleManager;

    private void Start()
    {
        tileMaterial = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        emissiveColor = tileMaterial.GetColor("_EmissionColor");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Capsule")
        {
            pathPuzzleManager.TileSteppedOn(this);
            AudioSource.PlayClipAtPoint(onStepSound, this.transform.position);
        }
    }

    // Illuminates the tile
    public void TurnOnTile()
    {
        // turn on tile emission
        tileMaterial.EnableKeyword("_EMISSION");
        tileMaterial.SetColor("_EmissionColor", emissiveColor * 1);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Capsule")
            steppedOn = false;
    }
}
