using UnityEngine;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }  // GameManager instance

        public List<Property> properties;       // Lists all properties
        public Bank bank;                       // Bank instance
        public List<Player> players;            // List of players
        public PropertyManager propertyManager; // Manages property initialization

        private void Awake()
        {
            // Singleton implementation
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Makes GameManager persistent between scenes
                //Debug.Log("GameManager instance created successfully.");
            }
            else
            {
                //Debug.LogError("Multiple GameManager instances detected!");
                Destroy(gameObject); // Ensures there's only one GameManager instance
            }

            if (propertyManager != null)
            {
                //propertyManager.initialiseProperties();  // Load properties from PropertyManager
                properties = propertyManager.properties; // Assign properties list
            }
            else
            {
                //Debug.LogError("PropertyManager is not assigned in GameManager!");
                propertyManager = new PropertyManager(); // Initialize PropertyManager

            }

            // Initialize core game components
            bank = new Bank();                       // Initialize the bank
            players = new List<Player>();            // Initialize the players list
            propertyManager = new PropertyManager(); // Initialize PropertyManager
            //Debug.Log("GameManager Online");         // Debug message for confirmation
        }

        private void Start()
        {
            //Debug.Log("Game Started");  // Debug message for confirmation
            DisplayBalances();          // Show initial balances in the debug console
        }

        // Method to initialize properties
        public void InitialiseProperties()
        {
            propertyManager.initialiseProperties();  // Load properties from PropertyManager
            properties = propertyManager.properties; // Assign properties list
        }

        // Method to add a player to the game
        public void AddPlayer(string name, boardPlayer playerBoard)
        {
            players.Add(new Player(name, playerBoard));
            //Debug.Log($"Player {name} has been added to the game.");
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

            Debug.Log($"{payer.Name} paid {payee.Name} £{amount}."); // Debug message for confirmation

            Turn_Script.Instance.CheckBankruptcy(payer); // Checks for bankruptcy
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
                Turn_Script.Instance.CheckBankruptcy(player); // Checks for bankruptcy
            }
        }

        // Method to display all balances
        public void DisplayBalances()
        {
            foreach (var player in players)
            {
                Debug.Log($"Player {player.Name}: £{player.Balance}");
            }
            //Debug.Log($"Bank Funds: £{bank.TotalFunds}");
            //Debug.Log($"Free Parking: £{bank.FreeParking}");
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

        // Start of Auction
        public void StartAuction(Property property)
        {
            if (property != null && AuctionScrn.Instance != null) // Check if AuctionScrn is properly initialized
            {
                AuctionScrn.Instance.StartAuction(property, players); // Start the auction
                Debug.Log($"Auction started for {property.name}");
            }
            else
            {
                Debug.LogError("Failed to start auction. Either the property or AuctionScrn is null.");
            }
        }
    }
}
