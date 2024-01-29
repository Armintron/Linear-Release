using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class SimonSaysManager : MonoBehaviour
{
    #region Private Values
    // These are private in the sense that they do not show up in the inspector

    // References to the 4 different buttons
    private ButtonManager[] buttonManagers;
    // The current button pressed to complete logic on.
    private ButtonManager selectedButton;

    // The current colors in our sequence, with levels 1-5 (ie. 5 array positions)
    private Color[] currentSequence;
    // Indexing for the currentSequence!
    private int i = 0;
    private bool checkedCurrentButton = false;

    // Used to represent the sequence chart that we have next to the actual circle
    private Dictionary<Color, int> inputChart;
    private Dictionary<int, Color> outputChart;

    // Standard sequence - Flash each color 2 seconds and wait between each color
    // Finished current sequence - Same as victory but slower and less times
    // Failed - Flash all colors at once (maybe multiple times)
    // Victory - Flash all colors in circular pattern quickly
    [HideInInspector]
    public enum FlashMode {standard, finished, failed, victory};

    // Keeps track of whether the button has been pressed
    [HideInInspector]
    public bool buttonAlreadyPressed = false;

    private Color simonSaysButtonMaterial;
    #endregion

    #region Public Values
    // These are public in the sense that they show up in the inspector

    // Represents the current digit in the display
    [SerializeField]
    private TextMeshPro numberDisplay;

    [SerializeField]
    [Tooltip("This is how long each color flashes when FlashStandard() plays.")]
    private float standardDuration = 2.0f;

    // Editable variables for color flash duration and color flash timeToWait increment
    [SerializeField]
    [Tooltip("This is how long each color flashes when FlashFinished() plays.")]
    private float finishedDuration = 0.25f;

    [SerializeField]
    [Tooltip("This is how long the entire circle flashes when FlashFailed() plays.")]
    private float failedDuration = 1.0f;

    [SerializeField]
    [Tooltip("This is how long each color flashes when FlashVictory() plays.")]
    private float victoryDuration = 0.1f;

    [SerializeField]
    [Tooltip("This is how long is waited after Simon Says is won before activation the teleportation pad.")]
    private float activationDelay = 2f;

    // Used to trigger the event that activates the teleportation pad
    public UnityEngine.Events.UnityEvent toTrigger;

    public GameObject simonSaysButton;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        buttonManagers = new ButtonManager[4];
        for (int j = 0; j < 4; j++)
            buttonManagers[j] = transform.GetChild(j).GetComponentInChildren<ButtonManager>();

        simonSaysButtonMaterial = simonSaysButton.GetComponent<Renderer>().material.color;

        // Initializes current sequence
        currentSequence = new Color[3];

        // Set a custom color for "yellow"
        Color yellow = new Vector4(1, 1, 0, 1);

        // Given an input color, assign it an integer based on the chart in the game
        inputChart = new Dictionary<Color, int>()
        {
            { Color.red,        0 },
            {       yellow,     1 },
            { Color.green,      2 },
            { Color.blue,       3 },
        };

        // Given an integer, assign it a color based on the chart in the game
        outputChart = new Dictionary<int, Color>()
        {
            { 0,          yellow },
            { 1,    Color.green },
            { 2,    Color.red },
            { 3,    Color.blue },
        };

        AddToSequence();
    }

    void Update()
    {
        if (PauseMenu.gamePaused)
            return;

        // If we dont have a button selected OR we have any single button flashing,
        // get outta there!
        if (checkedCurrentButton || selectedButton == null || IsSimonSaysFlashing())
        {
            selectedButton = null;
            return;
        }

        selectedButton.FlashColor(2.0f);

        // Success :) <3
        if (selectedButton.GetColor().Equals(GetCorrectColor(currentSequence[i])))
        {
            GenerateNewRandoNumber();

            checkedCurrentButton = true;
            i++;
        }
        
        // Fail >:(
        else
        {
            // Deals with reseting the state of everything
            Array.Clear(currentSequence, 0, currentSequence.Length);
            numberDisplay.text = "";
            
            i = 0;

            // Deals with starting over and showing the failure animation
            AddToSequence();
            FlashSequence(FlashMode.failed);

            return;
        }

        // If we complete all 5 stages
        if (i >= currentSequence.Length)
            FlashSequence(FlashMode.victory);

        // Finished current sequence
        // (add a new color to the sequence and go back to beginning of currentSequence)
        else if (currentSequence[i].Equals(Color.clear))
        {
            // Debug.LogError("selectedColor" + selectedButton.GetColor() + "\nGetCorrectColor(currentSequence[i]): " + GetCorrectColor(currentSequence[i]) + "\nShownColor: " + currentSequence[i] + "\nOffset Number: " + numberDisplay.text);

            AddToSequence();
            FlashSequence(FlashMode.finished);

            numberDisplay.text = "";
            i = 0;
        }

        selectedButton = null;
    }

    // Goes through the sequence and flashes the respective colors
    // based on the mode given
    public void FlashSequence(FlashMode flashMode)
    {
        switch (flashMode)
        {
            case FlashMode.standard:
                FlashStandard();
                break;

            case FlashMode.finished:
                FlashFinished();
                break;

            case FlashMode.failed:
                FlashFailed();
                break;

            case FlashMode.victory:
                FlashVictory();
                break;
        }
    }

    // Tells us if a single button is flashing
    private bool IsSimonSaysFlashing()
    {
        bool currentlyFlashing = false;

        foreach (ButtonManager button in buttonManagers)
            currentlyFlashing |= button.currentlyFlashing;

        return currentlyFlashing;
    }

    // Flashes the colors within the current sequence
    private void FlashStandard()
    {
        float timeToWait = 0;

        foreach (Color color in currentSequence)
        {
            if (color.Equals(Color.clear))
                break;

            // Goes through the sequence and flashes the respective button
            foreach (ButtonManager button in buttonManagers)
                if (color.Equals(button.GetColor()))
                    button.FlashColor(standardDuration, timeToWait);

            timeToWait += (standardDuration + 0.1f);
        }

        StartCoroutine(ChangeNumberDisplay(timeToWait));
    }

    IEnumerator ChangeNumberDisplay(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        GenerateNewRandoNumber();
    }
    
    IEnumerator EnableButtonPressable(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        buttonAlreadyPressed = false;
        simonSaysButton.GetComponent<Renderer>().material.color = simonSaysButtonMaterial;
    }

    IEnumerator ActivateTeleportationPad(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        toTrigger.Invoke();
    }

    // Plays a spiral animation after finishing a single sequence
    private void FlashFinished()
    {
        float timeToWait = 0f;

        for (int j = 0; j < 3; j++)
            foreach (ButtonManager button in buttonManagers)
                button.FlashColor(finishedDuration, timeToWait += (finishedDuration + 0.01f));
                // button.FlashColor(.25f, timeToWait += .26f);

        StartCoroutine(EnableButtonPressable(timeToWait + 0.43f));
    }

    // Plays the failure animation after failing a single sequence
    private void FlashFailed()
    {
        float timeToWait = 1.1f;

        for (int j = 0; j < 3; j++)
            foreach (ButtonManager button in buttonManagers)
                button.FlashColor(failedDuration, timeToWait * j);

        StartCoroutine(EnableButtonPressable(timeToWait * 3));
    }

    // Plays the spiral animation after finishing the entire 5 sequences
    public void FlashVictory()
    {
        float timeToWait = 0f;

        for (int j = 0; j < 8; j++)
            foreach (ButtonManager button in buttonManagers)
                button.FlashColor(victoryDuration, timeToWait += (victoryDuration * 2));
        // button.FlashColor(.1f, timeToWait += .2f);

        // Coroutine activates the teleportation pad after the win animation is finished
        // timeToWait accumulates to the amount of time the win animation takes to play
        // Afterwards, add a small delay, and activate the teleportation pad
        numberDisplay.text = "";
        StartCoroutine(ActivateTeleportationPad(timeToWait + activationDelay));
        // enabled = false;
        simonSaysButton.GetComponent<SimonSaysButton>().TurnOffButton();
    }

    // Adds color to the sequence array
    private void AddToSequence()
    {
        currentSequence[i] = outputChart[UnityEngine.Random.Range(0, 4)];
    }

    // Generates random number from 1-15 and assigns it to the number display
    private void GenerateNewRandoNumber()
    {
        if (numberDisplay == null)
            return;
        
        numberDisplay.text = UnityEngine.Random.Range(1, 16).ToString();
    }

    // Assigns the current button through a call by the ButtonManager instance
    public void SetCurrentButton(ButtonManager button)
    {
        checkedCurrentButton = false;
        selectedButton = button;
    }

    // Gets the correct color based on this idea:
    // --------------------------------------------------------------- 
    // Color -> (n(#) - display(#)) % 4 -> map to outputChart -> Color
    // --------------------------------------------------------------- 
    private Color GetCorrectColor(Color inputColor)
    {
        //    map to output |  Convert color to number    |     get number from display
        return outputChart[((inputChart[inputColor] - int.Parse(numberDisplay.text)) % 4 + 4) % 4];
    }
}