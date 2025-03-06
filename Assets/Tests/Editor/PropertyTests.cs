//Unit test cases generated with Deepseek AI 
//Prompt: Give me unit and system tests for these unity project files in their current state (Property.cs and GameManager.cs attached)

using NUnit.Framework;
using PropertyTycoon;

public class PropertyTests
{
    public Property property;

    [SetUp]
    public void SetUp()
    {
        property = new Property("Test Property", 100, "Brown", 10);
    }

    [Test]

    //Ensures new properties have correct values
    public void Property_Initialization_Test()
    {
        Assert.AreEqual("Test Property", property.name);
        Assert.AreEqual(100, property.price);
        Assert.AreEqual("Brown", property.colour);
        Assert.AreEqual(10, property.baseRent);
        Assert.IsFalse(property.owned);
        Assert.IsNull(property.owner);
        Assert.AreEqual(0, property.houses);
        Assert.IsFalse(property.hotel);
    }

    [Test]


    //Tests whether values can be changed
    public void AddHouse_Test()
    {
        property.addHouse();
        Assert.AreEqual(1, property.houses);

        for (int i = 0; i < 3; i++)
        {
            property.addHouse();
        }
        Assert.AreEqual(4, property.houses);

        // Adding a fifth house should not be possible
        property.addHouse();
        Assert.AreEqual(4, property.houses);
    }

    [Test]
    public void AddHotel_Test()
    {
        // Adding a hotel without 4 houses should not work
        property.addHotel();
        Assert.IsFalse(property.hotel);

        // Adding 4 houses and then a hotel
        for (int i = 0; i < 4; i++)
        {
            property.addHouse();
        }
        property.addHotel();
        Assert.IsTrue(property.hotel);
        Assert.AreEqual(0, property.houses);
    }

    [Test]
    public void SwitchOwner_Test()
    {
        Player player = new Player("Test Player");
        property.switchOwner(player);
        Assert.AreEqual(player, property.owner);
        Assert.IsTrue(property.owned);
    }

    [Test]
    public void RemoveOwner_Test()
    {
        Player player = new Player("Test Player");
        property.switchOwner(player);
        property.removeOwner();
        Assert.IsNull(property.owner);
        Assert.IsFalse(property.owned);
        Assert.AreEqual(0, property.houses);
        Assert.IsFalse(property.hotel);
    }
}