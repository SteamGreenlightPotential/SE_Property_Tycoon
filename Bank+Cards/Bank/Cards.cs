using System;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class Card
    {
        public string Description { get; set; }
        public string Action { get; set; }

        public Card(string description, string action)
        {
            Description = description;
            Action = action;
        }
    }

    public static class Cards
    {
        public static List<Card> PotLuck = new List<Card>()
        {
            new Card("You inherit £200", "Bank pays player £200"),
            new Card("You have won 2nd prize in a beauty contest, collect £50", "Bank pays player £50"),
            new Card("You are up the creek with no paddle - go back to the Old Creek", "Player token moves backwards to the Old Creek"),
            new Card("Student loan refund. Collect £20", "Bank pays player £20"),
            new Card("Bank error in your favour. Collect £200", "Bank pays player £200"),
            new Card("Pay bill for text books of £100", "Player pays £100 to the bank"),
            new Card("Mega late night taxi bill pay £50", "Player pays £50 to the bank"),
            new Card("Advance to go", "Player moves forwards to GO"),
            new Card("From sale of Bitcoin you get £50", "Bank pays player £50"),
            new Card("Bitcoin assets fall - pay off Bitcoin short fall", "Player pays £50 to the bank"),
            new Card("Pay a £10 fine or take opportunity knocks", "If fine paid, player puts £10 on free parking"),
            new Card("Pay insurance fee of £50", "Player puts £50 on free parking"),
            new Card("Savings bond matures, collect £100", "Bank pays £100 to the player"),
            new Card("Go to jail. Do not pass GO, do not collect £200", "As the card says"),
            new Card("Received interest on shares of £25", "Bank pays player £25"),
            new Card("It's your birthday. Collect £10 from each player", "Player receives £10 from each player"),
            new Card("Get out of jail free", "Retained by the player until needed. No resale or trade value")
        };

        public static List<Card> OpportunityKnocks = new List<Card>()
        {
            new Card("Bank pays you divided of £50", "Bank pays player £50"),
            new Card("You have won a lip sync battle. Collect £100", "Bank pays player £100"),
            new Card("Advance to Turing Heights", "Player token moves forwards to Turing Heights"),
            new Card("Advance to Han Xin Gardens. If you pass GO, collect £200", "Player moves token"),
            new Card("Fined £15 for speeding", "Player puts £15 on free parking"),
            new Card("Pay university fees of £150", "Player pays £150 to the bank"),
            new Card("Take a trip to Hove station. If you pass GO collect £200", "Player moves token"),
            new Card("Loan matures, collect £150", "Bank pays £150 to the player"),
            new Card("You are assessed for repairs, £40/house, £115/hotel", "Player pays money to the bank"),
            new Card("Advance to GO", "Player moves token"),
            new Card("You are assessed for repairs, £25/house, £100/hotel", "Player pays money to the bank"),
            new Card("Go back 3 spaces", "Player moves token"),
            new Card("Advance to Skywalker Drive. If you pass GO collect £200", "Player moves token"),
            new Card("Go to jail. Do not pass GO, do not collect £200", "As the card says"),
            new Card("Drunk in charge of a hoverboard. Fine £30", "Player puts £30 on free parking"),
            new Card("Get out of jail free", "Retained by the player until needed. No resale or trade value")
        };


        // Draw top card method
        public static Card DrawTopCard(List<Card> cards)
        {
            Card topCard = cards[0]; // initializing top card

            cards.RemoveAt(0);
            cards.Add(topCard); // Moving Drawn card to the bottom of the deck

            return topCard;
        }

        // Draw top card and execute its action
        public static void ExecuteCardAction(Card card, Player player, Bank bank, List<Player> players)
        {
            Console.WriteLine($"{card.Description} - {card.Action}");

            switch (card.Action)
            {
                // Bank to Player transactions
                case "Bank pays player £200":
                    bank.MakePayment(player, 200);
                    break;
                case "Bank pays player £150":
                    bank.MakePayment(player, 150);
                    break;
                case "Bank pays player £100":
                    bank.MakePayment(player, 100);
                    break;
                case "Bank pays player £50":
                    bank.MakePayment(player, 50);
                    break;
                case "Bank pays player £25":
                    bank.MakePayment(player, 25);
                    break;
                case "Bank pays player £20":
                    bank.MakePayment(player, 20);
                    break;

                // Player to Bank transactions
                case "Player pays £150 to the bank":
                    player.Debit(150);
                    bank.ReceivePayment(150);
                    break;
                case "Player pays £100 to the bank":
                    player.Debit(100);
                    bank.ReceivePayment(100);
                    break;
                case "Player pays £50 to the bank":
                    player.Debit(50);
                    bank.ReceivePayment(50);
                    break;

                // Player to Player transactions
                case "Player receives £10 from each player":
                    foreach (Player p in players)
                    {
                        if (p != player)
                        {
                            bank.MakePayment(player, 10);
                            bank.MakePayment(p, -10);
                        }
                    }
                    break;

                // Free Parking Funds
                case "If fine paid, player puts £10 on free parking":
                    player.Debit(10);
                    bank.AddToFreeParking(10);
                    break;
                case "Player puts £50 on free parking":
                    player.Debit(50);
                    bank.AddToFreeParking(50);
                    break;
                case "Player puts £15 on free parking":
                    player.Debit(15);
                    bank.AddToFreeParking(15);
                    break;  
                case "Player puts £30 on free parking":      
                    player.Debit(30);
                    bank.AddToFreeParking(30);
                    break;

                default:
                    Console.WriteLine("Action not implemented yet.");
                    break;
            }
        }
    }
}
