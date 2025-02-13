using System;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class Bank
    {
        public int TotalFunds { get; private set; }
        public List<Transaction> TransactionLog { get; private set; }

        public Bank()
        {
            TotalFunds = 50000; // Bank starts with £50,000
            TransactionLog = new List<Transaction>(); //Transaction Log (Could be used later)
        }

        // Bank receiving payment
        public void ReceivePayment(int amount)
        {
            TotalFunds += amount;
        }

        // Bank Paying a player
        public void MakePayment (Player player, int amount)
        {
            TotalFunds -= amount;
            player.Credit(amount);
        }

        // Method to record a transaction
        public void RecordTransaction(Transaction transaction)
        {
            TransactionLog.Add(transaction);
        }
    }
}



