using System.Collections.Generic;
using UnityEngine;

namespace PropertyTycoon.Tests
{
    public class CardsTest : MonoBehaviour
    {
        private Player testPlayer;
        private Bank testBank;
        private List<Player> testPlayers;
        private PropertyManager propertyManager;

        void Start()
        {
            // Initialize PropertyManager
            propertyManager = new GameObject("PropertyManager").AddComponent<PropertyManager>();
            propertyManager.initialiseProperties();

            // Set up test player and bank
            testPlayer = new Player
            {
                bPlayer = new boardPlayer(),
                Name = "Test Player"
            };

            testBank = new Bank();
            testPlayers = new List<Player> { testPlayer };

            RunTests();
        }

        void RunTests()
        {
            TestBankToPlayerTransactions();
            TestPlayerToBankTransactions();
            TestPlayerToPlayerTransactions();
            TestFreeParkingTransactions();
            TestPlayerMovement();
            Debug.Log("All tests completed.");
        }

        void TestBankToPlayerTransactions()
        {
            Debug.Log("Running TestBankToPlayerTransactions...");
            testPlayer.bPlayer.balance = 0;

            Cards.ExecuteCardAction(new Card("Bank pays player £200", "Bank pays player £200"), testPlayer, testBank, testPlayers);
            Debug.Assert(testPlayer.bPlayer.balance == 200, "Bank to Player £200 failed.");

            Cards.ExecuteCardAction(new Card("Bank pays player £50", "Bank pays player £50"), testPlayer, testBank, testPlayers);
            Debug.Assert(testPlayer.bPlayer.balance == 250, "Bank to Player £50 failed.");
        }

        void TestPlayerToBankTransactions()
        {
            Debug.Log("Running TestPlayerToBankTransactions...");
            testPlayer.bPlayer.balance = 200;

            Cards.ExecuteCardAction(new Card("Player pays £50 to the bank", "Player pays £50 to the bank"), testPlayer, testBank, testPlayers);
            Debug.Assert(testPlayer.bPlayer.balance == 150, "Player to Bank £50 failed.");

            Cards.ExecuteCardAction(new Card("Player pays £100 to the bank", "Player pays £100 to the bank"), testPlayer, testBank, testPlayers);
            Debug.Assert(testPlayer.bPlayer.balance == 50, "Player to Bank £100 failed.");
        }

        void TestPlayerToPlayerTransactions()
        {
            Debug.Log("Running TestPlayerToPlayerTransactions...");
            testPlayer.bPlayer.balance = 100;
            Player otherPlayer = new Player { bPlayer = new boardPlayer(), Name = "Other Player" };
            otherPlayer.bPlayer.balance = 100;
            testPlayers.Add(otherPlayer);

            Cards.ExecuteCardAction(new Card("It's your birthday. Collect £10 from each player", "Player receives £10 from each player"), testPlayer, testBank, testPlayers);
            Debug.Assert(testPlayer.bPlayer.balance == 110, "Player to Player transaction failed (Test Player).");
            Debug.Assert(otherPlayer.bPlayer.balance == 90, "Player to Player transaction failed (Other Player).");
        }

        void TestFreeParkingTransactions()
        {
            Debug.Log("Running TestFreeParkingTransactions...");
            testPlayer.bPlayer.balance = 100;

            Cards.ExecuteCardAction(new Card("Pay a £10 fine or take Opportunity Knocks", "If fine paid, player puts £10 on free parking"), testPlayer, testBank, testPlayers);
            Debug.Assert(testPlayer.bPlayer.balance == 90, "Free Parking £10 failed.");
        }

        void TestPlayerMovement()
        {
            Debug.Log("Running TestPlayerMovement...");
            testPlayer.bPlayer.TileCount = 0;

            // Test movement to GO
            Cards.ExecuteCardAction(new Card("Advance to GO", "Player moves forwards to GO"), testPlayer, testBank, testPlayers);
            Debug.Assert(testPlayer.bPlayer.TileCount == 0, "Movement to GO failed.");

            // Test movement to Turing Heights
            Cards.ExecuteCardAction(new Card("Advance to Turing Heights", "Player token moves forwards to Turing Heights"), testPlayer, testBank, testPlayers);
            Debug.Assert(testPlayer.bPlayer.TileCount == 40, "Movement to Turing Heights failed.");

            // Test movement to Han Xin Gardens
            Cards.ExecuteCardAction(new Card("Advance to Han Xin Gardens", "Player moves token to Han Xin Gardens"), testPlayer, testBank, testPlayers);
            Debug.Assert(testPlayer.bPlayer.TileCount == 25, "Movement to Han Xin Gardens failed.");

            // Test backward movement
            testPlayer.bPlayer.TileCount = 5; // Reset position
            Cards.ExecuteCardAction(new Card("Go back 3 spaces", "Player moves token backwards 3 spaces"), testPlayer, testBank, testPlayers);
            Debug.Assert(testPlayer.bPlayer.TileCount == 2, "Backward movement failed.");
        }
    }
}
