using UnityEngine;
using UnityEngine.UI;
using PropertyTycoon;

namespace PropertyTycoon
{
    public class PropertyPurchaseScrn : MonoBehaviour
    {
        [Header("UI Text Components")]
        public Text PropertyName;       // Displays the property name
        public Text PropertyPrice;      // Displays the property price
        public Text PropertyColorText;  // Displays the property color
        public Text PlayerBalance;      // Displays the player's balance

        [Header("Buttons")]
        public Button BuyButton;        // Button to purchase the property
        public Button AuctionButton;    // Button to start an auction

        [Header("Auction UI Reference")]
        public AuctionScrn AuctionUI;   // Reference to the Auction UI

        private Property CurrentProperty; // Property currently being displayed
        private Player CurrentPlayer;     // Player currently viewing the screen

        /// <summary>
        /// Displays the property purchase screen with the provided property and player information.
        /// </summary>
        /// <param name="property">The property to display.</param>
        /// <param name="player">The player attempting to purchase the property.</param>
        public void Show(Property property, Player player)
        {
            if (property == null || player == null)
            {
                Debug.LogError("Show() received a null Property or Player!");
                return;
            }

            Debug.Log($"Show() called with Property: {property.name}, Player: {player.Name}");

            // Set the current property and player
            CurrentProperty = property;
            CurrentPlayer = player;

            // Update UI elements
            UpdateUIElements();

            // Make the property purchase screen visible
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Updates all UI elements on the property purchase screen.
        /// </summary>
        private void UpdateUIElements()
        {
            // Update the Property Name
            if (PropertyName != null)
            {
                PropertyName.text = "Property: " + CurrentProperty.name;
                Debug.Log($"PropertyName updated to: {PropertyName.text}");
            }
            else
            {
                Debug.LogError("PropertyName Text UI is not assigned in the Inspector!");
            }

            // Update the Property Price
            if (PropertyPrice != null)
            {
                PropertyPrice.text = "Price: £" + CurrentProperty.price.ToString();
                Debug.Log($"PropertyPrice updated to: {PropertyPrice.text}");
            }
            else
            {
                Debug.LogError("PropertyPrice Text UI is not assigned in the Inspector!");
            }

            // Update the Property Color
            if (PropertyColorText != null)
            {
                PropertyColorText.text = "Color: " + CurrentProperty.colour;
                Debug.Log($"PropertyColorText updated to: {PropertyColorText.text}");
            }
            else
            {
                Debug.LogError("PropertyColorText Text UI is not assigned in the Inspector!");
            }

            // Update the Player Balance
            if (PlayerBalance != null)
            {
                PlayerBalance.text = "Balance: £" + CurrentPlayer.Balance.ToString();
                Debug.Log($"PlayerBalance updated to: {PlayerBalance.text}");
            }
            else
            {
                Debug.LogError("PlayerBalance Text UI is not assigned in the Inspector!");
            }
        }

        /// <summary>
        /// Called when the "Buy" button is clicked.
        /// </summary>
        public void OnBuyButtonClicked()
        {
            if (CurrentPlayer == null || CurrentProperty == null)
            {
                Debug.LogError("CurrentPlayer or CurrentProperty is null!");
                return;
            }

            if (CurrentPlayer.Balance >= CurrentProperty.price)
            {
                // Complete the purchase
                CurrentPlayer.Debit(CurrentProperty.price); // Deduct the price from player's balance
                CurrentPlayer.AddProperty(CurrentProperty); // Add the property to the player's portfolio
                CurrentProperty.switchOwner(CurrentPlayer); // Update the owner of the property

                Debug.Log($"{CurrentPlayer.Name} purchased {CurrentProperty.name} for £{CurrentProperty.price}.");

                // Update the player's balance on the UI
                if (PlayerBalance != null)
                {
                    PlayerBalance.text = "Balance: £" + CurrentPlayer.Balance.ToString();
                    Debug.Log($"PlayerBalance updated to: {PlayerBalance.text}");
                }
                else
                {
                    Debug.LogError("PlayerBalance Text UI is not assigned in the Inspector!");
                }

                Close(); // Close the purchase screen
            }
            else
            {
                Debug.Log("Not enough funds to purchase this property.");
            }
        }

        /// <summary>
        /// Hides the property purchase screen.
        /// </summary>
        private void Close()
        {
            gameObject.SetActive(false);
            Debug.Log("PropertyPurchaseScrn closed.");
        }

        /// <summary>
        /// Called when the "Auction" button is clicked (optional implementation for auctions).
        /// </summary>
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
    }
}
