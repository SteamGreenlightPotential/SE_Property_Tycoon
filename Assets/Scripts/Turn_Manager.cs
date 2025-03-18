using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PropertyTycoon;

namespace PropertyTycoon
{
    public class Turn_Script : MonoBehaviour
    {
        public boardPlayer[] players; // Assigned the scripts from each piece in the Inspector
        public PropertyManager pmanager; // Assigned PropertyManager in Unity Inspector
        public int currentPlayerIndex = 0; // Tracks the current player
        public bool isWaitingForRoll = true; // Wait for player to press space to roll
        public int round = 1; // Tracks the current round of the game
        public bool turnEnded = false; // Tracks whether the player's turn has ended
        public int bankBalance = 50000; // Total funds in the bank
        public int freeParkingBalance = 0; // Funds in Free Parking
        public List<Player> playerlist = new List<Player>(); // Creates an array of player objects corresponding to board players

        public bool testMode = true; // "Test Mode" allows for hard-coded dice rolls for testing purposes

        public void Start()
        {
            Debug.Log("Round " + round); // Announce round 1 has started

            // Initialize player objects for each boardPlayer
            int i = 1;
            foreach (boardPlayer bplayer in players)
            {
                string name = ("player " + i.ToString());
                playerlist.Add(new Player(name, bplayer)); // Add each player to the player list
                i += 1;
            }

            StartTurn(); // Start the first turn
        }

        void Update()
        {
            // Wait for the player to press SPACE to roll the dice
            if (isWaitingForRoll && Input.GetKeyDown(KeyCode.Space))
            {
                isWaitingForRoll = false; // Prevent multiple rolls
                StartCoroutine(PlayerMovePhase(players[currentPlayerIndex]));
            }
        }

        void StartTurn()
        {
            turnEnded = false; // Disable the end turn button
            Debug.Log("Player " + (currentPlayerIndex + 1) + "'s Turn. Press SPACE to roll.");
            isWaitingForRoll = true; // Wait for player to press space before rolling
        }

        public IEnumerator PlayerMovePhase(boardPlayer player, bool testCase = false, int testRoll = 1, int testRoll2 = 1)
        {
            int roll = 0;
            int roll2 = 0; // Second dice roll for double roll

            // Use hardcoded dice rolls in test mode
            if (testMode)
            {
                roll = testRoll;
                roll2 = testRoll2;
            }
            else
            {
                // Roll the dice
                roll = Random.Range(1, 7);
                roll2 = Random.Range(1, 7);
                Debug.Log("Player " + (currentPlayerIndex + 1) + " rolled: " + roll);
            }

            // Jail logic
            if (player.inJail)
            {
                player.jailTurns += 1;
                if (player.jailTurns == 3)
                {
                    player.jailTurns = 0;
                    player.inJail = false;
                    player.Move(roll + roll2);
                    yield return new WaitForSeconds(roll * 0.2f + 0.5f);
                }
                else
                {
                    Debug.Log("Player is in jail. Press 'End turn' to end turn.");
                    yield break; // End the turn if the player is still in jail
                }
            }
            else
            {
                yield return player.Move(roll + roll2); // Move the player
            }

            if (!player.inJail)
            {
                // Update the current tile and ownership checks
                int currentTile = player.TileCount;
                bool tileOwned = false;
                int ownerIndex = -1;

                foreach (boardPlayer p in players)
                {
                    Player realPlayer = getPlayerFromBoard(p);

                    if (pmanager.getTileProperty(currentTile) == null)
                    {
                        continue; // Avoid null properties
                    }

                    if (pmanager.getTileProperty(currentTile).owner == realPlayer)
                    {
                        Debug.Log("Tile " + currentTile + " is owned by " + p.name);
                        tileOwned = true;
                        ownerIndex = System.Array.IndexOf(players, p);
                        break;
                    }
                }

                // Handle passing GO
                if (player.goPassed)
                {
                    player.balance += 200;
                    player.goPassed = false;
                    Debug.Log("PASSED GO");
                }

                // Handle wildcard tiles (tax, parking, jail)
                if (pmanager.getTileProperty(currentTile) == null)
                {
                    if (currentTile == 5 || currentTile == 39)
                    {
                        player.taxCheck();
                        freeParkingBalance += 100;
                        Debug.Log("GET TAXED");
                    }
                    else if (currentTile == 21)
                    {
                        player.balance += freeParkingBalance;
                        freeParkingBalance = 0;
                        Debug.Log("FREE PARKING :D");
                    }
                    else if (currentTile == 31)
                    {
                        yield return player.toJail();
                        player.TileCount = 11;
                        player.inJail = true;
                        player.goPassed = false;
                        Debug.Log("GO TO JAIL");
                    }
                }
                else if (tileOwned) // Handle owned tiles
                {
                    if (ownerIndex != currentPlayerIndex && !players[ownerIndex].inJail)
                    {
                        int rent = 50; // Temporary rent value
                        Debug.Log("Tile " + currentTile + " is owned by Player " + (ownerIndex + 1) + ". Paying rent £" + rent);
                        players[currentPlayerIndex].PayRent(rent, pmanager.getTileProperty(currentTile));
                    }
                    else
                    {
                        Debug.Log("Tile " + currentTile + " is owned by you.");
                    }
                }
                else // Handle unowned tiles
                {
                    Debug.Log("Tile " + currentTile + " is not owned by anyone and is available.");
                    Debug.Log("Press B to buy or SPACE to skip.");
                    bool decisionMade = false;
                    if (testCase) decisionMade = true;

                    while (!decisionMade)
                    {
                        if (Input.GetKeyDown(KeyCode.B))
                        {
                            Property property = pmanager.getTileProperty(currentTile);
                            player.BuyTile(property, getPlayerFromBoard(players[currentPlayerIndex]));
                            decisionMade = true;
                            bankBalance += 200;
                        }
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            Debug.Log("Purchase skipped.");
                            decisionMade = true;
                        }
                        yield return null;
                    }
                }
            }

            Debug.Log("Press End Turn now for next turn.");
            turnEnded = true;
        }

        public void EndTurnButtonClicked()
        {
            if (turnEnded)
            {
                StartCoroutine(EndTurn());
            }
        }

        public IEnumerator EndTurn()
        {
            Debug.Log("Ending Player " + (currentPlayerIndex + 1) + "'s Turn...");
            yield return new WaitForSeconds(0.5f);

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

            if (currentPlayerIndex == 0)
            {
                round += 1;
                Debug.Log("Round " + round);
            }

            StartTurn();
        }

        Player getPlayerFromBoard(boardPlayer player)
        {
            foreach (Player p in playerlist)
            {
                if (p.bPlayer == player)
                {
                    Debug.Log("Fetched player " + p.Name);
                    return p;
                }
            }
            return null;
        }
    }
}
