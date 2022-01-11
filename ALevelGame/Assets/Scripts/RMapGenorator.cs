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

    private const int maxint = 2147473647;
    private HashSet<Vector2Int> corridors { get; set; } = new HashSet<Vector2Int>();
    private List<ObjectLocation> doorsNotVisited = new List<ObjectLocation>();
    static int ArrayMax = 100;
    private int[,] WeightToMoveArray = new int[ArrayMax, ArrayMax];
    private int roomsMade;

    [SerializeField]
    private TilemapVisualiser tilemapVisualiser;

    public void AddToRoomsMade()
    {
        roomsMade += 1;
        if (roomsMade == 4)
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
                if(wallx<=100 && wally<=100)
                {
                    WeightToMoveArray[wallx, wally] = maxint; //Set locations of walls in array to max int so weight of moving ples prev location will be max and not picked
                }
            }

            ObjectLocation doorStart = new ObjectLocation(21, 15, 0);
            
            PickEnd(doorStart, maxint);
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
        while (roomsMade == 3 && doorsNotVisited.Count > 0)
        {
            PickStart(); 
        } 

    }

    private void PickStart()
    {
        int startRoomIndex = Random.Range(0, roomsList.Count-1); //call function to set seed of random at start / random does not work
        
        Room roomSt = roomsList[startRoomIndex];
        List<ObjectLocation> doorsn = roomSt._doors;
        DoorsInStart(doorsn, startRoomIndex);
    }

    private void DoorsInStart(List<ObjectLocation> doorsn, int startRoomIndex)
    {
        if(doorsn.Count>0)
        {
            foreach (var door in doorsn)
            {
                //if door was in doorsNotVisited removes and runs pickend
                int whereInDoorsNotVisited = ContainsFunction(doorsNotVisited, door);
                if (whereInDoorsNotVisited < maxint)
                {

                    doorsNotVisited.RemoveAt(whereInDoorsNotVisited);
                    PickEnd(door, startRoomIndex);
                }
            }
        }
        roomsList.RemoveAt(startRoomIndex);
    }


    private void PickEnd(ObjectLocation startDoor, int startRoomIndex)
    {
        if (doorsNotVisited.Count == 0 || roomsList.Count==0)
        {

        }
        else
        {
            ObjectLocation endDoor;
            int doorsInRoomCount=maxint;
            int doorChecking = 0;
            do
            {
                doorChecking = 0;
                List<ObjectLocation> doorsn = pickRoomFunct(startRoomIndex, roomsList);
                doorsInRoomCount = doorsn.Count;
                int rand3;
                ObjectLocation doorCheck;
                
                do
                {
                    doorChecking += 1;
                    rand3 = Random.Range(0, doorsn.Count - 1);
                    doorCheck = doorsn[rand3];
                } while (ContainsFunction(doorsNotVisited, doorCheck) == maxint && doorChecking != doorsInRoomCount + 1);//if visited already and not all doors in room have been checked and are visited
                
                endDoor = doorsn[rand3];

                if (doorChecking == doorsInRoomCount + 1)
                {
                    roomsList.RemoveAt(startRoomIndex);
                }

            } while (doorChecking == doorsInRoomCount + 1); //if it has checked every door in room and they have all been busy then fins another room

            

            
            int whereInDoorsNotVisited = ContainsFunction(doorsNotVisited, endDoor);
            if (whereInDoorsNotVisited < maxint)
            {
                doorsNotVisited.RemoveAt(whereInDoorsNotVisited);
                MakeDistanceFromEndArray(startDoor, endDoor);
            }
        }
    }

    private List<ObjectLocation> pickRoomFunct(int rand1, List<Room> roomsList)
    {
        int endRoomIndex;
        do
        {
            endRoomIndex = Random.Range(0, roomsList.Count - 1);
        } while (endRoomIndex == rand1);   //While random number is the same as the location of the startRoom in roomsList pick another number
        Room roomEn = roomsList[endRoomIndex];
        List<ObjectLocation> doorsn = roomEn._doors;
        return doorsn;
    }

    private void MakeDistanceFromEndArray(ObjectLocation startDoor,ObjectLocation endDoor)
    {
        
        List<ObjectLocation> perminant = new List<ObjectLocation>();
        List<PriorityListElement> locationsCanVisit = new List<PriorityListElement>();

        int[,] distanceFromStartArray = new int[ArrayMax, ArrayMax];
        for (int x = 0; x < ArrayMax; x++)
        {
            for (int y = 0; y < ArrayMax; y++)
            {
                distanceFromStartArray[x, y] = maxint;
            }
        }
       
        distanceFromStartArray[endDoor._x, endDoor._y] = 0;
       
        PriorityListElement currentPosition = new PriorityListElement(endDoor, maxint);
        locationsCanVisit.Add(currentPosition);
        distanceFromStartArray[startDoor._x, startDoor._y] = 0;
        ObjectLocation currentLocation = endDoor;
        while(currentLocation._x != startDoor._x || currentLocation._y != startDoor._y)
        {
            int weightToCompare = maxint;
            foreach (var varr in locationsCanVisit)   //Makes current position the position with the lowest weight
            {
                int newCurrentWeight = varr._thisWeight;  //all weights 1***?
                int isPerm = ContainsFunction(perminant, varr._thisObject);
                if (newCurrentWeight < weightToCompare && isPerm == maxint)
                {
                    currentPosition = varr; //if all the same, current weight still needs to change
                    weightToCompare = currentPosition._thisWeight;
                }
            }
            perminant.Add(currentPosition._thisObject);
            currentLocation = currentPosition._thisObject;


            List<PriorityListElement> adjacentLocations = FindSurrounding(distanceFromStartArray, currentLocation); //currentLocation out of bounds error?

            foreach (var square in adjacentLocations)
            {
                ObjectLocation squareObj = square._thisObject;

                int isPerm = ContainsFunction(perminant, squareObj);
                if (isPerm == maxint) //if not perminant/visited
                {
                    int contains1 = ContainsFunctionPLE(locationsCanVisit,square); 
                    NewWeightSetter(square, currentPosition, locationsCanVisit, contains1, distanceFromStartArray); //changes weight if new weight is smaller then current
                }
            }

        }
        FindShortestPath(distanceFromStartArray, startDoor, endDoor);
    }

    private int ContainsFunctionPLE(List<PriorityListElement> listToCheck, PriorityListElement isItemIn)
    {
        ObjectLocation isItemInObj = isItemIn._thisObject;
        int whereItContains = maxint;
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
        int whereItContains = maxint;
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
        if(IsInArray(leftSquareObj)==true)
        {
            PriorityListElement leftSquare = new PriorityListElement(leftSquareObj, distanceFromStartArray[xcurrentPosition - 1, ycurrentPosition]);
            adjacentLocations.Add(leftSquare);
        }
        ObjectLocation rightSquareObj = new ObjectLocation(xcurrentPosition + 1, ycurrentPosition, 0);
        if (IsInArray(rightSquareObj) == true)
        {
            PriorityListElement rightSquare = new PriorityListElement(rightSquareObj, distanceFromStartArray[xcurrentPosition + 1, ycurrentPosition]);
            adjacentLocations.Add(rightSquare);
        }
        ObjectLocation forwardsquareObj = new ObjectLocation(xcurrentPosition, ycurrentPosition + 1, 0);
        if (IsInArray(forwardsquareObj) == true)
        {
            PriorityListElement forwardsquare = new PriorityListElement(forwardsquareObj, distanceFromStartArray[xcurrentPosition, ycurrentPosition + 1]);
            adjacentLocations.Add(forwardsquare);
        }
        ObjectLocation downSquareObj = new ObjectLocation(xcurrentPosition, ycurrentPosition - 1, 0);
        if (IsInArray(downSquareObj) == true)
        {
            PriorityListElement downSquare = new PriorityListElement(downSquareObj, distanceFromStartArray[xcurrentPosition, ycurrentPosition - 1]);
            adjacentLocations.Add(downSquare);
        }
        return adjacentLocations;
    }

    private bool IsInArray(ObjectLocation square)
    {
        bool contains = false;
        if (-1 < square._x && square._x <= ArrayMax-1 && -1 < square._y && square._y <= ArrayMax-1) //if in array
        {
            contains = true;
        }
        return contains;
    }

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
            if (whereInLocationsCanVisit<maxint)
            {
                locationsCanVisit.RemoveAt(whereInLocationsCanVisit);
            }

            newWeightSquare._thisWeight = possibleNewWeight;
            locationsCanVisit.Add(newWeightSquare);
        }
        else
        {
            if (contains == maxint) //if surrounding tile in not in locationsCanVisit then add it
            {
                locationsCanVisit.Add(newWeightSquare);
            }
        }
    }

    private void FindShortestPath(int[,] distanceFromStartArray, ObjectLocation startDoor, ObjectLocation endDoor)
    {
        List<ObjectLocation> finalVisited = new List<ObjectLocation>();
        ObjectLocation thisDoor = startDoor;
        distanceFromStartArray[endDoor._x, endDoor._y] = 0;
        ObjectLocation nextSquare = thisDoor;

        while (thisDoor._x != endDoor._x || thisDoor._y != endDoor._y)//if hasnt reached end door
        {
            List<PriorityListElement> adjacentLocations = FindSurrounding(distanceFromStartArray, thisDoor);
            int nextWeight = maxint;
            foreach (var square in adjacentLocations)
            {
                if (square._thisWeight<nextWeight && ContainsFunction(finalVisited,square._thisObject)==maxint)
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
    }

    public void runProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = corridors;
        tilemapVisualiser.Clear();
        tilemapVisualiser.paintFloorTiles(floorPositions);
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
