/*
Test script for BootStart.cs
Anik Kazi
*/
using NuGet.Frameworks; // NUnit framework for writing tests
using System;

public class BootStartTest
{
    [Test]
    public void Start_SetsPositionCorrectly()
    {
        // Arrange; Creates a new game object and adds BootStart component to it
        var gameObj = new GameObject();
        var bootStart = gameObj.AddComponent<BootStart>();

        // Act; Calls the Start method from BootStart
        bootStart.Start();

        // Assert; Checks if the position of the game object is set correctly
        Vector3 expectedPosition = new Vector3(-4.7f, 5.35f, 0.125f);
        Assert.AreEqual(expectedPosition, bootStart.transform.position);
    }
}