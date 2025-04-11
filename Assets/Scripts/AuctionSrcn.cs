using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PropertyTycoon
{
    public class AuctionScrn : MonoBehaviour
    {
        // Singleton instance for global access
        public static AuctionScrn Instance { get; private set; }

        public Text PropertyNameText;  // Text to display the property name
        public Text HighestBidText;    // Text to display the highest bid
        public Text PlayerTurnText;    // Text to display the current player's turn
        public Button BidButton;       // Button to place a bid
        public Button PassButton;      // Button to pass the turn
        public InputField BidAmountInput; // Input field for entering a bid amount

        private Property propertyBeingAuctioned;  // The property being auctioned
        private List<Player> players;            // List of players participating in the auction
        private int currentBid;                  // The current highest bid
        private int currentPlayerIndex;          // Index of the current player in the auction
        private Player highestBidder;            // Player who placed the highest bid

        private void Start()
        {
            // Ensure the Auction Screen is hidden at the start
            gameObject.SetActive(false);
        }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Ensures persistence
            }
            else
            {
                Debug.LogWarning("Duplicate AuctionScrn detected. Destroying this instance.");
                Destroy(gameObject); // Destroy duplicate
            }
        }

        public void StartAuction(Property property, List<Player> playerList)
        {
            Debug.Log($"Starting auction for {property.name} with {playerList.Count} players.");
            propertyBeingAuctioned = property;
            players = new List<Player>(playerList);
            currentBid = 0;
            currentPlayerIndex = 0;
            highestBidder = null;

            UpdateUI();

            if (!gameObject.activeSelf)
            {
                Debug.Log("Manually activating AuctionUI.");
                gameObject.SetActive(true);
            }
        }


        private void UpdateUI()
        {
            // Update auction UI elements
            PropertyNameText.text = $"Auctioning: {propertyBeingAuctioned.name}";
            HighestBidText.text = $"Highest Bid: £{currentBid} by {(highestBidder != null ? highestBidder.Name : "None")}";
            PlayerTurnText.text = $"Current Turn: {players[currentPlayerIndex].Name}";
        }

        public void PlaceBid()
        {
         Player currentPlayer = players[currentPlayerIndex];
         if (currentPlayer.isAI==false){   
            // Get the player's inputted bid amount
            if (int.TryParse(BidAmountInput.text, out int bidAmount))
            {

                if (bidAmount > currentBid && bidAmount <= currentPlayer.Balance)
                {
                    currentBid = bidAmount;
                    highestBidder = currentPlayer;

                    Debug.Log($"{currentPlayer.Name} placed a bid of £{currentBid}");
                    
                    Turn_Script.Instance.CheckBankruptcy(currentPlayer); // Checks for Bankruptcy
                    
                    NextTurn(); // Move to the next player's turn
                }
                else
                {
                    Debug.Log("Invalid bid. Either the bid is too low or the player doesn't have enough balance.");
                }
            }
            else
            {
                Debug.Log("Invalid bid amount entered.");
            }
         }
         else{
            PassTurn();
         }
        }

        public void PassTurn()
        {
            Debug.Log($"{players[currentPlayerIndex].Name} passed their turn.");
            players.RemoveAt(currentPlayerIndex); // Remove the player who passed their turn

            if (players.Count == 1) // If only one player is left, they win the auction
            {
                EndAuction();
                return;
            }

            currentPlayerIndex %= players.Count; // Loop back to the first player if needed
            UpdateUI();
        }

        private void NextTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            UpdateUI();
        }

        private void EndAuction()
        {
            if (highestBidder != null)
            {
                // Deduct the bid amount from the highest bidder
                highestBidder.Debit(currentBid);
                
                Turn_Script.Instance.CheckBankruptcy(highestBidder); // Checks for Bankruptcy

                // Transfer ownership of the property
                propertyBeingAuctioned.SwitchOwner(highestBidder);

                Debug.Log($"{highestBidder.Name} won the auction for {propertyBeingAuctioned.name} at £{currentBid}.");
            }
            else
            {
                Debug.Log("No bids were placed. The property remains unsold.");
            }

            // Hide the auction screen
            gameObject.SetActive(false);
            Turn_Script.purchaseDone=true;
        }

    }
}
