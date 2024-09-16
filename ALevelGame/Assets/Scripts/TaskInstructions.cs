using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInstructions : MonoBehaviour
{
    public GameObject InstructionsMenu;
    private GameObject PickTaskObj;
    private PickTask PickTaskScript;
    public GameObject SecondInstructions;
    public GameObject endScreen;
    public GameObject forgottenObject;
    public GameObject forgottenTask;
    public GameObject[] objects;

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
        endScreen.SetActive(false);
        forgottenObject.SetActive(false);
        forgottenTask.SetActive(false);
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

    public void OpenForgottenObject()
    {
        forgottenObject.SetActive(true);
        PickTaskScript.ChangeTextFunction();
        PickTaskScript.ForgotTaskObject();
    }
    public void CloseForgottenObject()
    {
        forgottenObject.SetActive(false);
    }
    public void OpenForgottenTask()
    {
        forgottenTask.SetActive(true);
    }
    public void CloseForgottenTask()
    {
        forgottenTask.SetActive(false);
    }

    public void OpenEndScreen()
    {
        endScreen.SetActive(true);
    }

    public void ShowObjectPicture()
    {
        PickTaskScript.ShowPictureOfObject();

    }

    public void EndButton()
    {
        // Quit Game
        Application.Quit();
    }
}

