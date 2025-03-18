using UnityEngine;
using UnityEngine.UI;

namespace PropertyTycoon
{
    public class PropertyPurchaseScrn : MonoBehaviour
    {
        public Text PropertyName;        // Displays the property name
        public Text PropertyPrice;       // Displays the property price
        public Text PropertyColor;       // Displays the property color
        public Text PlayerBalance;       // Displays the player's balance
        public Button BuyButton;         // Button to purchase the property
        public Button AuctionButton;     // Button to start an auction
        public AuctionScrn AuctionUI;    // Reference to the Auction UI

        private Property CurrentProperty; // Property currently being displayed
        private Player CurrentPlayer;     // Player currently viewing the screen

        // Show the property purchase screen
        public void Show(Property property, Player player)
        {
            CurrentProperty = property;
            CurrentPlayer = player;

            // Update the UI with property and player details
            PropertyName.text = "Property: " + CurrentProperty.name;
            PropertyColor.text = "Color: " + CurrentProperty.colour;
            PropertyPrice.text = "Price: £" + CurrentProperty.price.ToString();
            PlayerBalance.text = "Balance: £" + CurrentPlayer.Balance.ToString();

            gameObject.SetActive(true); // Make the UI visible
        }

        // When "Buy" button is clicked
        public void OnBuyButtonClicked()
        {
            if (CurrentPlayer.Balance >= CurrentProperty.price)
            {
                CurrentPlayer.Debit(CurrentProperty.price); // Deduct the price
                CurrentPlayer.AddProperty(CurrentProperty); // Add the property to the player
                CurrentProperty.switchOwner(CurrentPlayer); // Update the property's owner

                Debug.Log($"{CurrentPlayer.Name} purchased {CurrentProperty.name} for £{CurrentProperty.price}.");
                PlayerBalance.text = "Balance: £" + CurrentPlayer.Balance.ToString();

                Close(); // Close the purchase screen
            }
            else
            {
                Debug.Log("Not enough funds to purchase this property.");
            }
        }

        // Auction button implemented instead of cancel.
        // Called when the "Auction" button is clicked
        /*public void OnAuctionButtonClicked()
        {
            if (AuctionUI != null)
            {
                AuctionUI.gameObject.SetActive(true); // Activate the AuctionScrn GameObject
                AuctionUI.StartAuction(CurrentProperty, GameManager.Instance.players); // Start the auction
                Debug.Log($"Auction started for {CurrentProperty.name}.");
            }
            else
            {
                Debug.LogError("AuctionScrn is not assigned in the PropertyPurchaseScrn script!");
            }

            Close(); // Close the property purchase screen
        }*/

        // Hide the property purchase screen
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
