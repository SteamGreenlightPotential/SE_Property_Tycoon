/*
Test class for HatstandStart.cs
Anik Kazi
*/
using Nunit.Frameworks; // NUnit framework for writing tests
using System;

public class HatstandStartTest
{
    [Test]
    public void Start_SetsPositionCorrectly()
    {
        // Arrange; Creates a new game object and adds HatstandStart component to it
        var gameObj = new GameObject();
        var hatstandStart = gameObj.AddComponent<HatstandStart>();

        // Act; Calls the Start method from HatstandStart
        hatstandStart.Start();

        // Assert; Checks if the position of the game object is set correctly
        Vector3 expectedPosition = new Vector3(-5.3f, 5.35f, 0.125f);
        Assert.AreEqual(expectedPosition, hatstandStart.transform.position);
    }
}