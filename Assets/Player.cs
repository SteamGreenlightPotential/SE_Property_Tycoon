using System;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class Player
    {
        public string Name { get; set; }
        public int Balance { get; set; }
        public List<Property> Properties { get; set; }

        public boardPlayer bPlayer; //Associated piece on board


        public Player(string name, boardPlayer bplayer)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name)); // Preventing Null Error
            Balance = 1500; // Player starts with £1500
            OwnedProperties = new List<Property>();
        }

        public void Credit(int amount)
        {
            Balance += amount; // Adds funds to player
        }

        public void Debit(int amount)
        {
            Balance -= amount; // Deducts funds from player
        }

        public void AddProperty(Property property)
        {
            OwnedProperties.Add(property); // Adds property to player's owned properties
        }

        public bool OwnsProperty(string propertyName)
        {
            return OwnedProperties.Exists(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)); // Checks if player owns a property
        }   
    }
}