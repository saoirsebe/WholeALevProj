using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startRoomScript : MonoBehaviour
{
    private GameObject s1;
    private RMapGenorator s11;

    private Vector3 transPos;
    private int xCoord;
    private int yCoord;
    // Start is called before the first frame update
    void Start()
    {
        s1 = GameObject.Find("RMapGenerator");
        s11 = s1.GetComponent<RMapGenorator>();
        GameObject obj = this.gameObject;
        string searchTagWallTile = "Wall Tile";
        List<ObjectLocation> walls = new List<ObjectLocation>();
        walls = FindObjectswithTag(searchTagWallTile, obj, walls);//Adds Wall location of each Wall to the list walls
        s11.AddToWallsList(walls);//Adds wall tiles in walls to total wallsList
    }

    private List<ObjectLocation> FindObjectswithTag(string _tag, GameObject obj, List<ObjectLocation> listToAdd)
    {
        listToAdd.Clear();
        Transform parent = obj.transform;

        List<ObjectLocation> listToReturn = GetChildObject(parent, _tag, listToAdd);
        return listToReturn;
    }

    private List<ObjectLocation> GetChildObject(Transform parent, string _tag, List<ObjectLocation> listToAdd)
    {
        for (int i = 0; i < parent.childCount; i++) //for each child of current room, if game tag == "Door" then add x and y coordinates to the list doors
        {
            Transform childx = parent.GetChild(i);
            string tagg = childx.tag;
            if (tagg == _tag)
            {
                transPos = childx.position;

                xCoord = (int)transPos.x;
                yCoord = (int)transPos.y;

                ObjectLocation newobj = new ObjectLocation(xCoord, yCoord, 0);
                listToAdd.Add(newobj);
            }

        }
        return listToAdd;
    }

    //add its walls to wallsList
}
