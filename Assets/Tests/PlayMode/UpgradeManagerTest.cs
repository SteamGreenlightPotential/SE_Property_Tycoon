/*
using NUnit.Framework;

namespace PropertyTycoon.Tests
{
    public class UpgradeManagerTests
    {
        private UpgradeManager upgradeManager;
        private Property property;
        private Player player;

        [SetUp]
        public void SetUp()
        {
            upgradeManager = new UpgradeManager();
            property = new Property { name = "Test Property", houseCost = 100 };
            player = new Player { Balance = 1000 };
        }

        [Test]
        public void TryAddHouse_ShouldAddHouse_WhenConditionsMet()
        {
            property.CanAddHouse = p => true; // Mock behavior
            player.CanAddHotelToSet = p => true; // Mock behavior
            
            upgradeManager.TryAddHouse(property, player);
            
            Assert.AreEqual(1, property.houses, "House was not added as expected.");
            Assert.AreEqual(900, player.Balance, "Player balance was not updated correctly.");
        }

        [Test]
        public void TryAddHotel_ShouldAddHotel_WhenConditionsMet()
        {
            property.CanAddHotel = p => true; // Mock behavior
            player.CanAddHotelToSet = p => true; // Mock behavior
            
            upgradeManager.TryAddHotel(property, player);
            
            Assert.AreEqual(true, property.HasHotel, "Hotel was not added as expected.");
            Assert.AreEqual(500, player.Balance, "Player balance was not updated correctly.");
        }

        [Test]
        public void TryAddHouse_ShouldNotAddHouse_WhenFundsInsufficient()
        {
            player.Balance = 50; // Insufficient funds
            
            upgradeManager.TryAddHouse(property, player);
            
            Assert.AreEqual(0, property.houses, "House was added despite insufficient funds.");
        }

        [Test]
        public void TryAddHotel_ShouldNotAddHotel_WhenFundsInsufficient()
        {
            player.Balance = 100; // Insufficient funds
            
            upgradeManager.TryAddHotel(property, player);
            
            Assert.AreEqual(false, property.HasHotel, "Hotel was added despite insufficient funds.");
        }
    }
}
*/