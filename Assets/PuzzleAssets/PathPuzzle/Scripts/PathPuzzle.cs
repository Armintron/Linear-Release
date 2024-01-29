using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPuzzle : MonoBehaviour
{
    private string[] correctPath = new[] { "StartTile", "Step1", "Step2", "Step3", "Step4", "Step5", "Step6", "Step7", "Step8", "Step9", "Step10", "Step11", "Step12", "Step13", "Step14", "Step15", "Step16", "EndTile" };
    private int counter = 0;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject puzzleKey;

    [SerializeField]
    private GameObject turret;

    private string prevTilePressed;

    public UnityEngine.Events.UnityEvent whenWon;

    private void Start()
    {
        prevTilePressed = "";

        for (int i = 0; i < 8; i++)
        {
            GameObject Row = transform.GetChild(i).gameObject;

            for (int j = 0; j < Row.transform.childCount; j++)
            {
                GameObject tile = Row.transform.GetChild(j).gameObject;
                tile.GetComponent<PathPuzzleTile>().pathPuzzleManager = this;
            }
        }
    }

    // everytime the player steps on a tile run this function
    public void TileSteppedOn(PathPuzzleTile tile)
    {
        if (counter == 0 && !tile.name.Equals("StartTile")) return;
        if (prevTilePressed.Equals(tile.name)) return;

        if (tile.name.Equals(correctPath[counter]))
        {
            prevTilePressed = tile.name;
            counter++;

            tile.TurnOnTile();

            // reached the end of the correct path
            if (counter == correctPath.Length)
            {
                PuzzleWon();
                Debug.Log("Puzzle solved!");
                return;
            }
        }

        else
        {
            // reset counter
            prevTilePressed = "";
            counter = 0;

            // turn off tile emission
            for (int i = 0; i < 8; i++)
            {
                GameObject Row = transform.GetChild(i).gameObject;

                for (int j = 0; j < Row.transform.childCount; j++)
                    Row.transform.GetChild(j).GetChild(0).gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            }

            return;
        }
    }

    // Update is called once per frame
/*    void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject Row = transform.GetChild(i).gameObject;

            for (int j = 0; j < Row.transform.childCount; j++)
            {
                GameObject tile = Row.transform.GetChild(j).gameObject;

                if (tile.GetComponent<PathPuzzleTile>().steppedOn)
                    TileSteppedOn(tile);
            }
        }          
    }*/

    void PuzzleWon()
    {
        // fun coroutine that lights up the key
        StartCoroutine(LightUpKey());
        whenWon.Invoke();
    }

    IEnumerator LightUpKey()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject RowKey = puzzleKey.transform.GetChild(i).gameObject;
            float delay = 0.75f / Mathf.Log(i + 2, 2);

            for (int j = 0; j < RowKey.transform.childCount; j++)
                RowKey.transform.GetChild(j).GetChild(0).gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

            yield return new WaitForSeconds(delay);
        }
    }
}
