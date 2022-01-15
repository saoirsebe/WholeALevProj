using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{
    public GameObject[] objects;
    private Vector3 transPos;
    private int xCoord;
    private int yCoord;
    private string searchTagDoor;
    private string searchTagWallTile;
    public bool RunGenerator = false;

    private GameObject s1;
    private RMapGenorator s11;

    /// <summary>
    /// Instantiated random room from list then locates its walls and doors to add to doors List and walls List then calles AddToRoomsMade function from RMapGenerator
    /// </summary>
    void Start()
    {
        s1 = GameObject.Find("RMapGenerator");

        int locationx = (int)transform.position.x;
        int locationy = (int)transform.position.y;

        int rand = Random.Range(0, objects.Length);
        GameObject obj = Instantiate(objects[rand],transform.position, Quaternion.identity);//Spawns random room/levelTemplate from list

        string objTag = obj.name;

        if (objTag.StartsWith("R")) //if rooms point not level templatre was spawned
        {
            searchTagDoor = "Door";
            List<ObjectLocation> doors = new List<ObjectLocation>();
            s11 = s1.GetComponent<RMapGenorator>();
            doors = FindObjectswithTag(searchTagDoor, obj,doors);//Adds Door location of each door to the list doors
            
        
            Room room = new Room(locationx, locationy, doors);
            s11.AddToRoomsList(room);//Adds room location and list of its doors to roomsList
            foreach(var door in doors)//Add to doorsNotVisited
            {
                s11.AddTodoorsNotVisited(door);
            }

            searchTagWallTile = "Wall Tile";
            List<ObjectLocation> walls = new List<ObjectLocation>();
            walls = FindObjectswithTag(searchTagWallTile, obj, walls);//Adds Wall location of each Wall to the list walls
            s11.AddToWallsList(walls);//Adds wall tiles in walls to total wallsList


            s1.BroadcastMessage("AddToRoomsMade");
        }
    }

    private List<ObjectLocation> FindObjectswithTag(string _tag,GameObject obj, List<ObjectLocation> listToAdd)
    {
        listToAdd.Clear();
        Transform parent = obj.transform;

        List<ObjectLocation> listToReturn =  GetChildObject(parent, _tag, listToAdd);
        return listToReturn;
    }

    private List<ObjectLocation> GetChildObject(Transform parent, string _tag, List<ObjectLocation> listToAdd)
    {
        for(int i = 0; i < parent.childCount; i++) //for each child of current room, if game tag == "Door" then add x and y coordinates to the list doors
        {
            Transform childx = parent.GetChild(i);
            string tagg = childx.tag;
            if (tagg ==_tag)
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

}
