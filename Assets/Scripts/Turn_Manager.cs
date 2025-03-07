using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PropertyTycoon;

namespace PropertyTycoon{
    public class Turn_Script : MonoBehaviour{
        public boardPlayer[] players; // Assigned the scripts from each piece in the Inspector
        public PropertyManager pmanager; //Assigned PropertyManager in Unity Inspector
        public int currentPlayerIndex = 0;
        public bool isWaitingForRoll = true; // Wait for player to press space to roll
        public int round = 1;
        public bool turnEnded = false;
        public int bankBalance = 50000;
        private int freeParkingBalance = 0;
        public List<Player> playerlist=new List<Player>(); //Create an array of player objects corresponding to board players

        public bool testMode = true; //"Test Mode" allows for hard coded dice rolls for testing purposes


        public void Start(){
            Debug.Log("Round " + round); // Announce round 1 has started
            //add each board player to a player object
            int i = 1;
            foreach (boardPlayer bplayer in players){
                string name = ("player " + i.ToString());
                playerlist.Add(new Player(name,bplayer));
                i += 1;
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

        public IEnumerator PlayerMovePhase(boardPlayer player,bool testCase=false)
        {
            int roll = 0;
            //Lets me skip input, because this isnt modular i gotta do this
            if (testCase==false){
                // Wait for player to press space before rolling
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }
            
            //If test mode is on, roll hard coded number for test reasons 
            if (testMode==true){
                roll = 10;
            }
            else{
                
                // Roll the dice
                roll = Random.Range(1, 7); // Roll dice for movement
                Debug.Log("Player " + (currentPlayerIndex + 1) + " rolled: " + roll);
            }
            
            player.Move(roll); // Move the player
            yield return new WaitForSeconds(roll * 0.2f + 0.5f); // Wait for movement to finish

            //temp bank stuff           -----------------------------------------------------------------------------------------------
            int currentTile = player.TileCount;
            bool tileOwned = false;
            int ownerIndex = -1;
            
            int i = 0;
            foreach (boardPlayer p in players)  // Loop through all players to see if any own the current tile
            {
                Player realplayer = getPlayerFromBoard(p);
                
                if (pmanager.getTileProperty(currentTile)==null){
                    //Avoids crashing from a null
                    continue;
                } 
                if (pmanager.getTileProperty(currentTile).owner == realplayer)
                {
                    Debug.Log("Tile " + currentTile + " is owned by " + (p.name));
                    tileOwned = true;
                    ownerIndex = i;
                    break; // Terminate loop immediately
                }
                i += 1;
            }

            //If player passed GO, give them their money. 
            if (player.goPassed == true){
                player.balance += 200;
                player.goPassed = false;
                Debug.Log("PASSED GO");
            }
            
            //Check if tile is a wildcard
            if (pmanager.getTileProperty(currentTile) == null){
                
                //Checks whether current tile is tax
                if (currentTile == 5 || currentTile == 39){
                //TAX THEM
                player.taxCheck();
                freeParkingBalance += 100;
                Debug.Log("GET TAXED");
                }

                //Checks whether current tile is pot luck or opportunity knocks
                //if (currentTile == )

                //Checks whether current tile is free parking
                if (currentTile == 21){
                    player.balance += freeParkingBalance;
                    freeParkingBalance = 0;
                    Debug.Log("FREE PARKING :D");
                }
                
                //checks whether current tile is on go to jail. If they are, send them to jail
                if (currentTile == 31){
                    yield return player.toJail();
                    player.TileCount =11;
                    Debug.Log("GO TO JAIL");
                    player.inJail = true;

                    //Prevent go money coming in next turn from "passing go"
                    player.goPassed = false;
                }

            }
            else if (tileOwned) // If tile is owned makes player pay rent unlesss they own it
            {
                if (ownerIndex != currentPlayerIndex)
                {
                    int rent = 50; //Temp value
                    Debug.Log("Tile " + currentTile + " is owned by Player " + (ownerIndex + 1) + ". Paying rent £" + rent);
                    players[currentPlayerIndex].PayRent(rent,pmanager.getTileProperty(currentTile));
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
                        player.BuyTile(property,getPlayerFromBoard(players[currentPlayerIndex]));
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

        //Get the player object corresponding to a player's board object
        Player getPlayerFromBoard(boardPlayer player){
            foreach (Player p in playerlist){
                if (p.bPlayer == player){
                    Debug.Log("Fetched player "+ p.Name);
                    return p;
                    
                }

            }
            return null;
        }


    }
}