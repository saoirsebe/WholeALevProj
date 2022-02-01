using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class IsInArrayTestOutsideNegative
{
    // A Test behaves as an ordinary method
    [Test]
    public void IsInBoundsOfArray()
    {
        var square = new ObjectLocation(-1,6,0);

        var RMapGScript = new RMapGenorator();

        // Use the Assert class to test conditions
        Assert.AreEqual(false, RMapGScript.IsInBoundsOfArray(square));
    }
}