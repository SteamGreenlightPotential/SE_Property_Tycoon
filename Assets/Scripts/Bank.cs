using System;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class Bank
    {
        public int TotalFunds { get; private set; }
        public List<Transaction> TransactionLog { get; private set; }
        public int FreeParking { get; private set; }

        public Bank()
        {
            TotalFunds = 50000; // Bank starts with £50,000
            TransactionLog = new List<Transaction>(); // Transaction Log (Could be used later)
            FreeParking = 0; // Free Parking initially £0
        }

        public void ReceivePayment(int amount)
        {
            TotalFunds += amount; // Adds funds to the bank
        }

        public void MakePayment(Player player, int amount)
        {
            TotalFunds -= amount; // Reduces bank's funds
            player.Credit(amount); // Credits the player's account
        }

        public void AddToFreeParking(int amount)
        {
            FreeParking += amount; // Adds funds to the Free Parking pool
        }

        public void RecordTransaction(Transaction transaction)
        {
            TransactionLog.Add(transaction); // Logs the transaction
        }
    }
}
