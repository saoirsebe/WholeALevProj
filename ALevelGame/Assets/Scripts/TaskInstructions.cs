using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInstructions : MonoBehaviour
{
    public GameObject InstructionsMenu;
    // Start is called before the first frame update
    private System.DateTime startTime;
    void Start()
    {
        MainMenuButton();
    }

    public void MainMenuButton()
    {
        // Show Instructions
        InstructionsMenu.SetActive(true);
    }

    public void CloseButton()
    {
        InstructionsMenu.SetActive(false);
        startTime = System.DateTime.Now;
    }
}

