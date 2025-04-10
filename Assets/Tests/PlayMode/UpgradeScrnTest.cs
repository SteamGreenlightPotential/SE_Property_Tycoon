/*
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace PropertyTycoon.Tests
{
    public class UpgradeScrnTests
    {
        private UpgradeScrn upgradeScreen;
        private GameObject panel;
        private Text message;
        private Button houseButton;
        private Button hotelButton;
        private Property property;
        private Player player;

        [SetUp]
        public void SetUp()
        {
            // Setup UpgradeScrn instance and mock elements
            var gameObject = new GameObject();
            upgradeScreen = gameObject.AddComponent<UpgradeScrn>();

            panel = new GameObject();
            panel.SetActive(false);
            upgradeScreen.OwnedPropertyPanel = panel;

            message = new GameObject().AddComponent<Text>();
            upgradeScreen.PropertyMessage = message;

            houseButton = new GameObject().AddComponent<Button>();
            upgradeScreen.UpgradeHouseButton = houseButton;

            hotelButton = new GameObject().AddComponent<Button>();
            upgradeScreen.UpgradeHotelButton = hotelButton;

            // Mock Property and Player instances
            property = new Property { name = "Test Property", houseCost = 100 };
            player = new Player { Balance = 500 };
        }

        [Test]
        public void ShowOwnedPropertyPanel_ShouldActivatePanelAndSetMessage()
        {
            upgradeScreen.ShowOwnedPropertyPanel(property, player);

            Assert.IsTrue(panel.activeSelf, "OwnedPropertyPanel was not activated.");
            Assert.AreEqual($"Welcome to {property.name}! You own this property.", message.text, "Property message was not updated correctly.");
        }

        [Test]
        public void ShowOwnedPropertyPanel_ShouldSetButtonListeners()
        {
            upgradeScreen.ShowOwnedPropertyPanel(property, player);

            Assert.IsNotNull(houseButton.onClick, "UpgradeHouseButton listeners were not set.");
            Assert.IsNotNull(hotelButton.onClick, "UpgradeHotelButton listeners were not set.");
        }

        [Test]
        public void OnUpgradeHouse_ShouldCallTryAddHouse()
        {
            var upgradeManager = new GameObject().AddComponent<UpgradeManager>();
            upgradeScreen.upgradeManager = upgradeManager;

            property.CanAddHouse = p => true; // Mock behavior
            player.CanAddHotelToSet = p => true; // Mock behavior

            upgradeScreen.ShowOwnedPropertyPanel(property, player);
            houseButton.onClick.Invoke();

            Assert.AreEqual(1, property.houses, "House was not added correctly.");
        }

        [Test]
        public void OnUpgradeHotel_ShouldCallTryAddHotel()
        {
            var upgradeManager = new GameObject().AddComponent<UpgradeManager>();
            upgradeScreen.upgradeManager = upgradeManager;

            property.CanAddHotel = p => true; // Mock behavior
            player.CanAddHotelToSet = p => true; // Mock behavior

            upgradeScreen.ShowOwnedPropertyPanel(property, player);
            hotelButton.onClick.Invoke();

            Assert.AreEqual(true, property.HasHotel, "Hotel was not added correctly.");
        }

        [Test]
        public void ClosePanel_ShouldDeactivatePanel()
        {
            panel.SetActive(true);
            upgradeScreen.ClosePanel();

            Assert.IsFalse(panel.activeSelf, "OwnedPropertyPanel was not deactivated.");
        }
    }
}
*/