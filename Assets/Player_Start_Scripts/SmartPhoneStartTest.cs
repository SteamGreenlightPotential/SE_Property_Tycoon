/*
Test class for SmartPhoneStart.cs
Anik Kazi
*/

public class SmartPhoneStartTest
{
    [Test]
    public void Start_SetsPositionCorrectly()
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