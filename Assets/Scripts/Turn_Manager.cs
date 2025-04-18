using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using UnityEditor;


namespace PropertyTycoon
{
    public class Turn_Script : MonoBehaviour
    {
        // References set in the Inspector
        public boardPlayer[] players; // Array of board players (assigned in the Unity Inspector)
        public PropertyManager pmanager; // Reference to the PropertyManager (manages properties)
        public PropertyPurchaseScrn propertyPurchaseScrn; // UI for property purchase
        //public UpgradeScrn upgradeScrn; // Reference to the OwnedPropertyUI script

        public MortgageScreen mortgageScreen;
        public UpgradeScrn upgradeScrn; // Reference to the OwnedPropertyUI script
        public Bank bank = new Bank();


        // Game state variables
        public int currentPlayerIndex = 0; // Tracks the current player's turn
        public bool isWaitingForRoll = true; // True when waiting for the player to press SPACE to roll dice
        public int round = 1; // Tracks the current game round
        public bool turnEnded = false; // True when the player's turn ends
        public int bankBalance = 50000; // Total money in the bank
        public int freeParkingBalance = 0; // Funds available on Free Parking
        public List<Player> playerlist = new List<Player>(); // List of Player objects corresponding to board players
        public static bool purchaseDone = true;    //Global bool to stop turn from continuing without property decicion being made

        public bool startAsTest = false; //Stops Update running by itself in test cases

        public DiceRoller droll;
        public DiceRoller droll2;


        [System.Obsolete]
        public void Start()
        {
            Debug.Log("Round " + round); // Announce round 1 has started
            int playerCount = PlayerSelection.numberOfPlayers;
            int AiCount = PlayerSelection.aiCount;
            bool startScreenUsed = PlayerSelection.startScreenUsed;

            if (!startScreenUsed){
                playerCount = 6; 
                AiCount=0;
            }


            //Make player array match number of players
            boardPlayer[] temparray = new boardPlayer[playerCount];
            for (int j = 0;j<playerCount;j++){
                temparray[j] = players[j];
                //Debug.Log($"Added {players[j].name} to reduced player list");
            }
            //DESTROY unused players
            for (int j = players.Length-(players.Length-playerCount);j<players.Length;j++){
                GameObject.DestroyImmediate(players[j].gameObject);
            }
            players = temparray;

            // Initialize Player objects for each board player
            int i = 1;
            foreach (boardPlayer bplayer in players)
            {
                string name = "Player " + i;
                playerlist.Add(new Player(name, bplayer)); // Link board player to player logic
                //Debug.Log($"Added {name} to player list.");
                i++;
            }
            upgradeScrn = FindObjectOfType<UpgradeScrn>(); // Find the UpgradeScrn in the scene
            if (upgradeScrn == null)
            {
                //Debug.Log("UpgradeScrn not found in the scene!");
            }
            //Avoid too many AI
            if (players.Length<AiCount){
                AiCount=players.Length;
            }

             //Logic to assign AI
            
            for (int j = playerlist.Count-1; j > playerlist.Count - AiCount-1; j--)
            {
                playerlist[j].isAI=true;
                //Debug.Log($"{players[j].name} assigned as AI");
            }



        }

        public void Update()
        {

            if (isWaitingForRoll &&playerlist[currentPlayerIndex].isAI==false&&startAsTest==false)
            {
                isWaitingForRoll = false; // Prevent multiple rolls
                StartCoroutine(PlayerMovePhase(players[currentPlayerIndex])); // Start the move phase
            }
            else if (isWaitingForRoll&&playerlist[currentPlayerIndex].isAI){
                isWaitingForRoll=false;
                StartCoroutine(PlayerMovePhase(players[currentPlayerIndex]));
                
            }
        }

        public void StartTurn()
        {
            turnEnded = false; // Reset end turn state
            Debug.Log($"Player {currentPlayerIndex + 1}'s Turn. Press SPACE to roll.");
            Turn_Script.Instance.CheckBankruptcy(playerlist[currentPlayerIndex]);
            isWaitingForRoll = true; // Wait for player input to roll dice
        }

