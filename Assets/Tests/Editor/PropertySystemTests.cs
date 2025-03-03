//System test cases generated with Deepseek AI 
//Prompt: Give me unit and system tests for these unity project files in their current state (Property.cs and GameManager.cs attached)
using NUnit.Framework;
using UnityEngine;
using PropertyTycoon;

//Ensures Property and GameManager classes are interacting properly
public class SystemTests
{
    private GameManager gameManager;
    private Player player;

    [SetUp]
    public void SetUp()
    {
        GameObject gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();
        boardPlayer bplayer = new boardPlayer();
        player = new Player("Test Player",bplayer);
    }

    [Test]
    public void PropertyPurchase()
    {
        gameManager.initialiseProperties();
        Property property = gameManager.properties[0];

        // Player buys the property
        property.switchOwner(player);

        Assert.AreEqual(player, property.owner);
        Assert.IsTrue(property.owned);
    }

    [Test]
    public void PlayerSellsProperty_Test()
    {
        gameManager.initialiseProperties();
        Property property = gameManager.properties[0];

        // Player buys the property
        property.switchOwner(player);

        // Player sells the property
        property.removeOwner();

        Assert.IsNull(property.owner);
        Assert.IsFalse(property.owned);
        Assert.AreEqual(0, property.houses);
        Assert.IsFalse(property.hotel);
    }

    [Test]
    public void PlayerAddsHousesAndHotel_Test()
    {
        gameManager.initialiseProperties();
        Property property = gameManager.properties[0];

        // Player buys the property
        property.switchOwner(player);

        // Player adds houses
        for (int i = 0; i < 4; i++)
        {
            property.addHouse();
        }

        Assert.AreEqual(4, property.houses);

        // Player adds a hotel
        property.addHotel();

        Assert.IsTrue(property.hotel);
        Assert.AreEqual(0, property.houses);
    }
}