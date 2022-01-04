using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RMapGenorator : MonoBehaviour
{
    public GameObject[] nodes;
    public List<Room> roomsList = new List<Room>();
    public Room Room;
    public List<ObjectLocation> wallsList = new List<ObjectLocation>();
    
    private int[,] DistanceFromStartArray = new int[50, 50];

    public const int maxint = 2147483647;
    public int roomsMade;
    
    private GameObject startRoomObj;
    private Vector3 transPos;
    private int xCoord;
    private int yCoord;
    private int xstart;
    private int ystart;
    private int xend;
    private int yend;
    private bool roomsLeft = true;
    private List<ObjectLocation> corridors { get; set; } = new List<ObjectLocation>();
    public HashSet<ObjectLocation> doorsNotVisited = new HashSet<ObjectLocation>();
    public int[,] WeightToMoveArray = new int[50, 50];

    public void AddToRoomsList(Room room)
    {
        roomsList.Add(room);
    }

    public void AddToWallsList(List<ObjectLocation> walls)
    {
        foreach (var wallTile in walls)
        {
            wallsList.Add(wallTile);  
        }
    }

    void Start()
    {
        
        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                WeightToMoveArray[x, y] = 1;
            }
        }
        foreach (var wall in wallsList)
        {
            int wallx = wall._x;
            int wally = wall._y;
            WeightToMoveArray[wallx, wally] = maxint; //Set locations of walls in array to max int so weight of moving ples prev location will be max and not picked
        }

        ObjectLocation doorStart = new ObjectLocation(5,3,0);
        PickEnd(doorStart,maxint);


        while (roomsMade==9 & roomsLeft)
        {
            PickStart();
        }
    }

    private void PickStart()
    {
        int rand = Random.Range(0, roomsList.Count);
        Room roomSt = roomsList[rand];
        List<ObjectLocation> doorsn = roomSt.doors;
        DoorsInStart(doorsn, rand);
    }

    private void DoorsInStart(List<ObjectLocation> doorsn, int rand)
    {
        foreach (var door in doorsn)
        {

            bool contains = doorsNotVisited.Remove(door);  //returns true if door was in and removed from doorsNotVisited
            if (contains)
            {
                PickEnd(door, rand);
            }
        }
    }


    private void PickEnd(ObjectLocation startDoor, int rand)
    {
        int rand2;
        do
        {
            rand2 = Random.Range(0, roomsList.Count);
        } while (rand2 == rand);                           //While random number is the same as the location of the startRoom in roomsList pick another number

        Room roomEn = roomsList[rand2];
        List<ObjectLocation> doorsn = roomEn.doors;

        int rand3;
        rand3 = Random.Range(0, doorsn.Count);
        ObjectLocation endDoor = doorsn[rand3];

        bool contains = doorsNotVisited.Remove(endDoor);
        if (contains)
        {
            MakeDistanceFromEndArray(startDoor, endDoor);
        }
    }



    private void MakeDistanceFromEndArray(ObjectLocation startDoor,ObjectLocation endDoor)
    {
        List<PriorityListElement> locationsCanVisit = new List<PriorityListElement>();  //Change to priority queue so picks smallest distance first?
        List<ObjectLocation> perminant = new List<ObjectLocation>();


        int[,] distanceFromStartArray = new int[50, 50];
        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                distanceFromStartArray[x, y] = maxint;
            }
        }
        int xSet = endDoor._x;
        int ySet = endDoor._y;
        distanceFromStartArray[xSet,ySet] = 0;
        perminant.Add(endDoor);
        ObjectLocation currentPosition = endDoor;


        do
        {
            int xcurrentPosition = currentPosition._x;
            int ycurrentPosition = currentPosition._y;

            ObjectLocation leftSquareObj = new ObjectLocation(xcurrentPosition - 1, ycurrentPosition, 0);
            PriorityListElement leftSquare = new PriorityListElement(leftSquareObj, distanceFromStartArray[xcurrentPosition - 1, ycurrentPosition]);
            ObjectLocation rightSquareObj = new ObjectLocation(xcurrentPosition + 1, ycurrentPosition, 0);
            PriorityListElement rightSquare = new PriorityListElement(rightSquareObj, distanceFromStartArray[xcurrentPosition + 1, ycurrentPosition]);
            ObjectLocation forwardsquareObj = new ObjectLocation(xcurrentPosition, ycurrentPosition + 1, 0);
            PriorityListElement forwardsquare = new PriorityListElement(forwardsquareObj, distanceFromStartArray[xcurrentPosition, ycurrentPosition+1]);
            ObjectLocation downSquareObj = new ObjectLocation(xcurrentPosition, ycurrentPosition - 1, 0);
            PriorityListElement downSquare = new PriorityListElement(downSquareObj, distanceFromStartArray[xcurrentPosition, ycurrentPosition - 1]);

            List<PriorityListElement> adjacentLocations = new List<PriorityListElement> { leftSquare, rightSquare, forwardsquare, downSquare };

            foreach (var square in adjacentLocations)
            {
                bool contains = false;
                ObjectLocation squareObj = square._thisObject;
                if (-1< squareObj._x & squareObj._x < 51 & -1< squareObj._y & squareObj._y < 51) //if in array
                {
                    contains = true;
                }

                
                bool isPerm = perminant.Contains(squareObj);
                if (contains==true & isPerm==false) //if it an actual square in array
                {
                    bool contains1 = locationsCanVisit.Contains(square);
                    NewWeightSetter(square, currentPosition, locationsCanVisit, contains1, distanceFromStartArray); //changes weight if new weight is smaller then current
                }
            }

            foreach (var varr in locationsCanVisit)   //Makes current position the position with the lowest weight
            {
                int newCurrentWeight = varr._thisWeight;
                int thisCurrentWeight = currentPosition._thisWeight;

                if (newCurrentWeight < thisCurrentWeight)
                {
                    currentPosition = varr._thisObject; //doesnt work not same
                }
            }
            perminant.Add(currentPosition);

            foreach (var element in locationsCanVisit)
            {
                if(element._thisObject == currentPosition)
                {
                    locationsCanVisit.Remove(element);
                }
            }
            
        } while (locationsCanVisit.Count != 0);
    }

    private void NewWeightSetter(PriorityListElement square, ObjectLocation currentPosition, List<PriorityListElement> locationsCanVisit, bool contains, int[,] distanceFromStartArray)
    {
        ObjectLocation squareObj = square._thisObject;
        int xVal = squareObj._x;
        int yVal = squareObj._y;
        int currentWeight = distanceFromStartArray[xVal,yVal];
        int prevWeight = distanceFromStartArray(currentPosition);
        int possibleNewWeight = prevWeight + WeightToMoveArray(square);
        if (currentWeight > possibleNewWeight)
        {
            distanceFromStartArray[xVal,yVal] = possibleNewWeight; //If previous squares weight plus weight to move to this square is less then the weight at the square now then change weight to new weight
            if (contains == true)
            {
                foreach (var element in locationsCanVisit)
                {
                    if (element._thisObject = square)
                    {
                        locationsCanVisit.Remove(element);
                    }
                }
            }
            PriorityListElement toAddToCanVisit = new PriorityListElement(square, possibleNewWeight);
        }
        else
        {
            PriorityListElement toAddToCanVisit = new PriorityListElement(square, currentWeight);  //need to not make new instance ever time if already in 
        }
        if (contains == false)
        {
            locationsCanVisit.Add(toAddToCanVisit);
        }
            
    }

    /*private (int xstart, int ystart) GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++) //for each child of start room, if game tag == "Door" then return x and y coordinates
        {
            Transform childx = parent.GetChild(i);
            string tagg = childx.tag;
            if (tagg == _tag)
            {
                transPos = childx.position;
                xCoord = (int)transPos.x;
                yCoord = (int)transPos.y;
            }
        }
        return (xCoord, yCoord);
    }*/
}

public class Room
{
    internal List<ObjectLocation> doors;
    int _x;
    int _y;
    List<ObjectLocation> _doors;

    public Room(int x, int y, List<ObjectLocation> doors)
    {
        _x = x;
        _y = y;
        _doors = doors;
    }
}

public class ObjectLocation
{
    public int _x;
    public int _y;
    int _direction;

    public ObjectLocation(int x, int y, int direction)
    {
        _x = x;
        _y = y;
        _direction = direction;
    }
}

public class PriorityListElement
{ 
    public ObjectLocation _thisObject;
    public int _thisWeight;

    public PriorityQueueElement(ObjectLocation thisObject, int thisWeight)
    {
        _thisObject = thisObject;
        _thisWeight = thisWeight;
    }
}

//CUrrent position must be priority list element, mix of priority queue elements and objectLocations is messyy!
