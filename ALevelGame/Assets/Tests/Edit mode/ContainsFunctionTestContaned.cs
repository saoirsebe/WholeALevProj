using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ContainsFunctionTestContained
{
    // A Test behaves as an ordinary method
    [Test]
    public void ContainsFunction()
    {
        var listToCheck = new List<ObjectLocation>();
        listToCheck.Add(new ObjectLocation(2, 0, 0));
        listToCheck.Add(new ObjectLocation(2, 1, 0));
        listToCheck.Add(new ObjectLocation(5, 3, 0));

        var isItemIn = new ObjectLocation(2, 1, 0);

        var RMapGScript = new RMapGenorator();

        Assert.AreEqual(1, RMapGScript.ContainsFunction(listToCheck, isItemIn));
    }
}
