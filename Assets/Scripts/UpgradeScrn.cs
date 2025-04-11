using UnityEngine;
using UnityEngine.UI;

namespace PropertyTycoon
{
    public class UpgradeScrn : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject OwnedPropertyPanel;
        public MortgageScreen mortgageScreen;
        public Text PropertyMessage;
        public Button UpgradeHouseButton;
        public Button UpgradeHotelButton;
        public Button MortgageButton; 
        public Button CloseButton;

        private Property currentProperty;
        private Player currentPlayer;
         public boardPlayer testPlayer; // For testing purposes, remove in production
        private UpgradeManager upgradeManager; // Declare upgradeManager

        [System.Obsolete]
        private void Start()
        {

            // Initialize upgradeManager
            upgradeManager = FindObjectOfType<UpgradeManager>();
            if (upgradeManager == null)
            {
                //Debug.Log("UpgradeManager is missing! Please add it to the scene.");
            }

            MortgageScreen mortgageScreen = FindObjectOfType<MortgageScreen>();
            if (mortgageScreen == null)
            {
                //Debug.Log("MortgageScreen is missing! Please add it to the scene.");
            }

            // Setup Close button functionality
            if (CloseButton != null)
            {
                CloseButton.onClick.AddListener(ClosePanel);
            }
            // Ensure the panel is initially hidden
            this.gameObject.SetActive(false);

        }

        public void ShowOwnedPropertyPanel(Property property, Player player)
        {
            if (property == null || player == null)
            {
                Debug.LogError("Invalid property or player passed to ShowOwnedPropertyPanel!");
                return;
            }

            currentProperty = property;
            currentPlayer = player;

            // Update UI elements
            OwnedPropertyPanel.SetActive(true);
            PropertyMessage.text = $"Welcome Back to {property.name}!";

            // Setup button listeners
            UpgradeHouseButton.onClick.RemoveAllListeners();
            UpgradeHouseButton.onClick.AddListener(OnUpgradeHouse);

            UpgradeHotelButton.onClick.RemoveAllListeners();
            UpgradeHotelButton.onClick.AddListener(OnUpgradeHotel);

            if (MortgageButton != null)
            {
                MortgageButton.onClick.RemoveAllListeners();
                MortgageButton.onClick.AddListener(OnMortgage);
            }
        }

        public void OnUpgradeHouse()
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
        public void OnUpgradeHotel()
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

        public void OnMortgage()
        {
            //TEST REASONS

            
            mortgageScreen.mortgageCall(testPlayer);
            ClosePanel();

        }

        public void ClosePanel()
        {
            OwnedPropertyPanel.SetActive(false);
        }
    }
}