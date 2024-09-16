using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickTask : MonoBehaviour
{ 
    public int objectToFindIndex;
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
    private GameObject CanvasObj;
    private TaskInstructions TaskInstructionsScript;
    private int forgotTaskObject;
    public GameObject[] objects;
    private GameObject taskIntructionObj;
    public GameObject A;

    // Start is called before the first frame update
    void Start()
    {
        forgotTaskObject = 0;
        objectsAddedToList = 0;
        textObj = GameObject.Find("ObjectText");
        taskTextScript = textObj.GetComponent<taskTextScript>();
        rMapGeneratorObj = GameObject.Find("RMapGenerator");
        RMapGenoratorScript = rMapGeneratorObj.GetComponent<RMapGenorator>();
        CanvasObj = GameObject.Find("Canvas");
        TaskInstructionsScript = CanvasObj.GetComponent<TaskInstructions>();
        taskIntructionObj = GameObject.Find("Task Instructions");
    }

    public void AddToObjectsInRoomsList(string objectNameToAdd)
    {
        objectsInRoomsList.Add(objectNameToAdd);
        objectsAddedToList += 1;


        if(RMapGenoratorScript.TOTALNOFROOMS == objectsAddedToList)
        {
            objectToFindIndex = Random.Range(0, objectsInRoomsList.Count - 1);
            objectToFindName = objectsInRoomsList[objectToFindIndex];
            ChangeTextFunction();
        }
    }

    public void ChangeTextFunction()
    {
        taskTextScript.ChangeText(objectToFindName);
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

    /// <summary>
    /// Once the object is found the time taken is found and RepeatFindObj is called
    /// </summary>
    public void TaskFinished(bool firstFind)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timeTaken = niceTime;

        if (firstFind == false)
        {
            TaskInstructionsScript.OpenSecondInstructions();
        }
        else
        {
            TaskInstructionsScript.OpenEndScreen();
        }
    }

    public void ForgotTaskObject()
    {
        forgotTaskObject += 1;
    }

    public void ShowPictureOfObject()
    {
        var instantiationPosition = new Vector3(40,11,0); //2603, 808, 0
        var countObj = 0;
        var objectPicture = new GameObject();
        string firstLet;
        string fistLetFind;
        do
        {
            objectPicture = objects[countObj];
            countObj += 1;
            firstLet = objectToFindName.Substring(0, 1);
            fistLetFind = objectPicture.name.Substring(0,1) ;
        } while (objectPicture.name.StartsWith(firstLet)==false);

        GameObject obj = Instantiate(objectPicture, instantiationPosition, Quaternion.identity, taskIntructionObj.transform);
    }
}