        public IEnumerator PlayerMovePhase(boardPlayer player, bool testMode = false, int testRoll = 10, int testRoll2 = 20)
        {
            bool isAi = getPlayerFromBoard(player).isAI;
            //SORRY THIS IS STUPID BUT UPDATE WORKS WEIRD WITH AI AND TESTS IM SORRY

            if(isAi &&isWaitingForRoll==true){
                isWaitingForRoll=false;
            }


            bool repeatturn = true;
            int loopcount = 1;
            bool jailBound = false;

            
            //Allows for moving again on doubles 
            while (repeatturn){
                //testMode = true; // THIS IS TEST PLEASE PLEASE PLEASE GET RID OF AFTER
                int roll = 0;
                int roll2 = 0; // Second dice roll for handling doubles

                // Use test rolls in test mode
                if (testMode)
                {
                    roll = testRoll;
                    roll2 = testRoll2;
                }
                else if (isAi){
                    // Roll the dice for the player
                    roll = Random.Range(1, 7);
                    roll2 = Random.Range(1, 7);
                    Debug.Log($"Player {currentPlayerIndex + 1} rolled: {roll} and {roll2}");
                }
                else
                {
                    // Show both dice and prepare them for rolling
                    droll.GetComponent<DiceRoller>().PrepareRoll();
                    droll2.GetComponent<DiceRoller>().PrepareRoll();

                    // Wait for player to press Space to roll
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

                    // Roll both dice simultaneously
                    var dice1 = droll.GetComponent<DiceRoller>();
                    var dice2 = droll2.GetComponent<DiceRoller>();
                    
                    // Start both rolls at the same time
                    Coroutine roll1 = StartCoroutine(dice1.CompleteRoll());
                    Coroutine roll2Coroutine = StartCoroutine(dice2.CompleteRoll());
                    
                    // Wait for both rolls to complete
                    yield return roll1;
                    yield return roll2Coroutine;
                

                    roll = droll.GetComponent<DiceRoller>().Result;
                    roll2 = droll2.GetComponent<DiceRoller>().Result; 
                
                    Debug.Log("Player " + (currentPlayerIndex + 1) + " rolled: " + (roll + roll2));
                    //Only shuffle cards out of testmode
                    Cards.Shuffle(Cards.OpportunityKnocks);
                    Cards.Shuffle(Cards.PotLuck);
                }

                
                if (loopcount>3){
                    jailBound=true;
                    repeatturn=false;
                }
                else if (roll!=roll2){
                    repeatturn=false;
                }
                else{
                    if(player.inJail==true){
                        player.inJail=false;
                        Debug.Log("Broke out of jail!");
                    }
                    loopcount++;
                }

                // Handle jail logic
                if (player.inJail)
                {
                    player.jailTurns += 1;

                    if (player.jailTurns == 3)
                    {
                        // Player leaves jail on the third turn
                        player.jailTurns = 0;
                        player.inJail = false;
                        player.Move(roll + roll2); // Move the player
                        yield return new WaitForSeconds(roll * 0.2f + 0.5f); // Simulate delay
                    }
                    else
                    {
                        Debug.Log("Player is in jail. Press 'End turn' to finish the turn.");
                        if (isAi){
                            StartCoroutine(EndTurn());
                        }
                        
                    }
                }
                else if (!jailBound)
                {
                    yield return player.Move(roll + roll2); // Move the player normally
                }

                // Post-movement logic
                if (!player.inJail&&jailBound==false)
                {
                    int currentTile = player.TileCount; // Get the current tile of the player
                    bool tileOwned = false; // Flag to check if tile is owned
                    int ownerIndex = -1; // Index of the player who owns the tile

                    // Check if the tile is owned by another player
                    foreach (boardPlayer p in players)
                    {
                            Player realPlayer = getPlayerFromBoard(p);
                        Property property = pmanager.getTileProperty(currentTile);

                        if (property == null)
                            continue; // Skip null properties

                        if (property.owner == realPlayer && realPlayer != null)
                        {
                            Debug.Log($"Tile {currentTile} is owned by {p.name}");
                            tileOwned = true;
                            ownerIndex = System.Array.IndexOf(players, p);
                            break;
                        }
                    }

                    // Handle scenarios based on the type of tile
                    if (player.goPassed)
                    {
                        // Player passed GO, reward them
                        player.balance += 200;
                        player.goPassed = false;
                        Debug.Log("PASSED GO! Get 200");
                        Turn_Script.Instance.CheckBankruptcy(getPlayerFromBoard(player)); // Check for bankruptcy
                    }

                    Property landedProperty = pmanager.getTileProperty(currentTile);
                    if (landedProperty == null)
                    {
                        // Handle special tiles like taxes, jail, or parking
                        yield return HandleSpecialTiles(currentTile, player);
                    }
                    else if (landedProperty.owner == getPlayerFromBoard(player)) // Player owns the tile
                    {
                        CheckOwnership(player, landedProperty); // Call CheckOwnership method
                    }
                    else if (tileOwned)
                    {
                        // Tile is owned, handle rent payment
                        HandleOwnedTile(player, landedProperty, ownerIndex);
                    }
                    else
                    {
                        // Tile is unowned, trigger property purchase
                        if (!isAi){
                        Debug.Log($"Tile {currentTile} is not owned by anyone and is available.");
                        ShowPropertyPurchaseScreen(player, landedProperty);
                        }
                        else if (landedProperty.price < player.balance){
                        Player pObject = getPlayerFromBoard(player);
                        pObject.Debit(landedProperty.price);
                        pObject.AddProperty(landedProperty);
                        landedProperty.SwitchOwner(pObject);
                        }
                        else{
                        propertyPurchaseScrn.manualAuction(landedProperty);
                        }
                        //if (testMode==true){purchaseDone=true;}
                        
                    }
                    if(testMode==true){purchaseDone=true;}
                    //Stop the turns advancing while purchase screen is open
                    while (purchaseDone==false){
                        yield return null; 
                    }




                }
                //Sends player to jail for speeding
                if (jailBound){
                    yield return StartCoroutine(player.toJail());
                    player.TileCount = 11; // Jail tile index
                    player.inJail = true;
                    player.goPassed = false;
                    Debug.Log("Speeding, go to jail");
                }   
                
                //Stops player continuing out of jail if they double on go to jail
                if(player.inJail==true){
                    repeatturn=false;
                }

        }
            if (isAi&&testMode==false){
                yield return StartCoroutine(EndTurn());
            }
                            
            else{
            // Indicate that the turn can be ended
            Debug.Log("Press End Turn now for the next turn.");
            turnEnded = true;
            }

            //Make sure you don't continue if you landed on
        }

