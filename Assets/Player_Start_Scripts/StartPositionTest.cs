/*
Test class for various start components
Anik Kazi
*/
using NUnit.Framework; // NUnit framework for writing tests
using UnityEngine;    

public class StartPositionTests
{
    [Test] // Test for Boot_Start.cs
    public void BootStart_SetsPositionCorrectly()
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

    
    [Test] // Test for Cat_Start.cs
    public void CatStart_SetsPositionCorrectly()
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

 
    [Test] // Test for Hatstand_Start.cs
    public void HatstandStart_SetsPositionCorrectly()
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

    
    [Test] // Test for Iron_Start.cs
    public void IronStart_SetsPositionCorrectly()
    {
        // Arrange; Creates a new game object and adds IronStart component to it
        var gameObj = new GameObject();
        var ironStart = gameObj.AddComponent<IronStart>();

        // Act; Calls the Start method from IronStart
        ironStart.Start();

        // Assert; Checks if the position of the game object is set correctly
        Vector3 expectedPosition = new Vector3(-4.7f, 5f, 0.125f);
        Assert.AreEqual(expectedPosition, ironStart.transform.position);
    }


    [Test] // Test for Ship_Start.cs
    public void ShipStart_SetsPositionCorrectly()
    {
        // Arrange; Creates a new game object and adds ShipStart component to it
        var gameObj = new GameObject();
        var shipStart = gameObj.AddComponent<ShipStart>();

        // Act; Calls the Start method from ShipStart
        shipStart.Start();

        // Assert; Checks if the position of the game object is set correctly
        Vector3 expectedPosition = new Vector3(-5.3f, 5f, 0.125f);
        Assert.AreEqual(expectedPosition, shipStart.transform.position);
    }

    [Test] // Test for SmartPhone_Start.cs
    public void SmartPhoneStart_SetsPositionCorrectly()
    {
        // Arrange; Creates a new game object and adds SmartPhoneStart component to it
        var gameObj = new GameObject();
        var smartPhoneStart = gameObj.AddComponent<SmartPhoneStart>();

        // Act; Calls the Start method from SmartPhoneStart
        smartPhoneStart.Start();

        // Assert; Checks if the position of the game object is set correctly
        Vector3 expectedPosition = new Vector3(-5.3f, 4.65f, 0.125f);
        Assert.AreEqual(expectedPosition, smartPhoneStart.transform.position);
    }
}
