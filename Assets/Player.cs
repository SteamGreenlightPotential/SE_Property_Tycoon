using System;

namespace PropertyTycoon
{
    public class Player 
    {
        public string Name { get; set; }
        public int Balance { get; set; }

        public Player(string name)
        {
            // Set Player's Name and Handle null exception
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Balance = 1500; // Player starts with £1500
        }

        // Adding money to the Player's Account
        public void Credit(int amount)
        {
            Balance += amount;
        }

        // Removing money from the Player's Account
        public void Debit(int amount)
        {
            Balance -= amount;
        }
    }
}
