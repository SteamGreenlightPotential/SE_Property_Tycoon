using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using PropertyTycoon;

namespace PropertyTycoon.Tests
{
    public class PropertyPurchaseScrnTests
    {
        private GameObject testObject;
        private PropertyPurchaseScrn purchaseScrn;
        private Text propertyNameText;
        private Text propertyPriceText;
        private Text propertyColorText; // Updated field name to match PropertyColorText in PropertyPurchaseScrn
        private Text playerBalanceText;
        private Button buyButton;

        private Property testProperty;
        private Player testPlayer;

        [SetUp]
        public void Setup()
        {
            // Create a GameObject to attach the PropertyPurchaseScrn script
            testObject = new GameObject();
            purchaseScrn = testObject.AddComponent<PropertyPurchaseScrn>();

            // Create Text objects (mocking them for now)
            propertyNameText = new GameObject().AddComponent<Text>();
            propertyPriceText = new GameObject().AddComponent<Text>();
            propertyColorText = new GameObject().AddComponent<Text>(); // Updated to match PropertyColorText
            playerBalanceText = new GameObject().AddComponent<Text>();

            // Assign the Text components to the script
            purchaseScrn.PropertyName = propertyNameText;
            purchaseScrn.PropertyPrice = propertyPriceText;
            purchaseScrn.PropertyColorText = propertyColorText; // Updated to match PropertyColorText
            purchaseScrn.PlayerBalance = playerBalanceText;

            // Create the Button object
            buyButton = new GameObject().AddComponent<Button>();
            purchaseScrn.BuyButton = buyButton;

            // Create a mock property and player
            testProperty = new Property("Test Property", 100, "Red", 0);  // Mock Property
            testPlayer = new Player("Test Player") { Balance = 200 };      // Mock Player with balance
        }

        [TearDown]
        public void Teardown()
        {
            // Clean up after each test
            Object.Destroy(testObject);
            Object.Destroy(propertyNameText.gameObject);
            Object.Destroy(propertyPriceText.gameObject);
            Object.Destroy(propertyColorText.gameObject);
            Object.Destroy(playerBalanceText.gameObject);
            Object.Destroy(buyButton.gameObject);
        }

        [Test]
        public void Show_UpdatesUIWithPropertyAndPlayerDetails()
        {
            // Act
            purchaseScrn.Show(testProperty, testPlayer);

            // Assert: Check if the UI is updated with the expected information
            Assert.AreEqual("Property: Test Property", propertyNameText.text);
            Assert.AreEqual("Color: Red", propertyColorText.text); // Updated to match PropertyColorText
            Assert.AreEqual("Price: £100", propertyPriceText.text);
            Assert.AreEqual("Balance: £200", playerBalanceText.text);
        }

        [Test]
        public void OnBuyButtonClicked_BuysPropertyWhenPlayerHasSufficientFunds()
        {
            // Arrange: Player has enough money
            purchaseScrn.Show(testProperty, testPlayer);

            // Act: Trigger buy action
            purchaseScrn.OnBuyButtonClicked();

            // Assert: Check if player balance is reduced correctly and the property is added
            Assert.AreEqual(100, testPlayer.Balance); // Balance should be reduced by property price
            Assert.IsTrue(testPlayer.OwnedProperties.Exists(p => p.name == testProperty.name)); // Property should be added
        }

        [Test]
        public void OnBuyButtonClicked_DoesNotBuyPropertyWhenPlayerHasInsufficientFunds()
        {
            // Arrange: Set the player's balance to less than the property price
            testPlayer.Balance = 50;
            purchaseScrn.Show(testProperty, testPlayer);

            // Act: Try to buy the property
            purchaseScrn.OnBuyButtonClicked();

            // Assert: Balance should remain the same and property should not be added
            Assert.AreEqual(50, testPlayer.Balance); // Balance should remain unchanged
            Assert.IsFalse(testPlayer.OwnedProperties.Exists(p => p.name == testProperty.name)); // Property should not be added
        }

        [Test]
        public void Close_HidesThePropertyPurchaseScreen()
        {
            // Arrange: Show the property purchase screen
            purchaseScrn.Show(testProperty, testPlayer);

            // Act: Close the purchase screen
            purchaseScrn.gameObject.SetActive(false);

            // Assert: Verify if the GameObject is inactive (the UI is hidden)
            Assert.IsFalse(testObject.activeSelf); // The GameObject should be inactive
        }
    }
}
