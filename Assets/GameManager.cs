using UnityEngine;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class GameManager : MonoBehaviour
    {
        public List<Property> properties;       // Lists all properties
        public Bank bank;                       // Bank instance
        public List<Player> players;            // List of players
        public PropertyManager propertyManager; // Manages property initialization

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);           // Makes GameManager persistent between scenes
            bank = new Bank();                       // Initialize the bank
            players = new List<Player>();            // Initialize the players list
            propertyManager = new PropertyManager(); // Initialize PropertyManager
            InitialiseProperties();                  // Load all properties
            Debug.Log("Game Manager Online");        // Debug message for confirmation
        }

        private void Start()
        {
            Debug.Log("Game Started");  // Debug message for confirmation
            DisplayBalances();          // Show initial balances in the debug console
        }

        // Method to initialise properties
        public void InitialiseProperties()
        {
            propertyManager.initialiseProperties();  // Load properties from PropertyManager
            properties = propertyManager.properties; // Assign properties list
        }

        // Method to add a player to the game
        public void AddPlayer(string name, boardPlayer playerBoard)
        {
            players.Add(new Player(name, playerBoard));
            Debug.Log($"Player {name} has been added to the game.");
        }


        // Player to Player transaction
        public void PerformTransaction(Player payer, Player payee, int amount)
        {
            if (payer.Balance < amount)
            {
                Debug.Log($"{payer.Name} does not have enough balance to pay {payee.Name} £{amount}.");
                return;
            }

            payer.Debit(amount);        // Deducts amount from the payer
            payee.Credit(amount);       // Adds amount to the payee
            bank.RecordTransaction(new Transaction(payer.Name, payee.Name, amount)); // Records the transaction in the bank
            Debug.Log($"{payer.Name} paid {payee.Name} £{amount}."); // Debug message for confirmation
        }

        // Bank to Player transaction
        public void PerformBankTransaction(Player player, int amount)
        {
            if (amount > 0)
            {
                bank.MakePayment(player, amount); // Bank pays the player
                Debug.Log($"Bank paid {player.Name} £{amount}.");
            }
            else
            {
                player.Debit(-amount); // Player pays the bank
                bank.ReceivePayment(-amount);
                Debug.Log($"{player.Name} paid Bank £{-amount}.");
            }
        }

        // Method to display all balances
        public void DisplayBalances()
        {
            foreach (var player in players)
            {
                Debug.Log($"Player {player.Name}: £{player.Balance}");
            }
            Debug.Log($"Bank Funds: £{bank.TotalFunds}");
            Debug.Log($"Free Parking: £{bank.FreeParking}");
        }

        // Method to draw and execute Pot Luck card
        public void DrawPotLuckCard(Player player)
        {
            Card potLuckCard = Cards.DrawTopCard(Cards.PotLuck);
            Cards.ExecuteCardAction(potLuckCard, player, bank, players); // Executes card action
        }

        // Method to draw and execute Opportunity Knocks card
        public void DrawOpportunityKnocksCard(Player player)
        {
            Card opportunityKnocksCard = Cards.DrawTopCard(Cards.OpportunityKnocks);
            Cards.ExecuteCardAction(opportunityKnocksCard, player, bank, players); // Executes card action
        }
    }
}
