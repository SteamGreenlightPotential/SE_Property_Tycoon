using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PropertyTycoon
{
    public class Card
    {
        public string Description { get; private set; }
        public string Action { get; private set; }

        public Card(string description, string action)
        {
            Description = description;
            Action = action;
        }
    }

    public static class Cards
    {
        private static PropertyManager propertyManager = GameObject.FindFirstObjectByType<PropertyManager>();
        
        public static List<Card> PotLuck = new List<Card>()
        {
            new Card("You inherit £200", "Bank pays player £200"),
            new Card("You have won 2nd prize in a beauty contest, collect £50", "Bank pays player £50"),
            new Card("You are up the creek with no paddle - go back to the Old Creek", "Player moves token to The Old Creek"), 
            new Card("Student loan refund. Collect £20", "Bank pays player £20"),
            new Card("Bank error in your favour. Collect £200", "Bank pays player £200"),
            new Card("Pay bill for text books of £100", "Player pays £100 to the bank"),
            new Card("Mega late night taxi bill pay £50", "Player pays £50 to the bank"),
            new Card("Advance to GO", "Player moves forwards to GO"),
            new Card("From sale of Bitcoin you get £50", "Bank pays player £50"),
            new Card("Bitcoin assets fall - pay off Bitcoin shortfall", "Player pays £50 to the bank"),
            new Card("Pay a £10 fine or take Opportunity Knocks", "If fine paid, player puts £10 on free parking"),
            new Card("Pay insurance fee of £50", "Player puts £50 on free parking"),
            new Card("Savings bond matures, collect £100", "Bank pays £100 to the player"),
            new Card("Go to jail. Do not pass GO, do not collect £200", "As the card says"),
            new Card("Received interest on shares of £25", "Bank pays player £25"),
            new Card("It's your birthday. Collect £10 from each player", "Player receives £10 from each player"),
            new Card("Get out of jail free", "Retained by the player until needed. No resale or trade value")
        };
        public static List<Card> OpportunityKnocks = new List<Card>()
        {
            new Card("Bank pays you a dividend of £50", "Bank pays player £50"),
            new Card("You have won a lip sync battle. Collect £100", "Bank pays player £100"),
            new Card("Advance to Turing Heights", "Player moves token to Turing Heights"),
            new Card("Advance to Han Xin Gardens. If you pass GO, collect £200", "Player moves token to Han Xin Gardens"),
            new Card("Fined £15 for speeding", "Player puts £15 on free parking"),
            new Card("Pay university fees of £150", "Player pays £150 to the bank"),
            //new Card("Take a trip to Hove station. If you pass GO, collect £200", "Player moves token"),
            new Card("Loan matures, collect £150", "Bank pays £150 to the player"),
            new Card("You are assessed for repairs, £40/house, £115/hotel", "Player pays money to the bank"),
            new Card("Advance to GO", "Player moves token"),
            new Card("You are assessed for repairs, £25/house, £100/hotel", "Player pays money to the bank"),
            //new Card("Go back 3 spaces", "Player moves token"),
            new Card("Advance to Skywalker Drive. If you pass GO, collect £200", "Player moves token to Skywalker Drive"),
            new Card("Go to jail. Do not pass GO, do not collect £200", "Go to jail"),
            new Card("Drunk in charge of a hoverboard. Fine £30", "Player puts £30 on free parking"),
            new Card("Get out of jail free", "Player leaves jail instantly")
        };

        // Draw the top card from the deck
        public static Card DrawTopCard(List<Card> cards)
        {
            Card topCard = cards[0];
            cards.RemoveAt(0);
            cards.Add(topCard); // Moves the card to the bottom of the deck
            return topCard;
        }

        // Shuffle the deck of cards
        public static void Shuffle(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Card temp = cards[i];
                int randomIndex = Random.Range(i, cards.Count);
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }

        public static IEnumerator ExecuteCardAction(Card card, Player player, Bank bank, List<Player> players)
        {
            Debug.Log($"{card.Description} - {card.Action}");
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
                            p.Debit(10);
                            player.Credit(10);
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

                // Player movement related cards
                case "Player moves forwards to GO":
                    int stepsToGo = 40 - player.bPlayer.TileCount;
                    player.bPlayer.Move(stepsToGo);
                    break;

                case "Player moves token to Turing Heights":
                    yield return MoveToProperty("Turing Heights", 40, player);
                    break;

                case "Player moves token to Han Xin Gardens":
                    yield return MoveToProperty("Han Xin Gardens", 25, player);
                    break;

                case "Player moves token to Hove station":
                    yield return MoveToProperty("Hove station", 15, player);
                    break;

                case "Player moves token to Skywalker Drive":
                    yield return MoveToProperty("Skywalker Drive", 12, player);
                    break;

                case "Player moves token to The Old Creek":
                    yield return MoveToProperty("The Old Creek", 2, player);
                    player.bPlayer.goPassed=false; //No go passed because player is moving backwards
                    break;

                case "Player moves token backwards 3 spaces":
                    player.bPlayer.TileCount -= 3;
                    if (player.bPlayer.TileCount < 0)
                        player.bPlayer.TileCount += 40;
                    player.bPlayer.Move(-3);
                    break;

                case "Go to jail":
                    player.bPlayer.inJail = true;       // Mark the player as being in jail
                    player.bPlayer.jailTurns = 0;       // Reset jail turn count
                    player.bPlayer.StartCoroutine(player.bPlayer.toJail()); // Move the player to jail
                    break;

                
                case "Player pays for property repairs":
                    int repairCostHouse = card.Description.Contains("£40") ? 40 : 25;
                    int repairCostHotel = card.Description.Contains("£115") ? 115 : 100;
                    int totalRepairCost = (player.HousesOwned * repairCostHouse) + (player.HotelsOwned * repairCostHotel);
                    player.Debit(totalRepairCost);
                    bank.ReceivePayment(totalRepairCost);
                    break;

                case "Get out of jail free":
                    GetOutOfJail(player, bank); // Useing the method
                    break;

                default:
                    Debug.Log("Action not implemented");
                    Shuffle(PotLuck);
                    Shuffle(OpportunityKnocks);
                    break;
            }
        }

        public static void GetOutOfJail(Player player, Bank bank)
        {
            if (player.HasGetOutOfJailCard) // Check for "Get Out of Jail Free" card
            {
                player.HasGetOutOfJailCard = false; // Use the card
                player.bPlayer.inJail = false; // Release the player
                Debug.Log($"{player.Name} used a Get Out of Jail Free card!");
            }
            else if (player.Balance >= 50) // Check if the player can pay £50
            {
                player.Debit(50); // Deduct £50 from their balance
                bank.ReceivePayment(50); // Send the payment to the bank
                player.bPlayer.inJail = false; // Release the player
                Debug.Log($"{player.Name} paid £50 to get out of jail!");
            }
            else // DOUBLE ROLL METHOD GOES HERE 
            {
                Debug.Log($"{player.Name} must roll for doubles to get out of jail.");
            }
        }

        private static IEnumerator MoveToProperty(string propertyName, int tileNo, Player player)
        {
            Property property = propertyManager.getTileProperty(tileNo);
            if (property != null)
            {
                int stepsToProperty = (property.tileno - player.bPlayer.TileCount + 40) % 40;
                // set timescale to 100 to save me time making a player teleport
                float originalTimeScale = Time.timeScale;
                Time.timeScale = 100f; //make piece "teleport" to location
                yield return player.bPlayer.StartCoroutine(player.bPlayer.ProcessTeleport(stepsToProperty,originalTimeScale));

            }
        }
    }
}