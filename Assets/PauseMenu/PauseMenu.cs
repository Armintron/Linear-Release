using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject pauseMenuUI;
    public GameObject puzzleSkips;
    public TextMeshProUGUI skipsButtonText;

    private bool skipsOn = false;

    private void Start()
    {
        // ensure that the timescale is 1, i.e. normal time passing
        // disable the pause menu and puzzle skips to begin with
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        puzzleSkips.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        // bring the cursor back to the game, disable the pause menu, and resume time
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    private void Pause()
    {
        // bring the cursor out of the game, enable the pause menu, and pause time
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void TogglePuzzleSkips()
    {
        // toggle skipsOn, enable/disable the puzzle skips, and change the menu text accordingly
        skipsOn = !skipsOn;
        puzzleSkips.SetActive(skipsOn);
        skipsButtonText.text = skipsOn ? "PUZZLE SKIPS OFF" : "PUZZLE SKIPS ON";
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
