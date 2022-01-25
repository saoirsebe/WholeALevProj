using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInstructions : MonoBehaviour
{
    public GameObject InstructionsMenu;
    private GameObject PickTaskObj;
    private PickTask PickTaskScript;
    public GameObject SecondInstructions;
    public GameObject EndScreen;

    void Start()
    {
        PickTaskObj = GameObject.Find("PickTask");
        PickTaskScript = PickTaskObj.GetComponent<PickTask>();
        MainMenuButton();
    }

    /// <summary>
    /// Show instructions and not the second instructions or the end screen
    /// </summary>
    public void MainMenuButton()
    {
        // Show Instructions
        InstructionsMenu.SetActive(true);
        SecondInstructions.SetActive(false);
        EndScreen.SetActive(false);
    }

    /// <summary>
    /// CLoses instructions panel when close button is pressed and starts the timer by running startTimer function in PickTaskScript
    /// </summary>
    public void CloseButton()
    {
        InstructionsMenu.SetActive(false);
        PickTaskScript.startTimer();
    }

    public void CloseSecondButton()
    {
        SecondInstructions.SetActive(false);
        PickTaskScript.startTimer();
    }

    public void OpenSecondInstructions()
    {
        SecondInstructions.SetActive(true);
    }

    public void OpenEndScreen()
    {
        EndScreen.SetActive(true);
    }

    public void EndButton()
    {
        // Quit Game
        Application.Quit();
    }
}

