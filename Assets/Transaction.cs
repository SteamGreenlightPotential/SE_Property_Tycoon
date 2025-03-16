using System;

namespace PropertyTycoon
{
    public class Transaction
    {

        public string From { get; private set; }
        public string To { get; private set; }
        public int Amount { get; private set; }

        public Transaction(string from, string to, int amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }

        // Transaction details
        public string GetTransactionDetails()
        {
            return $"{From} paid £{Amount} to {To}";
        }

    }
}