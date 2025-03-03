using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PropertyTycoon;

namespace PropertyTycoon{
    public class Turn_Script : MonoBehaviour{
        public boardPlayer[] players; // Assigned the scripts from each piece in the Inspector

        public PropertyManager pmanager; //Assigned PropertyManager in Unity Inspector
        private int currentPlayerIndex = 0;
        private bool isWaitingForRoll = true; // Wait for player to press space to roll
        private int round = 1;
        private bool turnEnded = false;
        private int bankBalance = 50000;
        private int freeParkingBalance = 0;
        public List<Player> playerlist; //Create an array of player objects corresponding to board players


        void Start(){
            Debug.Log("Round " + round); // Announce round 1 has started
            //add each board player to a player object
            int i = 1;
            foreach (boardPlayer bplayer in players){
                string name = ("player " + i.ToString());
                playerlist.Add(new Player(name,bplayer));
                i += 1;
            }
            //Checking they were made properly
            foreach (Player player in playerlist){
                Debug.Log(player.Name);
            }
            StartTurn();
        }

        void Update()
        {
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

        IEnumerator PlayerMovePhase(boardPlayer player)
        {
            // Wait for player to press space before rolling
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            // Roll the dice
            int roll = Random.Range(1, 7); // Roll dice for movement
            Debug.Log("Player " + (currentPlayerIndex + 1) + " rolled: " + roll);

            player.Move(roll); // Move the player
            yield return new WaitForSeconds(roll * 0.2f + 0.5f); // Wait for movement to finish

            //temp bank stuff           -----------------------------------------------------------------------------------------------
            int currentTile = player.TileCount;
            bool tileOwned = false;
            int ownerIndex = -1;
            
            //Checks whether current tile is tax
            if (currentTile == 4 || currentTile == 38)
            {
                player.taxCheck();
                freeParkingBalance += 100;
            }

            for (int i = 0; i < players.Length; i++)  // Loop through all players to see if any own the current tile
            {
                if (players[i].OwnedProperties.tileno.Contains(currentTile))
                {
                    Debug.Log("Tile " + currentTile + " is owned by Player " + (i + 1));
                    tileOwned = true;
                    ownerIndex = i;
                    break; // Terminate loop immediately
                }
            }

            if (tileOwned) // If tile is owned makes player pay rent unlesss they own it
            {
                if (ownerIndex != currentPlayerIndex)
                {
                    int rent = 50; //Temp value
                    Debug.Log("Tile " + currentTile + " is owned by Player " + (ownerIndex + 1) + ". Paying rent £" + rent);
                    players[currentPlayerIndex].PayRent(players[ownerIndex], rent);
                }
                else
                {
                    Debug.Log("Tile " + currentTile + " is owned by you.");
                }
            }
            else // Allows player to buy an unowned tile
            {
                Debug.Log("Tile " + currentTile + " is not owned by anyone and is available.");
                Debug.Log("Press B to buy or Space to skip.");
                bool decisionMade = false;
                while (!decisionMade)
                {
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        //Fetches property using current tile 
                        Property property = pmanager.getTileProperty(currentTile);
                        player.BuyTile(property);
                        decisionMade = true;
                        bankBalance += 200;
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Debug.Log("Purchase skipped.");
                        decisionMade = true;
                    }
                    yield return null; // Pauses coroutine until next frame
                }
            }

            //temp bank stuff ends        --------------------------------------------------------------------------------------------------

            Debug.Log("Press Space to Skip (THIS IS FOR THE BUY PHASE LATER)");
            Debug.Log("Press End Turn now for next turn");

            // Wait for the player to press space to continue
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            turnEnded = true; // Enable the end turn button
        }

        public void EndTurnButtonClicked()
        {
            if (turnEnded == true)
            {
                StartCoroutine(EndTurn()); // Detect when the End_Turn_Script is triggered
            }

        }

        IEnumerator EndTurn()
        {
            Debug.Log("Ending Player " + (currentPlayerIndex + 1) + "'s Turn...");
            yield return new WaitForSeconds(0.5f); // Wait before moving to the next turn

            // Move to the next player
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

            if (currentPlayerIndex == 0)
            {
                round += 1; // Increment round number if last player finishes their turn
                Debug.Log("Round " + round); // Prints next round number
            }

            // Start the next player's turn
            StartTurn();
        }


    }
}