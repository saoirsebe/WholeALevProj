using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FindSurroundingTestLeftOutOfArrayBounds
{
    // A Test behaves as an ordinary method
    [Test]
    public void AdjacentLocationListTest()
    {
        var RMapGScript = new RMapGenorator();

        var MiddleSquare = new ObjectLocation(0,5,0);

        int[,] distanceFromStartArray = new int[100, 100];
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                distanceFromStartArray[x, y] = 1073731823;
            }
        }

        var AdjacentLocationList = RMapGScript.FindSurrounding(distanceFromStartArray, MiddleSquare);

        var firstLoaction = AdjacentLocationList[0]; //(should be right square (1,5) instead of left square (-1,5) ad left is out of bounds or array)

        // Use the Assert class to test conditions
        Assert.AreEqual(1 , firstLoaction._thisObject._x);
        Assert.AreEqual(5, firstLoaction._thisObject._y);
        Assert.AreEqual(1073731823, firstLoaction._thisWeight);

    }
}
