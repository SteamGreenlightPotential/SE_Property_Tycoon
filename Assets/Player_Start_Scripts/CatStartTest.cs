/*
Test class for CatStart.cs
Anik Kazi
*/
using Nunit.Frameworks; // NUnit framework for writing tests
using System;

public class CatStartTest
{
    [Test]
    public void Start_SetsPositionCorrectly()
    {
        // Arrange; Creates a new game object and adds CatStart component to it
        var gameObj = new GameObject();
        var catStart = gameObj.AddComponent<CatStart>();

        // Act; Calls the Start method from CatStart
        catStart.Start();

        // Assert; Checks if the position of the game object is set correctly
        Vector3 expectedPosition = new Vector3(-4.7f, 4.65f, 0.125f);
        Assert.AreEqual(expectedPosition, catStart.transform.position);
    }
}