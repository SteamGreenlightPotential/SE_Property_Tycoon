/*
using UnityEngine;
using UnityEngine.UI;

namespace PropertyTycoon
{
    public class UpgradeScrn : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject OwnedPropertyPanel; // Reference to the panel
        public Text PropertyMessage; // Text to display property details
        public Button UpgradeHouseButton; // Button to upgrade to a house
        public Button UpgradeHotelButton; // Button to upgrade to a hotel
        public Button MortgageButton; // Button to mortgage the property (not implemented yet)
        public Button CloseButton; // Button to close the panel

        private Property currentProperty; // Property currently being interacted with
        private Player currentPlayer; // Player who owns the property
        //private UpgradeManager upgradeManager; // Reference to UpgradeManager

        [System.Obsolete]
        private void Start()
        {
            // Ensure the panel is initially hidden
            OwnedPropertyPanel.SetActive(false);

            // Initialize the UpgradeManager reference
            upgradeManager = FindObjectOfType<UpgradeManager>();
            if (upgradeManager == null)
            {
                Debug.LogError("UpgradeManager is missing! Please assign it in the scene.");
            }

            // Setup Close button functionality
            if (CloseButton != null)
            {
                CloseButton.onClick.AddListener(ClosePanel);
            }
        }

        public void ShowOwnedPropertyPanel(Property property, Player player)
        {
            if (property == null || player == null)
            {
                Debug.LogError("Invalid property or player passed to ShowOwnedPropertyPanel!");
                return;
            }

            // Assign current property and player
            currentProperty = property;
            currentPlayer = player;

            // Update UI elements
            OwnedPropertyPanel.SetActive(true);
            PropertyMessage.text = $"Welcome to {property.name}! You own this property.";

            // Setup button listeners
            UpgradeHouseButton.onClick.RemoveAllListeners();
            UpgradeHouseButton.onClick.AddListener(OnUpgradeHouse);

            UpgradeHotelButton.onClick.RemoveAllListeners();
            UpgradeHotelButton.onClick.AddListener(OnUpgradeHotel);
        }

        
        private void OnUpgradeHouse()
        {
            if (upgradeManager != null)
            {
                upgradeManager.TryAddHouse(currentProperty, currentPlayer);
            }
            else
            {
                Debug.LogError("UpgradeManager is not assigned.");
            }
            ClosePanel();
        }

        private void OnUpgradeHotel()
        {
            if (upgradeManager != null)
            {
                upgradeManager.TryAddHotel(currentProperty, currentPlayer);
            }
            else
            {
                Debug.LogError("UpgradeManager is not assigned.");
            }
            ClosePanel();
        }

        private void onMortGage

        private void ClosePanel()
        {
            OwnedPropertyPanel.SetActive(false);
        }
    }
}
*/