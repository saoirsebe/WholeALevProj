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

    public const int maxint = 2147483647;
    public int roomsMade;
    private bool RoorsLeft = true;
    private HashSet<Vector2Int> corridors { get; set; } = new HashSet<Vector2Int>();
    public HashSet<ObjectLocation> doorsNotVisited = new HashSet<ObjectLocation>();
    static int ArrayMax = 100;
    public int[,] WeightToMoveArray = new int[ArrayMax, ArrayMax];

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
        
        for (int x = 0; x < ArrayMax; x++)
        {
            for (int y = 0; y < ArrayMax; y++)
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


        do
        {
            PickStart();
        } while (roomsMade == 3 & RoorsLeft & doorsNotVisited.Count > 0);

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
        } while (rand2 == rand);                 //While random number is the same as the location of the startRoom in roomsList pick another number

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
        List<PriorityListElement> locationsCanVisit = new List<PriorityListElement>(); 
        List<ObjectLocation> perminant = new List<ObjectLocation>();


        int[,] distanceFromStartArray = new int[ArrayMax, ArrayMax];
        for (int x = 0; x < ArrayMax; x++)
        {
            for (int y = 0; y < ArrayMax; y++)
            {
                distanceFromStartArray[x, y] = maxint;
            }
        }
        int xSet = endDoor._x;
        int ySet = endDoor._y;
        distanceFromStartArray[xSet,ySet] = 0;
        perminant.Add(endDoor);
        PriorityListElement currentPosition = new PriorityListElement(endDoor, maxint);


        do
        {
            List<PriorityListElement> adjacentLocations = FindSurrounding(distanceFromStartArray, endDoor);

            foreach (var square in adjacentLocations)
            {
                bool contains = false;
                ObjectLocation squareObj = square._thisObject;
                if (-1< squareObj._x & squareObj._x <= ArrayMax & -1< squareObj._y & squareObj._y <= ArrayMax) //if in array
                {
                    contains = true;
                }

                
                bool isPerm = perminant.Contains(squareObj);
                if (contains==true & isPerm==false) //if it an actual square in array and not perminant/visited
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
                    currentPosition = varr; 
                }
            }
            perminant.Add(currentPosition._thisObject);

            foreach (var element in locationsCanVisit)
            {
                if(element == currentPosition)
                {
                    locationsCanVisit.Remove(element);
                }
            }
            
        } while (locationsCanVisit.Count != 0);
        FindShortestPath(distanceFromStartArray, startDoor, endDoor);
    }
    private List<PriorityListElement> FindSurrounding(int[,] distanceFromStartArray, ObjectLocation MiddleDoor)
    {
        int xcurrentPosition = MiddleDoor._x;
        int ycurrentPosition = MiddleDoor._y;
        List<PriorityListElement> adjacentLocations = new List<PriorityListElement>();
        ObjectLocation leftSquareObj = new ObjectLocation(xcurrentPosition - 1, ycurrentPosition, 0);
        if(isInArray(leftSquareObj))
        {
            PriorityListElement leftSquare = new PriorityListElement(leftSquareObj, distanceFromStartArray[xcurrentPosition - 1, ycurrentPosition]);
            adjacentLocations.Add(leftSquare);
        }
        ObjectLocation rightSquareObj = new ObjectLocation(xcurrentPosition + 1, ycurrentPosition, 0);
        if (isInArray(rightSquareObj))
        {
            PriorityListElement rightSquare = new PriorityListElement(rightSquareObj, distanceFromStartArray[xcurrentPosition + 1, ycurrentPosition]);
            adjacentLocations.Add(rightSquare);
        }
        ObjectLocation forwardsquareObj = new ObjectLocation(xcurrentPosition, ycurrentPosition + 1, 0);
        if (isInArray(forwardsquareObj))
        {
            PriorityListElement forwardsquare = new PriorityListElement(forwardsquareObj, distanceFromStartArray[xcurrentPosition, ycurrentPosition + 1]);
            adjacentLocations.Add(forwardsquare);
        }
        ObjectLocation downSquareObj = new ObjectLocation(xcurrentPosition, ycurrentPosition - 1, 0);
        if (isInArray(downSquareObj))
        {
            PriorityListElement downSquare = new PriorityListElement(downSquareObj, distanceFromStartArray[xcurrentPosition, ycurrentPosition - 1]);
            adjacentLocations.Add(downSquare);
        }
        return adjacentLocations;
    }

    private bool isInArray(ObjectLocation square)
    {
        bool contains = false;
        if (-1 < square._x & square._x <= ArrayMax & -1 < square._y & square._y <= ArrayMax) //if in array
        {
            contains = true;
        }
        return contains;
    }

    private void NewWeightSetter(PriorityListElement square, PriorityListElement currentPosition, List<PriorityListElement> locationsCanVisit, bool contains, int[,] distanceFromStartArray)
    {
        ObjectLocation squareObj = square._thisObject;
        int currentWeight = distanceFromStartArray[squareObj._x, squareObj._y];
        ObjectLocation currentPositionObj = currentPosition._thisObject;
        int prevWeight = distanceFromStartArray[currentPositionObj._x, currentPositionObj._y];
        int possibleNewWeight = prevWeight + WeightToMoveArray[squareObj._x, squareObj._y];
        if (currentWeight > possibleNewWeight)
        {
            distanceFromStartArray[squareObj._x, squareObj._y] = possibleNewWeight; //If previous squares weight plus weight to move to this square is less then the weight at the square now then change weight to new weight
            foreach (var element in locationsCanVisit)
            {
                if (element == square)  //finds the square un LocationsCanVisit and removis it so new updated weight verion can be added
                {
                    locationsCanVisit.Remove(element);
                }
                square._thisWeight = possibleNewWeight;
                locationsCanVisit.Add(square);
            }
        }
        
        if (contains == false)
        {
            locationsCanVisit.Add(square);
        }
            
    }

    private void FindShortestPath(int[,] distanceFromStartArray, ObjectLocation startDoor, ObjectLocation endDoor)
    {
        bool madeShortestPath = false;
        ObjectLocation thisDoor = startDoor;

        while (madeShortestPath ==false)
        {
            List<PriorityListElement> adjacentLocations = FindSurrounding(distanceFromStartArray, thisDoor);
            int nextWeight = maxint;
            PriorityListElement nextpriorityListElement = adjacentLocations[0];
            ObjectLocation nextSquare = nextpriorityListElement._thisObject;
            foreach (var square in adjacentLocations)
            {
                ObjectLocation squareObj = square._thisObject;
                int squareWeight = square._thisWeight;
                if (squareWeight<nextWeight)
                {
                    nextWeight = squareWeight;
                    nextSquare = squareObj;
                }
                Vector2Int nextSquareVector = new Vector2Int(nextSquare._x, nextSquare._y);
                corridors.Add(nextSquareVector);
                thisDoor = nextSquare;
                if(thisDoor == endDoor)
                {
                    madeShortestPath = true;
                }
            }
        }
        
  
    }
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

    public PriorityListElement(ObjectLocation thisObject, int thisWeight)
    {
        _thisObject = thisObject;
        _thisWeight = thisWeight;
    }
}

//Must run after walls added to list/roomSpawnPointIsDone
//solution is ALevel Game "C:\Users\saoir\Documents\Unity\A Level Game\WholeALevProj\ALevelGame\ALevelGame.sln"
