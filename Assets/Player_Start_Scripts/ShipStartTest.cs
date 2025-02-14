/*
Test class for ShipStart.cs
Anik Kazi
*/

public class ShipStartTest
{
    [Test]
    public void Start_SetsPositionCorrectly()
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
}