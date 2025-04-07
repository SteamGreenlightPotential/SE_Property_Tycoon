/*
Test for cards.cs
Anik Kazi
*/
using NUnit.Framework;
using System.Collections.Generic;

namespace PropertyTycoon.Tests
{
    [TestFixture]
    public class CardsTests
    {
        private Bank bank;
        private Player player;
        private List<Player> players;

        [SetUp]
        public void Setup()
        {
            bank = new Bank();
            player = new Player("Test Player");
            players = new List<Player>
            {
                player,
                new Player("Player 2"),
                new Player("Player 3")
            };
        }

        [Test]
        public void DrawTopCard()
        {
            // Arrange
            var cards = new List<Card>
            {
                new Card("Test Card 1", "Test Action 1"),
                new Card("Test Card 2", "Test Action 2")
            };

            // Act
            var topCard = Cards.DrawTopCard(cards);

            // Assert
            Assert.AreEqual("Test Card 1", topCard.Description);
            Assert.AreEqual(2, cards.Count);
            Assert.AreEqual("Test Card 2", cards[0].Description);
        }

        [Test]
        public void ExecuteCardAction_BankPaysPlayer()
        {
            // Arrange
            var card = new Card("Bank pays player £50", "Bank pays player £50");
            int initialBalance = player.Balance;

            // Act
            Cards.ExecuteCardAction(card, player, bank, players);

            // Assert
            Assert.AreEqual(initialBalance + 50, player.Balance);
        }

        [Test]
        public void ExecuteCardAction_PlayerPaysBank()
        {
            // Arrange
            var card = new Card("Player pays £50 to the bank", "Player pays £50 to the bank");
            int initialBalance = player.Balance;
            int initialBankFunds = bank.TotalFunds;

            // Act
            Cards.ExecuteCardAction(card, player, bank, players);

            // Assert
            Assert.AreEqual(initialBalance - 50, player.Balance);
            Assert.AreEqual(initialBankFunds + 50, bank.TotalFunds);
        }

        [Test]
        public void ExecuteCardAction_PlayerPutsMoneyOnFreeParking()
        {
            // Arrange
            var card = new Card("Player puts £15 on free parking", "Player puts £15 on free parking");
            int initialFreeParking = bank.FreeParking;

            // Act
            Cards.ExecuteCardAction(card, player, bank, players);

            // Assert
            Assert.AreEqual(initialFreeParking + 15, bank.FreeParking);
        }

        [Test]
        public void ExecuteCardAction_PlayerReceivesMoneyFromOtherPlayers()
        {
            // Arrange
            var card = new Card("It's your birthday. Collect £10 from each player", "Player receives £10 from each player");
            int initialBalance = player.Balance;

            // Act
            Cards.ExecuteCardAction(card, player, bank, players);

            // Assert
            Assert.AreEqual(initialBalance + 20, player.Balance); // 2 players pay £10 each
        }
    }
}