        private IEnumerator HandleSpecialTiles(int currentTile, boardPlayer player)
        {
            // Handle tax, parking, chance or jail tiles
            if (currentTile == 5 || currentTile == 39) // Tax tiles
            {
                player.taxCheck();
                Turn_Script.Instance.CheckBankruptcy(getPlayerFromBoard(player)); // Check for bankruptcy
                freeParkingBalance += 100;
                Debug.Log("GET TAXED");
            }
            else if (currentTile==3||currentTile==18||currentTile==34){
                Debug.Log("Pot luck");
                Card currentCard = Cards.DrawTopCard(Cards.PotLuck);
                yield return Cards.ExecuteCardAction(currentCard,getPlayerFromBoard(player),bank,playerlist);


            }
            else if (currentTile==8||currentTile==23){
                Debug.Log("OPPORTUNITY KNOCKS");
                
                Card currentCard = Cards.DrawTopCard(Cards.OpportunityKnocks);
                yield return Cards.ExecuteCardAction(currentCard,getPlayerFromBoard(player),bank,playerlist);
                



            }
            else if (currentTile == 21) // Free parking
            {
                player.balance += freeParkingBalance;
                freeParkingBalance = 0;
                Debug.Log("FREE PARKING :D");

                Turn_Script.Instance.CheckBankruptcy(getPlayerFromBoard(player)); // Check for bankruptcy
            }
            else if (currentTile == 31) // Go to jail
            {
                yield return StartCoroutine(player.toJail());
                player.TileCount = 11; // Jail tile index
                player.inJail = true;
                player.goPassed = false;
                Debug.Log("GO TO JAIL");
            }
        }

