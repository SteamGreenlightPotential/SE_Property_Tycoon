using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;

namespace PropertyTycoon.Tests
{
    public class AuctionScrnTests
    {
        private GameObject testObject;
        private AuctionScrn auctionScrn;
        private Text propertyNameText;
        private Text highestBidText;
        private Text playerTurnText;
        private Button bidButton;
        private Button passButton;
        private InputField bidAmountInput;

        private Property testProperty;
        private Player player1;
        private Player player2;

        [SetUp]
        public void Setup()
        {
            // Create a GameObject to attach the AuctionScrn script
            testObject = new GameObject();
            auctionScrn = testObject.AddComponent<AuctionScrn>();


            // Create Text objects (mocking them for now)
            propertyNameText = new GameObject().AddComponent<Text>();
            highestBidText = new GameObject().AddComponent<Text>();
            playerTurnText = new GameObject().AddComponent<Text>();

            // Assign the Text components to the script
            auctionScrn.PropertyNameText = propertyNameText;
            auctionScrn.HighestBidText = highestBidText;
            auctionScrn.PlayerTurnText = playerTurnText;

            // Create the Button objects
            bidButton = new GameObject().AddComponent<Button>();
            passButton = new GameObject().AddComponent<Button>();
            auctionScrn.BidButton = bidButton;
            auctionScrn.PassButton = passButton;

            // Create an InputField object for the bid amount input
            bidAmountInput = new GameObject().AddComponent<InputField>();
            auctionScrn.BidAmountInput = bidAmountInput;
            

            // Create players and a test property
            player1 = new Player("Player 1") { Balance = 500 };
            player2 = new Player("Player 2") { Balance = 400 };
            testProperty = new Property("Test Property", 100, "Red", 0);
        }

        [TearDown]
        public void Teardown()
        {
            // Clean up after each test
            Object.Destroy(testObject);
        }

        // Helper method to access private fields via reflection
        private T GetPrivateField<T>(object obj, string fieldName)
        {
            FieldInfo fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)fieldInfo.GetValue(obj);
        }

        [Test]
        public void StartAuction_InitializesUIAndStartsAuction()
        {
            // Arrange: Start the auction with the property and two players
            auctionScrn.StartAuction(testProperty, new List<Player> { player1, player2 });

            // Access private fields using reflection
            var players = GetPrivateField<List<Player>>(auctionScrn, "players");

            // Assert: Make sure the players list is initialized correctly
            Assert.AreEqual(2, players.Count);
        }

        [Test]
        public void PlaceBid_ValidBid_UpdatesBidAndNextTurn()
        {
            // Arrange: Start the auction
            auctionScrn.StartAuction(testProperty, new List<Player> { player1, player2 });
            bidAmountInput.text = "150"; // Valid bid

            // Act: Place the bid
            auctionScrn.PlaceBid();

            // Access private fields using reflection
            var currentBid = GetPrivateField<int>(auctionScrn, "currentBid");
            var highestBidder = GetPrivateField<Player>(auctionScrn, "highestBidder");

            // Assert: The bid should be updated and the next player's turn should begin
            Assert.AreEqual(150, currentBid);
            Assert.AreEqual(player1, highestBidder);
        }

        [Test]
        public void PlaceBid_InvalidBid_LowerThanCurrentBid_DoesNotUpdate()
        {
            // Arrange: Start the auction
            auctionScrn.StartAuction(testProperty, new List<Player> { player1, player2 });
            bidAmountInput.text = "50"; // Invalid bid (too low)

            // Act: Try to place the bid
            auctionScrn.PlaceBid();

            // Access private fields using reflection
            var currentBid = GetPrivateField<int>(auctionScrn, "currentBid");
            var highestBidder = GetPrivateField<Player>(auctionScrn, "highestBidder");

            // Assert: The current bid should not change
            Assert.AreEqual(100, currentBid); // Current bid should still be the original property price
            Assert.IsNull(highestBidder); // No valid highest bidder
        }

        [Test]
        public void PlaceBid_InvalidBid_InsufficientBalance_DoesNotUpdate()
        {
            // Arrange: Start the auction
            auctionScrn.StartAuction(testProperty, new List<Player> { player1, player2 });
            bidAmountInput.text = "500"; // Invalid bid (player2 can't afford it)

            // Act: Try to place the bid
            auctionScrn.PlaceBid();

            // Access private fields using reflection
            var currentBid = GetPrivateField<int>(auctionScrn, "currentBid");
            var highestBidder = GetPrivateField<Player>(auctionScrn, "highestBidder");

            // Assert: The current bid should not change
            Assert.AreEqual(100, currentBid); // Current bid should still be the original property price
            Assert.IsNull(highestBidder); // No valid highest bidder
        }

        [Test]
        public void PassTurn_PlayerPassesTurn()
        {
            // Arrange: Start the auction
            auctionScrn.StartAuction(testProperty, new List<Player> { player1, player2 });

            // Act: Player 1 passes their turn
            auctionScrn.PassTurn();

            // Access private fields using reflection
            var players = GetPrivateField<List<Player>>(auctionScrn, "players");

            // Assert: Player 1 should be removed from the auction, and Player 2 should be the current player
            Assert.AreEqual(1, players.Count); // Only one player left
        }

        [Test]
        public void EndAuction_OnlyOnePlayerLeft_EndsAuction()
        {
            // Arrange: Start the auction and pass Player 1's turn
            auctionScrn.StartAuction(testProperty, new List<Player> { player1, player2 });
            auctionScrn.PassTurn();  // Player 1 passes

            // Act: Now Player 2 is the only player, so the auction should end
            auctionScrn.PassTurn();

            // Assert: The auction should end, and Player 2 should win
            Assert.IsFalse(testObject.activeSelf); // Auction screen should be hidden
            Assert.AreEqual(300, player2.Balance); // Player 2 should have their balance deducted by the bid amount
        }

        [Test]
        public void EndAuction_NoBids_PropertyRemainsUnsold()
        {
            // Arrange: Start the auction with no bids
            auctionScrn.StartAuction(testProperty, new List<Player> { player1, player2 });
            
            // Act: Players pass their turns without bidding
            auctionScrn.PassTurn();
            auctionScrn.PassTurn();

            // Assert: Auction ends and no property is sold
            Assert.IsFalse(testObject.activeSelf); // Auction screen should be hidden
            Assert.AreEqual(500, player1.Balance); // Player 1 balance should remain the same
            Assert.AreEqual(400, player2.Balance); // Player 2 balance should remain the same
        }
    }
}
