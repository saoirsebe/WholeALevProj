using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickTask : MonoBehaviour
{ 
    private int objectToFindIndex;
    public string objectToFindName;
    public static float timer;
    public static bool timeStarted = false;
    public string timeTaken;
    private GameObject rMapGeneratorObj;
    private RMapGenorator RMapGenoratorScript;
    private GameObject textObj;
    private taskTextScript taskTextScript;
    public List<string> objectsInRoomsList = new List<string>();
    private int objectsAddedToList;

    // Start is called before the first frame update
    void Start()
    {
        objectsAddedToList = 0;
        textObj = GameObject.Find("ObjectText");
        taskTextScript = textObj.GetComponent<taskTextScript>();
        rMapGeneratorObj = GameObject.Find("RMapGenerator");
        RMapGenoratorScript = textObj.GetComponent<RMapGenorator>();
    }

    public void AddToObjectsInRoomsList(string objectNameToAdd)
    {
        objectsInRoomsList.Add(objectNameToAdd);
        objectsAddedToList += 1;

        if(RMapGenoratorScript.totalNOfDoors== objectsAddedToList)
        {
            objectToFindIndex = Random.Range(0, objectsInRoomsList.Count - 1);
            objectToFindName = objectsInRoomsList[objectToFindIndex];
            taskTextScript.ChangeText(objectToFindName);
        }
        
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
