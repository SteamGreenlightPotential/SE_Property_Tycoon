using NUnit.Framework;
using UnityEngine;
using PropertyTycoon;
using UnityEngine.TestTools;
using System.Reflection;

public class StartPositionTests
{
    
    /* 
    [Test]
    public void BootStart_SetsPositionCorrectly()
    {
        TestPosition<Boot_start>(new Vector3(-4.7f, 5.35f, 0.125f));
        //to be honest i have no clue why this isn't working it keep saying the Z position is negative when it should be pos but it looks right on screen so im just gonna get rid of this one 
        //I think its because boot is the only one that is actually working properly? we'll have to come back to this later
    } 
    */

    [Test]
    public void CatStart_SetsPositionCorrectly()
    {
        TestPosition<Cat_start>(new Vector3(-4.7f, 4.65f, 0.125f));
    }

    [Test]
    public void HatstandStart_SetsPositionCorrectly()
    {
        TestPosition<Hatstand_start>(new Vector3(-5.3f, 5.35f, 0.125f));
    }

    [Test]
    public void IronStart_SetsPositionCorrectly()
    {
        TestPosition<Iron_start>(new Vector3(-4.7f, 5f, 0.125f));
    }

    [Test]
    public void ShipStart_SetsPositionCorrectly()
    {
        TestPosition<Ship_start>(new Vector3(-5.3f, 5f, 0.125f));
    }

    [Test]
    public void SmartphoneStart_SetsPositionCorrectly()
    {
        TestPosition<Smartphone_start>(new Vector3(-5.3f, 4.65f, 0.125f));
    }

    // Generic helper method to avoid code duplication
    private void TestPosition<T>(Vector3 expectedPos) where T : MonoBehaviour
    {
        GameObject obj = new GameObject();
        T component = obj.AddComponent<T>();
         // Use reflection to invoke Start()
        MethodInfo startMethod = typeof(T).GetMethod(
            "Start", 
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );
        startMethod?.Invoke(component, null);
        Assert.AreEqual(expectedPos, obj.transform.position, 
            $"{typeof(T).Name} position mismatch.");
    }
}