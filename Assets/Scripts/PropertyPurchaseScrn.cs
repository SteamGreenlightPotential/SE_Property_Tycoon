using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class PropertyPurchaseScrn : MonoBehaviour
    {
        [Header("UI Text Components")]
        public Text PropertyName;       
        public Text PropertyPrice;      
        public Text PropertyColorText;  
        public Text PlayerBalance;      

        [Header("Buttons")]
        public Button BuyButton;        
        public Button AuctionButton;    

        [Header("Auction UI Reference")]
        public AuctionScrn AuctionUI;   

        private Property CurrentProperty;
        private Player CurrentPlayer;

        private void Start()
        {

            // Ensure the Property Purchase Panel is hidden at the start
            gameObject.SetActive(false);

            if (AuctionUI != null)
            {
                Debug.Log("AuctionUI is correctly assigned in the Inspector.");
            }
            else
            {
                Debug.LogError("AuctionUI is not assigned! Check the Inspector.");
            }
        }

        public void Show(Property property, Player player)
        {
            if (property == null || player == null)
            {
                Debug.LogError("Show() received a null Property or Player!");
                return;
            }

            CurrentProperty = property;
            CurrentPlayer = player;

            UpdateUIElements();
            gameObject.SetActive(true);

        }

        private void UpdateUIElements()
        {
            if (PropertyName != null)
                PropertyName.text = "Property: " + CurrentProperty.name;

            if (PropertyPrice != null)
                PropertyPrice.text = "Price: £" + CurrentProperty.price.ToString();

            if (PropertyColorText != null)
                PropertyColorText.text = "Color: " + CurrentProperty.colour;

            if (PlayerBalance != null)
                PlayerBalance.text = "Balance: £" + CurrentPlayer.Balance.ToString();
        }

        public void OnBuyButtonClicked()
        {
            if (CurrentPlayer.Balance >= CurrentProperty.price)
            {
                CurrentPlayer.Debit(CurrentProperty.price);
                CurrentPlayer.AddProperty(CurrentProperty);
                CurrentProperty.SwitchOwner(CurrentPlayer);

                Debug.Log($"{CurrentPlayer.Name} purchased {CurrentProperty.name} for £{CurrentProperty.price}.");
                
                Close(); // Hide the purchase screen
            }
            else
            {
                Debug.Log("Not enough funds to purchase this property.");
            }
        }

        /// <summary>
        /// Called when the "Auction" button is clicked.
        /// </summary>
        public void OnAuctionButtonClicked()
        {
            if (AuctionUI == null)
                Debug.LogError("AuctionUI is null!");
            if (CurrentProperty == null)
                Debug.LogError("CurrentProperty is null!");
            if (Turn_Script.Instance == null)
                Debug.LogError("Turn_Script.Instance is null!");

            if (AuctionUI != null && CurrentProperty != null && Turn_Script.Instance != null)
            {
                List<Player> playerList = Turn_Script.Instance.playerlist; // Retrieve the player list

                // Activate the Auction UI and start the auction
                AuctionUI.gameObject.SetActive(true);
                AuctionUI.StartAuction(CurrentProperty, playerList);
                Debug.Log($"Auction started for {CurrentProperty.name} with {playerList.Count} players.");
                Close();
            }
        }

        
        
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
