using UnityEngine;
using UnityEngine.UI;
using PropertyTycoon;

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
            if (property == null || player == null)
            {
                Debug.LogError("Show() received a null Property or Player!");
                return;
            }

            Debug.Log($"Show() called with Property: {property.name}, Player: {player.Name}");
            CurrentProperty = property;
            CurrentPlayer = player;

            // Update the UI
            if (PropertyName != null)
                PropertyName.text = "Property: " + CurrentProperty.name;
            if (PropertyPrice != null)
                PropertyPrice.text = "Price: £" + CurrentProperty.price.ToString();
            if (PlayerBalance != null)
                PlayerBalance.text = "Balance: £" + CurrentPlayer.Balance.ToString();

            gameObject.SetActive(true); // Make the UI visible
        }




        // When "Buy" button is clicked
        public void OnBuyButtonClicked()
        {
            if (CurrentPlayer == null || CurrentProperty == null)
            {
                Debug.LogError("CurrentPlayer or CurrentProperty is null!");
                return;
            }

            if (CurrentPlayer.Balance >= CurrentProperty.price)
            {
                CurrentPlayer.Debit(CurrentProperty.price); // Deduct the price
                CurrentPlayer.AddProperty(CurrentProperty); // Add the property to the player
                CurrentProperty.switchOwner(CurrentPlayer); // Update the property's owner

                Debug.Log($"{CurrentPlayer.Name} purchased {CurrentProperty.name} for £{CurrentProperty.price}.");
                if (PlayerBalance != null)
                    PlayerBalance.text = "Balance: £" + CurrentPlayer.Balance.ToString();
                else
                    Debug.LogError("PlayerBalance Text component is not assigned.");

                Close(); // Close the purchase screen
            }
            else
            {
                Debug.Log("Not enough funds to purchase this property.");
            }
        }

        // Called when the "Auction" button is clicked
        /*public void OnAuctionButtonClicked()
        {
            if (AuctionUI != null && GameManager.Instance != null)
            {
                AuctionUI.gameObject.SetActive(true);

                if (GameManager.Instance.players != null)
                {
                    AuctionUI.StartAuction(CurrentProperty, GameManager.Instance.players);
                    Debug.Log($"Auction started for {CurrentProperty.name}.");
                }
                else
                {
                    Debug.LogError("GameManager players list is null!");
                }
            }
            else
            {
                Debug.LogError("AuctionUI or GameManager.Instance is null!");
            }

            Close();
        }*/



        // Hide the property purchase screen
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
