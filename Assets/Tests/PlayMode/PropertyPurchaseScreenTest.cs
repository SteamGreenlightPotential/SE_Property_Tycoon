using NUnit.Framework; // For unit testing
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PropertyTycoon;

public class PropertyPurchaseScrnTest
{
    private GameObject testObject; // Mock object for the PropertyPurchaseScrn
    private PropertyPurchaseScrn purchaseScreen; // Referencing the script being tested
    private Player testPlayer;
    private Property testProperty;

    [SetUp]
    public void Setup()
    {
        // Creates a mock GameObject for the PropertyPurchaseScrn
        testObject = new GameObject("PropertyPurchaseScrn");
        purchaseScreen = testObject.AddComponent<PropertyPurchaseScrn>();

        // Creates and attaches TextMeshProUGUI components
        purchaseScreen.PropertyName = CreateTextElement("PropertyName");
        purchaseScreen.PropertyPrice = CreateTextElement("PropertyPrice");
        purchaseScreen.PropertyColor = CreateTextElement("PropertyColor");
        purchaseScreen.PlayerBalance = CreateTextElement("PlayerBalance");

        // Create and attach Button components
        purchaseScreen.BuyButton = CreateButtonElement("BuyButton");

        // Create mock player and property
        testPlayer = new Player("Test Player", null)
        {
            Balance = 1000 // Starting balance for testing
        };

        testProperty = new Property("Test Property", 200, "Red", 20);
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up after each test
        Object.DestroyImmediate(testObject);
    }

    [Test]
    public void Show_UpdatesUITextCorrectly()
    {
        // Act
        purchaseScreen.Show(testProperty, testPlayer);

        // Assert
        Assert.AreEqual("Property: Test Property", purchaseScreen.PropertyName.text);
        Assert.AreEqual("Color: Red", purchaseScreen.PropertyColor.text);
        Assert.AreEqual("Price: £200", purchaseScreen.PropertyPrice.text);
        Assert.AreEqual("Balance: £1000", purchaseScreen.PlayerBalance.text);

        // Ensure the purchase screen becomes visible
        Assert.IsTrue(testObject.activeSelf);
    }

    [Test]
    public void OnBuyButtonClicked_PlayerPurchasesProperty()
    {
        // Arrange
        purchaseScreen.Show(testProperty, testPlayer);

        // Act
        purchaseScreen.OnBuyButtonClicked();

        // Assert
        Assert.AreEqual(800, testPlayer.Balance); // Check that balance is reduced
        Assert.Contains(testProperty, testPlayer.OwnedProperties); // Check that property is added
        Assert.AreEqual(testPlayer, testProperty.owner); // Check that property owner is updated
        Assert.IsFalse(testObject.activeSelf); // Check that screen is hidden
    }

    [Test]
    public void OnBuyButtonClicked_NotEnoughBalance_ShowsError()
    {
        // Arrange
        testPlayer.Balance = 100; // Set insufficient balance
        purchaseScreen.Show(testProperty, testPlayer);

        // Act
        purchaseScreen.OnBuyButtonClicked();

        // Assert
        Assert.AreEqual(100, testPlayer.Balance); // Balance should remain unchanged
        Assert.IsFalse(testPlayer.OwnedProperties.Contains(testProperty)); // Property should not be added
        Assert.IsNull(testProperty.owner); // Property owner should remain null
        Assert.IsFalse(testObject.activeSelf); // Screen should still close
    }

    // Helper method to create TextMeshProUGUI components
    private TextMeshProUGUI CreateTextElement(string name)
    {
        var textObject = new GameObject(name);
        textObject.transform.parent = testObject.transform;
        return textObject.AddComponent<TextMeshProUGUI>();
    }

    // Helper method to create Button components
    private Button CreateButtonElement(string name)
    {
        var buttonObject = new GameObject(name);
        buttonObject.transform.parent = testObject.transform;
        return buttonObject.AddComponent<Button>();
    }
}
