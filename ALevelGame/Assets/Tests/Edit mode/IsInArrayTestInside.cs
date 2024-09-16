using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class IsInArrayTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void IsInBoundsOfArray()
    {
        var square = new ObjectLocation(5,7,0);

        var RMapGScript = new RMapGenorator();

        // Use the Assert class to test conditions
        Assert.AreEqual(true, RMapGScript.IsInBoundsOfArray(square));
    }
}
