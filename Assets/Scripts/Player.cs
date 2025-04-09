using System;
using System.Collections.Generic;
using System.Linq;

namespace PropertyTycoon
{
    public class Player
    {
        private string v; // added for test class
        public string Name { get; set; }
        public boardPlayer bPlayer; // Reference to Player on Board

        public int HousesOwned { get; set; } = 0; // Default to 0 houses
        public int HotelsOwned { get; set; } = 0; // Default to 0 hotels
        public bool HasGetOutOfJailCard { get; set; } = false; // Default to false
        public bool IsInJail { get; set; } = false; // Default to false ( found a same property in Player_Movement.cs... but it works so...)

     public int Balance
    {
        get => bPlayer.balance;
        set => bPlayer.balance = value;
    }

   public List<Property> OwnedProperties{
    get => bPlayer.OwnedProperties;
    set => bPlayer.OwnedProperties= value;
   } 

        public Player(string name, boardPlayer boardPlayer)
        {
            bPlayer = boardPlayer ?? throw new ArgumentNullException(nameof(boardPlayer));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Balance = 1500; // Player starts with £1500
        }

        public Player(string v)
        {
            this.v = v;
        }

        public void Credit(int amount)
        {
            Balance += amount; // Adds funds to the player
        }

        public void Debit(int amount)
        {
            Balance -= amount; // Deducts funds from the player
        }

        public void AddProperty(Property property)
        {
            OwnedProperties.Add(property); // Adds a property to the player's owned properties
        }

        public bool OwnsProperty(string propertyName)
        {
            return OwnedProperties.Exists(p => p.name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)); // Checks if the player owns a property
        }
        
        public bool OwnsAllPropertiesInColorGroup(string color)
        {
            if (GameManager.Instance != null)
            {
                var colorProperties = GameManager.Instance.properties.FindAll(p => p.colour == color);
                return colorProperties.TrueForAll(p => p.owner == this);
            }
            else
            {
                //Debug.LogError("GameManager instance is null!");
                return false;
            }
        }

        public bool CanAddHouseToSet(Property property)
        {
            var colorSet = GameManager.Instance.properties.FindAll(p => p.colour == property.colour && p.owner == this);

            // Get min and max houses in the color set
            int maxHouses = colorSet.Max(p => p.houses);
            int minHouses = colorSet.Min(p => p.houses);

            // Ensure no property in the set has more than one house difference
            return maxHouses - minHouses <= 1 && property.houses == minHouses;
        }

        public bool CanAddHotelToSet(Property property)
        {
            var colorSet = GameManager.Instance.properties.FindAll(p => p.colour == property.colour && p.owner == this);

            // Check if every property in the color set has 4 houses
            return colorSet.All(p => p.houses == 4);
        }

    }
}
