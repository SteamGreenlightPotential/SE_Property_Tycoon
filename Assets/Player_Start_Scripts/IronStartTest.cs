/*
Test class for IronStart.cs
Anik Kazi
*/

public class IronStartTest
{
    [Test]
    public void Start_SetsPositionCorrectly()
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
}