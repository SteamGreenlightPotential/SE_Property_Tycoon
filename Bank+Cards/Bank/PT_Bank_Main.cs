using System;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class PT_Bank_Main
    {
        static void Main(string[] args)
        {
            // Initializing Bank
            Bank bank = new Bank();

            // List to hold players
            List<Player> players = new List<Player>();

            // Get number of players
            int numPlayers = 0;

            while (numPlayers < 1 || numPlayers > 6)
            {
                Console.Write("Enter the number of players (1-6): ");
                int.TryParse(Console.ReadLine(), out numPlayers);
            }

            // Get players' names
            for (int i = 0; i < numPlayers; i++)
            {
                string? playerName;
                do
                {
                    Console.Write($"Enter name for Player {i + 1}: ");
                    playerName = Console.ReadLine();

                    // Handling Null or Empty Name error
                    if (string.IsNullOrWhiteSpace(playerName))
                    {
                        Console.WriteLine("Name cannot be empty. Please enter a valid name.");
                    }
                }
                while (string.IsNullOrWhiteSpace(playerName));

                players.Add(new Player(playerName)); // Adds player to the list
            }

            // Display balances (could be made into a method.... but not necessary for the overall project)
            Console.WriteLine("\nCurrent Balances:");
            foreach (Player player in players)
            {
                Console.WriteLine($"{player.Name}: £{player.Balance}");
            }
            Console.WriteLine($"Bank: £{bank.TotalFunds}\n");
            Console.WriteLine($"Free Parking: £{bank.FreeParking}\n");

            // Transaction Process
            bool performTransaction = true;

            while (performTransaction)
            {
                Console.Write("\nDo you want to perform a transaction? (Yes/No): ");
                string? response = Console.ReadLine();

                if (response != null && response.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\nTransaction Type:");
                    Console.WriteLine("1. Pay Rent");
                    Console.WriteLine("2. Buy Property");
                    Console.WriteLine("3. Draw Pot Luck Card");
                    Console.WriteLine("4. Draw Opportunity Knocks Card");
                    Console.Write("\nChoose Transaction Type (1-4): ");

                    string? transactionType = Console.ReadLine();

                    switch (transactionType)
                    {
                        case "1": // Pay Rent (Player to Player transaction)
                            Console.Write("Enter payer's name: ");
                            string? payerName = Console.ReadLine();
                            Player? payer = players.Find(p => p.Name.Equals(payerName, StringComparison.OrdinalIgnoreCase));

                            Console.Write("Enter payee's name: ");
                            string? payeeName = Console.ReadLine();
                            Player? payee = players.Find(p => p.Name.Equals(payeeName, StringComparison.OrdinalIgnoreCase));

                            if (payer != null && payee != null)
                            {
                                Console.Write("Enter rent amount: £");
                                if (int.TryParse(Console.ReadLine(), out int rentAmount))
                                {
                                    payer.Debit(rentAmount);
                                    payee.Credit(rentAmount);
                                    Transaction transaction = new Transaction(payer.Name, payee.Name, rentAmount);
                                    bank.RecordTransaction(transaction);
                                    Console.WriteLine(transaction.GetTransactionDetails());
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount entered.\n");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid payer or payee name. Transaction failed.");
                            }
                            break;

                        case "2": // Buying Property (Player to Bank transaction)
                            Console.Write("Enter player's name: ");
                            string? playerNameToBank = Console.ReadLine();
                            Player? playerToBank = players.Find(p => p.Name.Equals(playerNameToBank, StringComparison.OrdinalIgnoreCase));

                            if (playerToBank != null)
                            {
                                Console.Write("Enter amount to pay to Bank: £");
                                if (int.TryParse(Console.ReadLine(), out int amountToBank))
                                {
                                    playerToBank.Debit(amountToBank);
                                    bank.ReceivePayment(amountToBank);
                                    Transaction transaction = new Transaction(playerToBank.Name, "Bank", amountToBank);
                                    bank.RecordTransaction(transaction);
                                    Console.WriteLine(transaction.GetTransactionDetails());
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount entered.\n");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Player not found.\n");
                            }
                            break;

                        case "3": // Draw Pot Luck Card
                            Console.Write("Enter player's name: "); // getting players name for card actions
                            string? playerDrawingCard = Console.ReadLine();
                            Player? drawingPlayer = players.Find(p => p.Name.Equals(playerDrawingCard, StringComparison.OrdinalIgnoreCase));

                            if (drawingPlayer != null)
                            {
                                Card potLuckCard = Cards.DrawTopCard(Cards.PotLuck);
                                Cards.ExecuteCardAction(potLuckCard, drawingPlayer, bank, players); // Passing the player drawing the card
                            }
                            else
                            {
                                Console.WriteLine("Player not found.\n");
                            }
                            break;

                        case "4": // Draw Opportunity Knocks Card
                            Console.Write("Enter player's name: "); // getting players name for card actions
                            playerDrawingCard = Console.ReadLine();
                            drawingPlayer = players.Find(p => p.Name.Equals(playerDrawingCard, StringComparison.OrdinalIgnoreCase));

                            if (drawingPlayer != null)
                            {
                                Card opportunityKnocksCard = Cards.DrawTopCard(Cards.OpportunityKnocks);
                                Cards.ExecuteCardAction(opportunityKnocksCard, drawingPlayer, bank, players); // Passing the player drawing the card
                            }
                            else
                            {
                                Console.WriteLine("Player not found.\n");
                            }
                            break;

                        default:
                            Console.WriteLine("Invalid transaction type. Please enter 1, 2, 3, or 4.\n");
                            break;
                    }
                }
                else if (response != null && response.Equals("No", StringComparison.OrdinalIgnoreCase))
                {
                    performTransaction = false;

                    // Display balances
                    Console.WriteLine("\nCurrent Balances:");
                    foreach (Player player in players)
                    {
                        Console.WriteLine($"{player.Name}: £{player.Balance}");
                    }
                    Console.WriteLine($"Bank: £{bank.TotalFunds}\n");
                    Console.WriteLine($"Free Parking: £{bank.FreeParking}\n");

                    Console.WriteLine("Thanks for playing Property Tycoon.");
                }
                else
                {
                    Console.WriteLine("Invalid response. Please enter 'Yes' or 'No'.");
                }
            }
        }
        // Method to display balances
        public static void DisplayBalances(List<Player> players, Bank bank)
        {
            Console.WriteLine("\nCurrent Balances:");
            foreach (Player player in players)
            {
                Console.WriteLine($"{player.Name}: £{player.Balance}");
            }
            Console.WriteLine($"Bank: £{bank.TotalFunds}\n");
            Console.WriteLine($"Free Parking: £{bank.FreeParking}\n");
        }
    }
}
