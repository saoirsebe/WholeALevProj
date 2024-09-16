using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FindSurroundingTestDifferentWeights
{
    // A Test behaves as an ordinary method
    [Test]
    public void AdjacentLocationListTest()
    {
        var RMapGScript = new RMapGenorator();

        var MiddleSquare = new ObjectLocation(20,51,0);

        int[,] distanceFromStartArray = new int[100, 100];
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                distanceFromStartArray[x, y] = 1073731823;
            }
        }
        distanceFromStartArray[19,51] = 5; //Left
        distanceFromStartArray[20, 52] = 21; //Up
        distanceFromStartArray[20, 50] = 82; //Down



        var AdjacentLocationList = RMapGScript.FindSurrounding(distanceFromStartArray, MiddleSquare);

        var firstLoaction = AdjacentLocationList[0];
        var secondLocation = AdjacentLocationList[1];
        var thirdLocation = AdjacentLocationList[2];
        var forthLocation = AdjacentLocationList[3];


        // Use the Assert class to test conditions
        Assert.AreEqual(5, firstLoaction._thisWeight);
        Assert.AreEqual(1073731823, secondLocation._thisWeight);
        Assert.AreEqual(21, thirdLocation._thisWeight);
        Assert.AreEqual(82, forthLocation._thisWeight);

    }
}
