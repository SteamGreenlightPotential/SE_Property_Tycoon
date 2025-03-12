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
        private int freeParkingBalance = 0; // Funds in Free Parking
        public List<Player> playerlist = new List<Player>(); // Creates an array of player objects corresponding to board players

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

            // Check if players were created properly
            foreach (Player player in playerlist)
            {
                Debug.Log(player.Name);
            }

            StartTurn(); // Start the first turn
        }

        void Update()
        {
            // Wait for the player to press SPACE to roll the dice
            if (isWaitingForRoll && Input.GetKeyDown(KeyCode.Space))
            {
                isWaitingForRoll = false; // Prevent multiple rolls
                StartCoroutine(PlayerMovePhase(players[currentPlayerIndex])); // Begin the player's movement phase
            }
        }

        void StartTurn()
        {
            turnEnded = false; // Disable the end turn button
            Debug.Log("Player " + (currentPlayerIndex + 1) + "'s Turn. Press SPACE to roll.");
            isWaitingForRoll = true; // Wait for player to press space before rolling
        }

        public IEnumerator PlayerMovePhase(boardPlayer player, bool testCase = false)
        {
            // Allow skipping input during test cases
            if (!testCase)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); // Wait for space input
            }

            // Roll the dice for player movement
            int roll = Random.Range(1, 7);
            Debug.Log("Player " + (currentPlayerIndex + 1) + " rolled: " + roll);

            player.Move(roll); // Move the player piece
            yield return new WaitForSeconds(roll * 0.2f + 0.5f); // Wait for the movement animation to complete

            int currentTile = player.TileCount; // Get the player's current tile
            bool tileOwned = false; // Tracks whether the tile is owned
            int ownerIndex = -1; // Tracks the owner of the tile

            // Check if the player has landed on a tax tile
            if (currentTile == 4 || currentTile == 38)
            {
                player.taxCheck(); // Deduct tax from the player's balance
                freeParkingBalance += 100; // Add the tax to Free Parking
            }

            // Loop through all players to check if the current tile is owned
            foreach (boardPlayer p in players)
            {
                Player realPlayer = getPlayerFromBoard(p);

                // Skip null properties
                if (pmanager.getTileProperty(currentTile) == null)
                {
                    continue;
                }

                // Check ownership
                if (pmanager.getTileProperty(currentTile).owner == realPlayer)
                {
                    Debug.Log("Tile " + currentTile + " is owned by " + (p.name));
                    tileOwned = true;
                    ownerIndex = System.Array.IndexOf(players, p); // Get the owner index
                    break;
                }
            }

            // When Property Not Owned (Added by Anik)
            if (!tileOwned)
            {
                Debug.Log("Tile " + currentTile + " is not owned by anyone and is available.");

                // Fetch the property from the current tile
                Property property = pmanager.getTileProperty(currentTile);

                // Show the property purchase screen for the current player
                FindFirstObjectByType<PropertyPurchaseScrn>().Show(property, getPlayerFromBoard(players[currentPlayerIndex]));
            }
            else // Handle owned tiles (e.g., paying rent)
            {
                if (ownerIndex != currentPlayerIndex)
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

            // End the turn after handling all tile logic
            turnEnded = true;
            Debug.Log("Press End Turn now for the next turn.");
        }

        public void EndTurnButtonClicked()
        {
            if (turnEnded)
            {
                StartCoroutine(EndTurn()); // Trigger the end turn coroutine
            }
        }

        IEnumerator EndTurn()
        {
            Debug.Log("Ending Player " + (currentPlayerIndex + 1) + "'s Turn...");
            yield return new WaitForSeconds(0.5f); // Small delay before moving to the next player

            // Move to the next player's turn
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

            // Increment the round if all players have taken their turn
            if (currentPlayerIndex == 0)
            {
                round += 1;
                Debug.Log("Round " + round);
            }

            StartTurn(); // Start the next player's turn
        }

        // Get the Player object corresponding to a boardPlayer object
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
