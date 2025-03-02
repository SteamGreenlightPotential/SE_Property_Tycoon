using NUnit.Framework;
using System.Collections.Generic;

namespace PropertyTycoon.Tests
{
    [TestFixture]
    public class GameManagerTests
    {
        private GameManager gameManager;

        [SetUp]
        public void Setup()
        {
            // Initialize instance of GameManager for each test
            gameManager = new GameManager();
            gameManager.Awake(); // Simulates the Unity lifecycle for setup
        }

        [Test]
        public void InitialiseProperties_ShouldLoadAllProperties()
        {
            // Act
            gameManager.InitialiseProperties();

            // Assert
            Assert.AreEqual(22, gameManager.properties.Count); // 22 properties in the game
        }

        [Test]
        public void AddPlayer_ShouldAddPlayerToGame()
        {
            // Act
            gameManager.AddPlayer("Test Player");

            // Assert
            Assert.AreEqual(1, gameManager.players.Count);
            Assert.AreEqual("Test Player", gameManager.players[0].Name);
        }

        [Test]
        public void PerformTransaction_ShouldTransferFundsBetweenPlayers()
        {
            // Arrange
            var payer = new Player("Payer") { Balance = 1000 };
            var payee = new Player("Payee") { Balance = 500 };
            gameManager.players.Add(payer);
            gameManager.players.Add(payee);

            // Act
            gameManager.PerformTransaction(payer, payee, 200);

            // Assert
            Assert.AreEqual(800, payer.Balance); // Payer's balance reduced
            Assert.AreEqual(700, payee.Balance); // Payee's balance increased
        }

        [Test]
        public void PerformBankTransaction_PlayerReceivesFundsFromBank()
        {
            // Arrange
            var player = new Player("Player 1") { Balance = 500 };
            gameManager.players.Add(player);

            // Act
            gameManager.PerformBankTransaction(player, 200); // Player receives £200 from bank

            // Assert
            Assert.AreEqual(700, player.Balance);
            Assert.AreEqual(49800, gameManager.bank.TotalFunds); // Bank's total funds decrease
        }

        [Test]
        public void PerformBankTransaction_PlayerPaysBank()
        {
            // Arrange
            var player = new Player("Player 1") { Balance = 500 };
            gameManager.players.Add(player);

            // Act
            gameManager.PerformBankTransaction(player, -200); // Player pays £200 to the bank

            // Assert
            Assert.AreEqual(300, player.Balance);
            Assert.AreEqual(50200, gameManager.bank.TotalFunds); // Bank's total funds increase
        }

        [Test]
        public void DrawPotLuckCard_ShouldExecuteCardAction()
        {
            // Arrange
            var player = new Player("Player 1");
            gameManager.players.Add(player);
            var initialBalance = player.Balance;

            // Act
            gameManager.DrawPotLuckCard(player); // Draws a card and executes action

            // Assert
            Assert.AreNotEqual(initialBalance, player.Balance); // Assert balance changes due to card effect
        }

        [Test]
        public void DrawOpportunityKnocksCard_ShouldExecuteCardAction()
        {
            // Arrange
            var player = new Player("Player 1");
            gameManager.players.Add(player);
            var initialBalance = player.Balance;

            // Act
            gameManager.DrawOpportunityKnocksCard(player); // Draws a card and executes action

            // Assert
            Assert.AreNotEqual(initialBalance, player.Balance); // Assert balance changes due to card effect
        }

        [Test]
        public void DisplayBalances_ShouldListAllBalances()
        {
            // Arrange
            gameManager.AddPlayer("Player 1");
            gameManager.AddPlayer("Player 2");

            // Act and Assert (uses UnityEngine.Debug.Log in GameManager)
            Assert.DoesNotThrow(() => gameManager.DisplayBalances()); // Should run without exceptions
        }
    }
}
