using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInstructions : MonoBehaviour
{
    public GameObject InstructionsMenu;
    // Start is called before the first frame update

    private GameObject PickTaskObj;
    private PickTask PickTaskScript;

    void Start()
    {
        PickTaskObj = GameObject.Find("PickTask");
        PickTaskScript = PickTaskObj.GetComponent<PickTask>();
        MainMenuButton();
    }

    public void MainMenuButton()
    {
        // Show Instructions
        InstructionsMenu.SetActive(true);
    }

    /// <summary>
    /// CLoses instructions panel when close button is pressed and starts the timer by running startTimer function in PickTaskScript
    /// </summary>
    public void CloseButton()
    {
        InstructionsMenu.SetActive(false);
        PickTaskScript.startTimer();
    }
}

