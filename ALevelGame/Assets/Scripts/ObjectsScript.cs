using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsScript : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject PickTaskObj;
    private PickTask PickTaskScript;

    // Start is called before the first frame update
    void Start()
    {
        PickTaskObj = GameObject.Find("PickTask");
        PickTaskScript = PickTaskObj.GetComponent<PickTask>();

        int locationx = (int)transform.position.x;
        int locationy = (int)transform.position.y;
        int rand = Random.Range(0, objects.Length);
        GameObject obj = Instantiate(objects[rand], transform.position, Quaternion.identity);//Spawns random Object from list

        PickTaskScript.AddToObjectsInRoomsList(obj.name);
    }
}
