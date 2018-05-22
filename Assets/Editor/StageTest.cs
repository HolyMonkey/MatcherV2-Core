using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Assets.Scripts.Model;

public class StageTest {

	[Test]
	public void SpreadedNotEqualTest()
    {
        for (int i = 0; i < 1000; i++)
        {
            int spreaded = Stage.GetSpreaded(10, 10);
            Assert.AreNotEqual(spreaded, 10);
        }
	}

    [Test]
    public void SpreadedInRangeTest()
    {
        for (int i = 0; i < 1000; i++)
        {
            int spreaded = Stage.GetSpreaded(10, 10);
            Assert.LessOrEqual(spreaded, 11);
            Assert.GreaterOrEqual(spreaded, 9);
        }
    }
}
