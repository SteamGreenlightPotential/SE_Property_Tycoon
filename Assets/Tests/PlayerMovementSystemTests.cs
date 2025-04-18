// Generated by AI. Original prompt: Write comprehensive system tests for Player_Movement.cs
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using PropertyTycoon;
using System.Reflection;


    public class PlayerMovementSystemTests
    {
        private GameObject playerObj;
        private GameObject buyScreenObj;
        private GameObject AucScreenObj;
        private GameObject upgradeScreenObj;
        private boardPlayer player;
        private PropertyManager propManager;
        private Turn_Script turnManager;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            //initialises the player, property manager and turn manager

            PlayerSelection.aiCount=0;
            PlayerSelection.numberOfPlayers=2;
            PlayerSelection.startScreenUsed=true;
            turnManager = new GameObject().AddComponent<Turn_Script>();
            //Stops Update running early 
            turnManager.startAsTest=true;
            // Initialize players and dependencies
            turnManager.players = new boardPlayer[6];
            GameObject[] playerObj= new GameObject[6]; 
             for (int i = 0; i < 6; i++)
        {
            playerObj[i] = new GameObject();
            turnManager.players[i] = playerObj[i].AddComponent<boardPlayer>();
            turnManager.players[i].name="player "+i.ToString();
        }
            player = turnManager.players[0];
            propManager = new GameObject().AddComponent<PropertyManager>();
            //propManager.initialiseProperties();

            //Initialise auction,upgrade and property screens boilerplate
            buyScreenObj = new GameObject();
            PropertyPurchaseScrn buyScreen = buyScreenObj.AddComponent<PropertyPurchaseScrn>();
            AucScreenObj = new GameObject();
            AuctionScrn aucScreen = AucScreenObj.AddComponent<AuctionScrn>();
            upgradeScreenObj = new GameObject();
            UpgradeScrn upscrn = upgradeScreenObj.AddComponent<UpgradeScrn>();
            upscrn.OwnedPropertyPanel= new GameObject();
            turnManager.upgradeScrn = upscrn;
            buyScreen.AuctionUI = aucScreen;
            turnManager.propertyPurchaseScrn = buyScreen;
            
            
            turnManager.pmanager=propManager; //adds property manager to turn manager
            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(playerObj);
            Object.DestroyImmediate(propManager.gameObject);
            Object.DestroyImmediate(turnManager.gameObject);
            foreach (var player in turnManager.players) Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(buyScreenObj);
            Object.DestroyImmediate(AucScreenObj);
            Object.DestroyImmediate(upgradeScreenObj);
        }

        [UnityTest]
        public IEnumerator Test_Movement_UpdatesPositionAndOwnership()
        {
              // Setup initial position at (0,0,0)
            player.transform.position = Vector3.zero;

            // Use reflection to invoke private coroutine
            var method = typeof(boardPlayer).GetMethod("ProcessMovements", BindingFlags.NonPublic | BindingFlags.Instance);
            IEnumerator coroutine = (IEnumerator)method.Invoke(player, new object[] { 6 });

            // Manually execute the coroutine
            while (coroutine.MoveNext())
            {
                if (coroutine.Current is IEnumerator nested)
                {
                    while (nested.MoveNext()) { /* Process nested coroutine */ }
                }
                yield return null;
            }

            // Assertions
            Assert.AreEqual(7, player.TileCount);
            Property currentTile = propManager.getTileProperty(player.TileCount);
            Assert.IsNotNull(currentTile);
        }
        
        // System Test: Rent payment between players
        [UnityTest]
        public IEnumerator Test_PayRent_TransfersBalance()
        {
            // 1. Create a boardPlayer for the owner and link it to a Player instance
            boardPlayer ownerBoardPlayer = new GameObject().AddComponent<boardPlayer>();
            Player ownerPlayer = new Player("Owner", ownerBoardPlayer);

            // 2. Get a property and have the owner buy it
            Property ownedProp = propManager.getTileProperty(2);
            ownerBoardPlayer.BuyTile(ownedProp,ownerPlayer);

            // 3. Test rent payment
            int initialBalance = player.balance;
            player.PayRent(50, ownedProp); // Pass Player instance directly
            
            Assert.AreEqual(initialBalance - 50, player.balance);

            // 4. Test too much rent payment
            player.balance = 10;
            player.PayRent(11,ownedProp);
            Assert.AreEqual(10,player.balance);
            
            // 5. Test Monopoly bonus
            player.balance = 1500;
            initialBalance=1500;
            Property prop2 = propManager.getTileProperty(4);
            ownerPlayer.bPlayer.BuyTile(prop2,ownerPlayer);
            player.PayRent(50,ownedProp);
            Assert.AreEqual(initialBalance-100,player.balance);

            // Cleanup
            Object.DestroyImmediate(ownerBoardPlayer.gameObject);
            yield return null;
        }
    
    // PlayerMovementSystemTests.cs
    [UnityTest]
    public IEnumerator Test_JailTeleportation()
    {
        // Setup
        player.TileCount = 30;
        int initialTile = player.TileCount;
        
        // Execute jail turn
        
        yield return turnManager.StartCoroutine(turnManager.PlayerMovePhase(player,true,1,0));
        
        // Verify
        Assert.AreEqual(11, player.TileCount);
        Assert.IsTrue(player.inJail);
        Assert.AreEqual(0, player.jailTurns);
    }

    [UnityTest]
    public IEnumerator Test_FreeParkingPayout()
    {
        turnManager.freeParkingBalance = 500;
        player.TileCount = 20;
        player.balance=1500;


        // Start the coroutine and wait for it to finish
        yield return turnManager.StartCoroutine(turnManager.PlayerMovePhase(player, true, 1, 0));

        
        // Verify
        Assert.AreEqual(2000, player.balance);
        Assert.AreEqual(0, turnManager.freeParkingBalance);

       
    }
            
    }
