using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickTask : MonoBehaviour
{ 
    public GameObject[] objects;
    List<string> gameObjectNames = new List<string>();
    private int objectToFindIndex;
    public string objectToFindName;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var item in objects)
        {
            string itemName = item.name;
            gameObjectNames.Add(itemName);
        }
        objectToFindIndex = Random.Range(0, gameObjectNames.Count - 1);
        objectToFindName = gameObjectNames[objectToFindIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
