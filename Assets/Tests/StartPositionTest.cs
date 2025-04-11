using NUnit.Framework;
using UnityEngine;
using PropertyTycoon;
using UnityEngine.TestTools;
using System.Reflection;

public class StartPositionTests
{
    
    [Test]
    public void BootStart_SetsPositionCorrectly()
    {
        TestPosition<Boot_start>(new Vector3((float)-4.7, (float)-4.7, (float)-0.13));
    }

    [Test]
    public void CatStart_SetsPositionCorrectly()
    {
        TestPosition<Cat_start>(new Vector3((float)-4.7, (float)-5.35, (float)-0.09));
    }

    [Test]
    public void HatstandStart_SetsPositionCorrectly()
    {
        TestPosition<Hatstand_start>(new Vector3((float)-5.3, (float)-4.6, (float)-0.19));
    }

    [Test]
    public void IronStart_SetsPositionCorrectly()
    {
        TestPosition<Iron_start>(new Vector3((float)-4.7, (float)-5, (float)-0.43));
    }

    [Test]
    public void ShipStart_SetsPositionCorrectly()
    {
        TestPosition<Ship_start>(new Vector3((float)-5.3, (float)-5, (float)-0.12));
    }

    [Test]
    public void SmartphoneStart_SetsPositionCorrectly()
    {
        TestPosition<Smartphone_start>(new Vector3((float)-5.3, (float)-5.4, (float)-0.17));
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