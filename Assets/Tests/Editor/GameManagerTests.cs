//Unit test cases generated with Deepseek AI 
//Prompt: Give me unit and system tests for these unity project files in their current state (Property.cs and PropertyManager.cs attached)
using NUnit.Framework;
using UnityEngine;
using PropertyTycoon;

public class PropertyManagerTests
{
    private PropertyManager PropertyManager;

    [SetUp]
    public void SetUp()
    {
        GameObject PropertyManagerObject = new GameObject();
        PropertyManager = PropertyManagerObject.AddComponent<PropertyManager>();
    }

    [Test]

    
    public void InitialiseProperties_Test()
    {
        // Call the method to initialize properties
        PropertyManager.initialiseProperties();

        // Check if properties are initialized correctly
        Assert.AreEqual(22, PropertyManager.properties.Count);

        // Check a few properties to ensure they are initialized correctly
        Property firstProperty = PropertyManager.properties[0];
        Assert.AreEqual("The Old Creek", firstProperty.name);
        Assert.AreEqual(60, firstProperty.price);
        Assert.AreEqual("Brown", firstProperty.colour);
        Assert.AreEqual(2, firstProperty.baseRent);

        Property lastProperty = PropertyManager.properties[21];
        Assert.AreEqual("Turing Heights", lastProperty.name);
        Assert.AreEqual(400, lastProperty.price);
        Assert.AreEqual("DBlue", lastProperty.colour);
        Assert.AreEqual(50, lastProperty.baseRent);
    }
}