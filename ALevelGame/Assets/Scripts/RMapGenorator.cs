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

    private const int MAXINT = 2147473647;
    private HashSet<Vector2Int> corridors { get; set; } = new HashSet<Vector2Int>();
    private List<ObjectLocation> doorsNotVisited = new List<ObjectLocation>();
    public const int ARRAYMAX = 100;
    private int[,] WeightToMoveArray = new int[ARRAYMAX, ARRAYMAX];
    private int roomsMade;

    [SerializeField]
    private TileMapVisualiser tilemapVisualiser;
    private GameObject thisScript;
    private TileMapVisualiser nextScript;
    private List<ObjectLocation> doorsVisited = new List<ObjectLocation>();

    /// <summary>
    /// 
    /// </summary>
    public void AddToRoomsMade()
    {
        roomsMade += 1;
        if (roomsMade == 4)
        {
            for (int x = 0; x < ARRAYMAX; x++)
            {
                for (int y = 0; y < ARRAYMAX; y++)
                {
                    WeightToMoveArray[x, y] = 1;
                }
            }
            foreach (var wall in wallsList)
            {
                int wallx = wall._x;
                int wally = wall._y;
                if(wallx<=100 && wally<=100)
                {
                    WeightToMoveArray[wallx, wally] = MAXINT; //Set locations of walls in array to max int so weight of moving ples prev location will be max and not picked
                }
            }
            ObjectLocation doorStart = new ObjectLocation(21, 15, 0);
            
            PickEnd(doorStart);
            StartShortestPathAlgorithm();
        }
    }

    public void AddToRoomsList(Room room)
    {
        roomsList.Add(room);
    }

    public void AddTodoorsNotVisited(ObjectLocation door)
    {
        doorsNotVisited.Add(door);
    }

    public void AddToWallsList(List<ObjectLocation> walls)
    {
        foreach (var wallTile in walls)
        {
            wallsList.Add(wallTile);  
        }
    }

    public void StartShortestPathAlgorithm()
    {
        //Picks start room after 3 rooms are made and there are still doors to visit
        while (roomsMade == 4 && doorsNotVisited.Count > 0)
        {
            PickStart(); 
        } 

    }

    private void PickStart()
    {
        int startRoomIndex = Random.Range(0, roomsList.Count-1); //call function to set seed of random at start / random does not work
        Room roomSt = roomsList[startRoomIndex];
        List<ObjectLocation> doorsn = roomSt._doors;
        roomsList.RemoveAt(startRoomIndex);
        DoorsInStart(doorsn);
    }

    private void DoorsInStart(List<ObjectLocation> doorsn)
    {
        if(doorsn.Count>0)
        {
            //for each door in the room: remove from doorsn, remove from doorsNotVisited and add to doorsVisited
            foreach (var door in doorsn)
            {
                //if door was in doorsNotVisited removes and runs pickend
                int whereInDoorsN = ContainsFunction(doorsn, door);
                doorsn.RemoveAt(whereInDoorsN);
                int whereInDoorsNotVisited = ContainsFunction(doorsNotVisited, door);
                doorsNotVisited.RemoveAt(whereInDoorsNotVisited);
                doorsVisited.Add(door);
                PickEnd(door);
                
            }
        }
    }

    /// <summary>
    /// Picks the end room and the end door in that room and then runs MakeDistanceFromEndArray
    /// </summary>
    /// <param name="startDoor"></param>
    private void PickEnd(ObjectLocation startDoor)
    {
        ObjectLocation endDoor;
        if (doorsNotVisited.Count == 0 || roomsList.Count == 0)
        {
            int endDoorIndex = Random.Range(0, doorsVisited.Count - 1);
            endDoor = doorsVisited[endDoorIndex];
        }
        else//picks end door and room from roomslist and doors in that room
        {
            int endRoomIndex = Random.Range(0, roomsList.Count - 1);
            Room roomEn = roomsList[endRoomIndex];
            List<ObjectLocation> doorsn = roomEn._doors;
            int endDoorIndex = Random.Range(0, doorsn.Count - 1);
            endDoor = doorsn[endDoorIndex];
            doorsn.RemoveAt(endDoorIndex);

            //if there is now no doors in this room then remove it from the list of possible rooms to visit
            if (doorsn.Count == 0)
            {
                roomsList.RemoveAt(endRoomIndex);
            }
            doorsVisited.Add(endDoor);
        }
        

        int whereInDoorsNotVisited = ContainsFunction(doorsNotVisited, endDoor);
        if (whereInDoorsNotVisited < MAXINT)
        {
            doorsNotVisited.RemoveAt(whereInDoorsNotVisited);
        }

        MakeDistanceFromEndArray(startDoor, endDoor);
    }

    /// <summary>
    /// Starts at endDoor and makes a distanceFromStartArray by stepping one each time and updating the next weight if the weight of previous plus weight to move is less then the weight there
    /// </summary>
    /// <param name="startDoor"></param>
    /// <param name="endDoor"></param>
    private void MakeDistanceFromEndArray(ObjectLocation startDoor,ObjectLocation endDoor)
    {
        List<ObjectLocation> perminant = new List<ObjectLocation>();
        List<PriorityListElement> locationsCanVisit = new List<PriorityListElement>();

        int[,] distanceFromStartArray = MakeEmptyMakeDistanceFromEndArray(); //Makes array ARRAYMAX by ARRAYMAX where all squares are MAXINT

        //Validation
        if (IsInBoundsOfArray(endDoor)==true)
        {
            distanceFromStartArray[endDoor._x, endDoor._y] = 0;
        }
        else
        {
            Debug.LogError("MakeDistanceFromEndArray: endDoor out of bounds of array");
        }
        
        //Starts at endDoor:
        PriorityListElement currentPosition = new PriorityListElement(endDoor, MAXINT);
        locationsCanVisit.Add(currentPosition);
        ObjectLocation currentLocation = endDoor;


        while(perminant.Count<10000)
        {
            int weightToCompare = MAXINT+200;
            foreach (var varr in locationsCanVisit)   //Makes current position the position with the lowest weight
            {
                int newCurrentWeight = varr._thisWeight;  //all weights 1***?
                int isPerm = ContainsFunction(perminant, varr._thisObject);
                if (newCurrentWeight < weightToCompare && isPerm == MAXINT)
                {
                    currentPosition = varr; //if all the same, current weight still needs to change
                    weightToCompare = currentPosition._thisWeight;
                }
            }
            perminant.Add(currentPosition._thisObject);
            currentLocation = currentPosition._thisObject;
            locationsCanVisit.RemoveAt(ContainsFunctionPLE(locationsCanVisit,currentPosition));


            List<PriorityListElement> adjacentLocations = FindSurrounding(distanceFromStartArray, currentLocation); //currentLocation out of bounds error?

            foreach (var square in adjacentLocations)
            {
                ObjectLocation squareObj = square._thisObject;

                int isPerm = ContainsFunction(perminant, squareObj);
                if (isPerm == MAXINT) //if not perminant/visited
                {
                    int contains1 = ContainsFunctionPLE(locationsCanVisit,square); 
                    NewWeightSetter(square, currentPosition, locationsCanVisit, contains1, distanceFromStartArray); //changes weight if new weight is smaller then current
                }
            }
        }
        FindShortestPath(distanceFromStartArray, startDoor, endDoor);
    }

    private int[,] MakeEmptyMakeDistanceFromEndArray()
    {
        int[,] distanceFromStartArray = new int[ARRAYMAX, ARRAYMAX];
        for (int x = 0; x < ARRAYMAX; x++)
        {
            for (int y = 0; y < ARRAYMAX; y++)
            {
                distanceFromStartArray[x, y] = MAXINT;
            }
        }
        return distanceFromStartArray;
    }

    private int ContainsFunctionPLE(List<PriorityListElement> listToCheck, PriorityListElement isItemIn)
    {
        ObjectLocation isItemInObj = isItemIn._thisObject;
        int whereItContains = MAXINT;
        int counter = 0;
        foreach (var item in listToCheck)
        {
            ObjectLocation itemObj = item._thisObject;
            if (itemObj._x == isItemInObj._x && itemObj._y == isItemInObj._y)
            {
                whereItContains = counter;
            }
            counter += 1;
        }
        return whereItContains;
    }

    private int ContainsFunction(List<ObjectLocation> listToCheck,ObjectLocation isItemIn)//return where in list it is and maxint is its not
    {
        int whereItContains = MAXINT;
        int counter = 0;
        foreach (var item in listToCheck)
        {
            if(item._x == isItemIn._x && item._y == isItemIn._y)
            {
                whereItContains = counter;
            }
            counter += 1;
        }
        return whereItContains;
    }

    private List<PriorityListElement> FindSurrounding(int[,] distanceFromStartArray, ObjectLocation MiddleDoor)
    {
        int xcurrentPosition = MiddleDoor._x;
        int ycurrentPosition = MiddleDoor._y;
        List<PriorityListElement> adjacentLocations = new List<PriorityListElement>();
        ObjectLocation leftSquareObj = new ObjectLocation(xcurrentPosition - 1, ycurrentPosition, 0);
        if(IsInBoundsOfArray(leftSquareObj)==true)
        {
            PriorityListElement leftSquare = new PriorityListElement(leftSquareObj, distanceFromStartArray[xcurrentPosition - 1, ycurrentPosition]);
            adjacentLocations.Add(leftSquare);
        }
        ObjectLocation rightSquareObj = new ObjectLocation(xcurrentPosition + 1, ycurrentPosition, 0);
        if (IsInBoundsOfArray(rightSquareObj) == true)
        {
            PriorityListElement rightSquare = new PriorityListElement(rightSquareObj, distanceFromStartArray[xcurrentPosition + 1, ycurrentPosition]);
            adjacentLocations.Add(rightSquare);
        }
        ObjectLocation forwardsquareObj = new ObjectLocation(xcurrentPosition, ycurrentPosition + 1, 0);
        if (IsInBoundsOfArray(forwardsquareObj) == true)
        {
            PriorityListElement forwardsquare = new PriorityListElement(forwardsquareObj, distanceFromStartArray[xcurrentPosition, ycurrentPosition + 1]);
            adjacentLocations.Add(forwardsquare);
        }
        ObjectLocation downSquareObj = new ObjectLocation(xcurrentPosition, ycurrentPosition - 1, 0);
        if (IsInBoundsOfArray(downSquareObj) == true)
        {
            PriorityListElement downSquare = new PriorityListElement(downSquareObj, distanceFromStartArray[xcurrentPosition, ycurrentPosition - 1]);
            adjacentLocations.Add(downSquare);
        }
        return adjacentLocations;
    }

    /// <summary> 
    /// Checks if square is in the bounds of the array (returns true or false)
    /// </summary>
    /// <param name="square"></param> The square that is checked if it is in the array
    /// <returns></returns>
    private bool IsInBoundsOfArray(ObjectLocation square)
    {
        bool contains = false;
        if (-1 < square._x && square._x <= ARRAYMAX-1 && -1 < square._y && square._y <= ARRAYMAX-1) //if in array
        {
            contains = true;
        }
        return contains;
    }


    /// <summary> 
    /// Sets a new weight (in distanceFromStartArray) to the location if the location that has just been visited plus the weight of moving to that location is less then the weight already at the location
    /// </summary>
    /// <param name="newWeightSquare"></param> The square that may need changing weight
    /// <param name="currentPosition"></param> The current position thet the algorithm has visited
    /// <param name="locationsCanVisit"></param> List of locations that are adjacent to the visited locations
    /// <param name="contains"></param>
    /// <param name="distanceFromStartArray"></param>
    private void NewWeightSetter(PriorityListElement newWeightSquare, PriorityListElement currentPosition, List<PriorityListElement> locationsCanVisit, int contains, int[,] distanceFromStartArray)
    {
        ObjectLocation squareObj = newWeightSquare._thisObject; //currentposition and square are equal**** sometimes (contains function prob)
        int squarex = squareObj._x;
        int squarey = squareObj._y;

        int currentWeight = distanceFromStartArray[squarex, squarey];    //currentweight always 1
        ObjectLocation currentPositionObj = currentPosition._thisObject;
        int prevWeight = distanceFromStartArray[currentPositionObj._x, currentPositionObj._y];
        int possibleNewWeight = prevWeight + WeightToMoveArray[squarex, squarey];
        if (currentWeight > possibleNewWeight)
        {
            distanceFromStartArray[squarex, squarey] = possibleNewWeight; //If previous squares weight plus weight to move to this square is less then the weight at the square now then change weight to new weight

            int whereInLocationsCanVisit = ContainsFunctionPLE(locationsCanVisit, newWeightSquare);
            if (whereInLocationsCanVisit<MAXINT)
            {
                locationsCanVisit.RemoveAt(whereInLocationsCanVisit);
            }

            newWeightSquare._thisWeight = possibleNewWeight;
            locationsCanVisit.Add(newWeightSquare);
        }
        else
        {
            if (contains == MAXINT) //if surrounding tile in not in locationsCanVisit then add it
            {
                locationsCanVisit.Add(newWeightSquare);
            }
        }
    }

    /// <summary> 
    /// Steps through distanceFromStartArray starting at startDoor and picking the lowest weight adjacent square each time untill the endDoor is reached
    /// </summary>
    /// <param name="distanceFromStartArray"></param> The array that tells you the number of steps of each square away from start door
    /// <param name="startDoor"></param> The door that the shortest path finder starts at
    /// <param name="endDoor"></param> The door that the shortest path finder ends at
    private void FindShortestPath(int[,] distanceFromStartArray, ObjectLocation startDoor, ObjectLocation endDoor)
    {
        List<ObjectLocation> finalVisited = new List<ObjectLocation>();
        ObjectLocation thisDoor = startDoor;
        distanceFromStartArray[startDoor._x, startDoor._y] = MAXINT;
        distanceFromStartArray[endDoor._x, endDoor._y] = 0;
        ObjectLocation nextSquare = thisDoor;

        while (thisDoor._x != endDoor._x || thisDoor._y != endDoor._y)//if hasnt reached end door
        {
            List<PriorityListElement> adjacentLocations = FindSurrounding(distanceFromStartArray, thisDoor);

            int nextWeight = MAXINT;
            foreach (var square in adjacentLocations)
            {
                if (square._thisWeight<nextWeight && ContainsFunction(finalVisited,square._thisObject)==MAXINT)
                {
                    nextWeight = square._thisWeight;
                    nextSquare = square._thisObject;
                }
            }
            Vector2Int nextSquareVector = new Vector2Int(nextSquare._x, nextSquare._y);
            corridors.Add(nextSquareVector);
            thisDoor = nextSquare;
            finalVisited.Add(thisDoor);
        }
        runProceduralGeneration(corridors);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="floorPositions"></param>
    public void runProceduralGeneration(HashSet<Vector2Int> floorPositions)
    {
        thisScript = GameObject.Find("TileMapVisualiser");
        nextScript = thisScript.GetComponent<TileMapVisualiser>(); // breakpoint stops but wont go to next breakpoint (object reference not set to an instance of an object)
        IEnumerable<Vector2Int> positions = floorPositions; 
        nextScript.paintFloorTiles(positions); 
        
    }
}



public class Room
{
    int _x;
    int _y;
    public List<ObjectLocation> _doors;

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
