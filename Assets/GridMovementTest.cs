/*
Test for Grid_Movement.cs
Anik Kazi
*/

using UnityEngine;
using System.Collections;
using NUnit.Framework;

// Base class for Grid_Movement
public class NewBaseType
{
    protected Grid_Movement gridMovement;

    // Test To check NextDir function
    [Test]
    public void NextDir_Test()
    {
        gridMovement = new GameObject().AddComponent<Grid_Movement>();

        gridMovement.TileCount = 0;
        Vector3 direction = gridMovement.NextDir();
        Assert.AreEqual(Vector3.right, direction, "TileCount 0 should return Vector3.right");

        gridMovement.TileCount = 10;
        direction = gridMovement.NextDir();
        Assert.AreEqual(Vector3.down, direction, "TileCount 10 should return Vector3.down");

        gridMovement.TileCount = 20;
        direction = gridMovement.NextDir();
        Assert.AreEqual(Vector3.left, direction, "TileCount 20 should return Vector3.left");

        gridMovement.TileCount = 30;
        direction = gridMovement.NextDir();
        Assert.AreEqual(Vector3.up, direction, "TileCount 30 should return Vector3.up");
    
        GameObject.Destroy(gridMovement.gameObject);
    }
}

// Derived class for Grid_Movement
public class GridMovementTest : NewBaseType
{
    private GameObject player;

    // Seting up a new player object and add Grid_Movement component
    [SetUp]
    public void Setup()
    {
        player = new GameObject();
        gridMovement = player.AddComponent<Grid_Movement>();
    }    

    // Destroying the player object
    [TearDown]
    public void Teardown()
    {
        GameObject.Destroy(player);
    }

    // Chechikg if the player is moving or not
    [UnityTest]
    public IEnumerator MovePlayerTest()
    {
        Vector3 direction = Vector3.right;
        Vector3 expectedPosition = player.transform.position;

        yield return gridMovement.StartCoroutine(gridMovement.MovePlayer(direction));

        Assert.AreNotEqual(expectedPosition, player.transform.position);
        Assert.AreEqual(expectedPosition + direction, player.transform.position);
    }
}