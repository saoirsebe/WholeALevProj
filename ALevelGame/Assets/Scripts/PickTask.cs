using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickTask : MonoBehaviour
{ 
    public GameObject[] objects;
    List<string> gameObjectNames = new List<string>();
    private int objectToFindIndex;
    public string objectToFindName;

    private GameObject CanvasObj;
    private TaskInstructions taskInstructionScript;

    public static float timer;
    public static bool timeStarted = false;
    public string timeTaken;

    // Start is called before the first frame update
    void Start()
    {
        CanvasObj = GameObject.Find("Canvas");
        taskInstructionScript = CanvasObj.GetComponent<TaskInstructions>();

        foreach (var item in objects)
        {
            string itemName = item.name;
            gameObjectNames.Add(itemName);
        }
        objectToFindIndex = Random.Range(0, gameObjectNames.Count - 1);
        objectToFindName = gameObjectNames[objectToFindIndex];
    }

    /// <summary>
    /// Called by TaskInstructions script when task instruction box is closed to start the timer
    /// </summary>
    public void startTimer()
    {
        timeStarted = true;
    }

    void Update()
    {
        if (timeStarted == true)
        {
            timer += Time.deltaTime;
        }
    }

    // Update is called once per frame
    public void TaskFinished()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        GUI.Label(new Rect(10, 10, 250, 100), niceTime);
        timeTaken = niceTime;
    }
}
