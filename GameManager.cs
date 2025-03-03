using UnityEngine;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class GameManager : MonoBehaviour
    {
        public List<Property> properties; // Lists all properties
        public Bank bank; // Bank instance
        public List<Player> players; // List of players

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);       // Makes GameManager persistent between scenes
            bank = new Bank();                   // Initialize the bank
            players = new List<Player>();        // Initialize the player list
            InitialiseProperties();              // Load all properties
            Debug.Log("Game Manager Online");    // Debug message to confirm initialization
        }

        private void Start()
        {
            Debug.Log("Game Started");  // Debug message for confirmation
            DisplayBalances();          // Show initial balances in the debug console
        }

        // Method to initialise properties
        public void InitialiseProperties()
        {
            PropertyList propertyList = new PropertyList();
            propertyList.initialiseProperties(); // Loads properties form Property.cs
            properties = propertyList.properties; // Assign to the GameManager's list
        }

        // Method to add a player to the game
        public void AddPlayer(string name)
        {
            players.Add(new Player(name)); // Adds a new player to the game
            Debug.Log($"Player {name} has been added."); // Debug message for confirmation
        }

        // Player to Player transaction
        public void PerformTransaction(Player payer, Player payee, int amount)
        {
            payer.Debit(amount); // Deducts amount from payer
            payee.Credit(amount); // Adds amount to payee
            bank.RecordTransaction(new Transaction(payer.Name, payee.Name, amount)); // Records the transaction in the bank
            Debug.Log($"{payer.Name} paid {payee.Name} £{amount}"); // Debug message for confirmation
        }

        // Bank to Player transaction
        public void PerformBankTransaction(Player player, int amount)
        {
            if (amount > 0)
            {
                bank.MakePayment(player, amount); // Bank pays the player
                Debug.Log($"Bank paid {player.Name} £{amount}");
            }
            else
            {
                player.Debit(-amount); // Player pays the bank
                bank.ReceivePayment(-amount);
                Debug.Log($"{player.Name} paid Bank £{-amount}");
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

        // Method to draw and exicute Pot Luck card
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
