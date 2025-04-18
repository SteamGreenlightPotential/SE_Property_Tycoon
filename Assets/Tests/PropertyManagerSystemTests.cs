// Some tests Generated by AI. Original prompt: Write comprehensive system tests for PropertyManager.cs
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using PropertyTycoon;
using System.Reflection;

public class PropertyManagerSystemTests
{
    private GameObject gameObject;
    private PropertyManager propertyManager;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        gameObject = new GameObject();
        propertyManager = gameObject.AddComponent<PropertyManager>();
        
        yield return null; 
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameObject);
        


    }

    // System Test: Verify properties are initialized during Awake()
    [UnityTest]
    public IEnumerator Test_Awake_InitializesProperties()
    {
        yield return null;
        Assert.AreEqual(22, propertyManager.properties.Count);
    }

    // System Test: Verify tile-based property retrieval works after initialization
    [UnityTest]
    public IEnumerator Test_GetTileProperty_AfterInitialization()
    {
        yield return null; // Ensure Awake() completes
        Property prop = propertyManager.getTileProperty(40);
        Assert.IsNotNull(prop);
        Assert.AreEqual("Turing Heights", prop.name);
        Assert.AreEqual(400, prop.price);
    }

    // System Test: Verify last property has correct attributes
    [UnityTest]
    public IEnumerator Test_LastPropertyDetailsCorrect()
    {
        yield return null; // Ensure Awake() completes
        Property lastProp = propertyManager.properties[21];
        Assert.AreEqual("Turing Heights", lastProp.name);
        Assert.AreEqual(400, lastProp.price);
        Assert.AreEqual("DBlue", lastProp.colour);
        Assert.AreEqual(40, lastProp.tileno);
    }
}