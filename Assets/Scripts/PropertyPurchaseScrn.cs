using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro namespace

namespace PropertyTycoon
{
    public class PropertyPurchaseScrn : MonoBehaviour
    {
        public TextMeshProUGUI PropertyName;        // Displays the property name
        public TextMeshProUGUI PropertyPrice;       // Displays the property price
        public TextMeshProUGUI PropertyColor;       // Displays the property color
        public TextMeshProUGUI PlayerBalance;       // Displays the player's balance
        public Button BuyButton;                        // Button to purchase the property
        public Button CancelButton;                     // Button to cancel the purchase

        // Mortgage button to be added later //

        private Property CurrentProperty;            // Property currently being displayed
        private Player CurrentPlayer;                // Player currently viewing the screen

        // Show the property purchase screen
        public void Show(Property property, Player player)
        {
            CurrentProperty = property;
            CurrentPlayer = player;

            // Updateing the UI with property and player details
            PropertyName.text = "Property: " + CurrentProperty.name;
            PropertyColor.text = "Color: " + CurrentProperty.colour;
            PropertyPrice.text = "Price: £" + CurrentProperty.price.ToString();
            PlayerBalance.text = "Balance: £" + CurrentPlayer.Balance.ToString();

            gameObject.SetActive(true); // Makeing the UI visible
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

        // Called when the "Cancel" button is clicked
        public void OnCancelButtonClicked()
        {
            Close(); // Close the purchase screen
        }

        // Hide the property purchase screen
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
