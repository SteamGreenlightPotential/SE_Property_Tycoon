//Unit test cases generated with Deepseek AI 
//Prompt: Give me unit and system tests for these unity project files in their current state (Property.cs and GameManager.cs attached)
using NUnit.Framework;
using UnityEngine;
using PropertyTycoon;

public class GameManagerTests
{
    public GameManager gameManager;

    [SetUp]
    public void SetUp()
    {
        GameObject gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();
    }

    [Test]

    
    public void InitialiseProperties_Test()
    {
        // Call the method to initialize properties
        gameManager.initialiseProperties();

        // Check if properties are initialized correctly
        Assert.AreEqual(22, gameManager.properties.Count);

        // Check a few properties to ensure they are initialized correctly
        Property firstProperty = gameManager.properties[0];
        Assert.AreEqual("The Old Creek", firstProperty.name);
        Assert.AreEqual(60, firstProperty.price);
        Assert.AreEqual("Brown", firstProperty.colour);
        Assert.AreEqual(2, firstProperty.baseRent);

        Property lastProperty = gameManager.properties[21];
        Assert.AreEqual("Turing Heights", lastProperty.name);
        Assert.AreEqual(400, lastProperty.price);
        Assert.AreEqual("DBlue", lastProperty.colour);
        Assert.AreEqual(50, lastProperty.baseRent);
    }
}