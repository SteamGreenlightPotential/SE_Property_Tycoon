using System;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class Player
    {
        private string v; // added for test class

        public string Name { get; set; }
        public int Balance { get; set; }
        public List<Property> OwnedProperties { get; private set; }
        public boardPlayer bPlayer { get; private set; } // Reference to Player on Board


        public Player(string name, boardPlayer boardPlayer)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Balance = 1500; // Player starts with £1500
            OwnedProperties = new List<Property>();
            bPlayer = boardPlayer ?? throw new ArgumentNullException(nameof(boardPlayer));
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
        
        /*public bool OwnsAllPropertiesInColorGroup(string color)
        {
            var colorProperties = GameManager.Instance.properties.FindAll(p => p.colour == color);
            return colorProperties.TrueForAll(p => p.owner == this);
        }*/
    }
}