        private void HandleOwnedTile(boardPlayer player, Property property, int ownerIndex)
        {
            // Handle rent payments or ownership checks
            if (ownerIndex != currentPlayerIndex && !players[ownerIndex].inJail && property.mortgaged==false)
            {
                int rent = property.baseRent; // Base rent value
                Debug.Log($"Paying rent of £{rent} to Player {ownerIndex + 1}");
                players[currentPlayerIndex].PayRent(rent, property);
                Turn_Script.Instance.CheckBankruptcy(getPlayerFromBoard(player)); // Check for bankruptcy
            }
            else if (property.mortgaged==true){
                Debug.Log("Property mortgaged - no rent paid");
            }
            else
            {
                //Debug.Log("You own this property. No rent required.");
            }


        }
        
        private void ShowPropertyPurchaseScreen(boardPlayer player, Property property)
        {
            if (propertyPurchaseScrn != null && property != null)
            {
                Player currentPlayer = getPlayerFromBoard(player);
                if (currentPlayer != null)
                {
                    //Debug.Log($"Triggering purchase screen for Property: {property.name}, Player: {currentPlayer.Name}");
                    propertyPurchaseScrn.Show(property, currentPlayer);
                    
                }
                else
                {
                    //Debug.LogError("CurrentPlayer is null in ShowPropertyPurchaseScreen!");
                }
            }
            else
            {
                //Debug.LogError("PropertyPurchaseScrn or Property is null in ShowPropertyPurchaseScreen!");
            }

            
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
            // End the current player's turn
            //Debug.Log($"Ending Player {currentPlayerIndex + 1}'s Turn...");
            yield return new WaitForSeconds(0.5f); // Simulate a delay

            // Move to the next player
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

            // Increment the round if all players have taken their turn
            if (currentPlayerIndex == 0)
            {
                round++;
                Debug.Log($"Round {round}");
                //GameManager.Instance.DisplayBalances(); //Show everyones balance after a round

            }

            StartTurn(); // Start the next player's turn
        }

        private Player getPlayerFromBoard(boardPlayer player)
        {
            // Find the Player object linked to the board player
            foreach (Player p in playerlist)
            {
                if (p.bPlayer == player)
                {
                    //Debug.Log($"Fetched player {p.Name}");
                    return p;
                }
            }
            return null; // Return null if no match is found
        }

        public static Turn_Script Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                //Debug.LogError("Multiple Turn_Script instances detected!");
                Destroy(gameObject);
            }
        }

        
        public void CheckOwnership(boardPlayer player, Property property)
        {
            Player realPlayer = getPlayerFromBoard(player);

            if (property.owner == realPlayer) // Player owns the property
            {
                Debug.Log($"Player {realPlayer.Name} landed on their property: {property.name}");

                // Show the Owned Property Panel
                if (upgradeScrn != null && realPlayer.isAI==false)
                {
                    upgradeScrn.ShowOwnedPropertyPanel(property, realPlayer);
                }
                else
                {
                    //Debug.LogError("OwnedPropertyUI reference is missing!");
                }
            }
        }

        public void CheckBankruptcy(Player player)
        {
            // Check if the player is already flagged as bankrupt
            if (player.IsBankrupt)
            {
                if (player.Balance < 0) // Still bankrupt after one turn
                {
                    Debug.Log($"{player.Name} is eliminated due to bankruptcy!");

                    // Step 1: Transfer assets to the bank or creditors
                    foreach (var property in player.OwnedProperties)
                    {
                        property.owner = null; // Reset property ownership (or transfer to the bank)
                        Debug.Log($"{player.Name}'s property {property.name} is now unowned.");
                    }
                    player.OwnedProperties.Clear(); // Clear the player's owned properties

                    // Step 2: Remove the player from the game
                    playerlist.Remove(player);
                    Debug.Log($"{player.Name} has been removed from the game.");
                }
            }
            else
            {
                if (player.Balance < 0) // Bankrupt for the first time
                {
                    Debug.Log($"{player.Name} is bankrupt! Flagging them for elimination next turn.");
                    player.IsBankrupt = true; // Set the bankruptcy flag
                }
            }
        }
    }
}