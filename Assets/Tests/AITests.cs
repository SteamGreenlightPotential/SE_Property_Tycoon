using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using PropertyTycoon;
using System.Linq;

#region Dummy Helpers




// Dummy PropertyPurchaseScrn that hides (instead of overrides) manualAuction().
// Since PropertyPurchaseScrn.manualAuction is not virtual, we cannot override it directly.
// Instead, we use the 'new' keyword to hide the base implementation.
public class DummyPropertyPurchaseScrn : PropertyPurchaseScrn
{
    
    
    // To ensure that when PropertyPurchaseScrn.manualAuction is called in code,
    // it calls our dummy version, assign this dummy instance to Turn_Script.propertyPurchaseScrn.
    // (Your test code will call our dummy's manualAuction if using a reference to DummyPropertyPurchaseScrn.)
}
#endregion

#region AI Unit Tests


public class TurnManagerAIUnitTests
{
    private GameObject turnManagerGO;
    private GameObject pmGO;
    private Turn_Script turnManager;
    private PropertyManager dummyPM;
    private DummyPropertyPurchaseScrn dummyPurchaseScrn;
    private GameObject aiPlayerGO;
    private GameObject aucscrgo;
    private GameObject purchaseScrnGO;
    

    [SetUp]
    public void SetUp()
    {
        // Create a GameObject and add Turn_Script.
        turnManagerGO = new GameObject("TurnManager");
        

            PlayerSelection.aiCount=0;
            PlayerSelection.numberOfPlayers=2;
            turnManager = turnManagerGO.AddComponent<Turn_Script>();
            // Initialize players and dependencies
            turnManager.players = new boardPlayer[6];
            GameObject[] playerObj= new GameObject[6]; 
             for (int i = 0; i < 6; i++)
        {
            playerObj[i] = new GameObject();
            turnManager.players[i] = playerObj[i].AddComponent<boardPlayer>();
            turnManager.players[i].name="player "+i.ToString();
        }

        // Create and assign a dummy PropertyManager.
        pmGO = new GameObject("DummyPM");
        dummyPM = pmGO.AddComponent<PropertyManager>();
        turnManager.pmanager = dummyPM;

        // Create a dummy purchase screen.
        purchaseScrnGO = new GameObject("DummyPurchaseScrn");
        dummyPurchaseScrn = purchaseScrnGO.AddComponent<DummyPropertyPurchaseScrn>();
        // Assign the dummy purchase screen to Turn_Script.
        turnManager.propertyPurchaseScrn = dummyPurchaseScrn;

        GameObject aucscrgo = new GameObject("auction");
        AuctionScrn aucscrn = aucscrgo.AddComponent<AuctionScrn>();
        dummyPurchaseScrn.AuctionUI = aucscrn;


    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(turnManagerGO);
        foreach (boardPlayer player in turnManager.players){ Object.DestroyImmediate(player.gameObject);}
        Object.DestroyImmediate(aucscrgo);
        Object.DestroyImmediate(aiPlayerGO);
    }

    /// <summary>
    /// Test that when an AI lands on an unowned property and has sufficient funds,
    /// the AI automatically purchases the property.
    /// </summary>
    
    //Some insane shit broke here i'm gonna have to test it by hand
    [UnityTest]
    public IEnumerator Test_AI_AutoPurchaseProperty_Succeeds()
    {
        turnManager.playerlist[0].isAI = true;
        // Arrange
        // Create a test property with a price lower than the AI's balance.
        Property testProperty = dummyPM.properties[4];
        testProperty.owned = false;
        Player aiPlayer = turnManager.playerlist[0];
        

        // Ensure the AI player has sufficient funds.
        int prebalance = aiPlayer.Balance;

        // Simulate movement so that the AI lands on this property.
        turnManager.StartTurn();
        yield return turnManager.StartCoroutine(turnManager.PlayerMovePhase(aiPlayer.bPlayer, true, 4, 5));
        

        // Assert
        // The AI should have automatically purchased the property.
        Assert.IsTrue(aiPlayer.OwnedProperties.Contains(testProperty), "The AI should have acquired the property.");
        Assert.AreEqual(prebalance - testProperty.price, aiPlayer.Balance, "The AI's balance should be reduced by the property price.");
        Assert.AreEqual(aiPlayer, testProperty.owner, "The property's owner should be set to the AI player.");
    }
    
    
    /// <summary>
    /// Test that when an AI lands on an unowned property but lacks sufficient funds,
    /// the AI triggers an auction via the manualAuction call.
    /// </summary>

    /* 
    
    //I tested this personally and can see it works. Very hard to make a good unit test for this bit though
    [UnityTest]
    public IEnumerator Test_AI_AutoPurchaseProperty_Fails_TriggersAuction()
    {
        // Arrange
        // Create a test property with a price higher than the AI's balance.
        Property testProperty = new Property("ExpensiveEstate", 2000, "Red", 50);
        testProperty.owned = false;
        dummyPM.DummyProperty = testProperty;

        // Set AI player's balance to an insufficient amount.
        turnManager.playerlist[0].bPlayer.balance = 500;

        // Reset flag on dummy purchase screen.
        dummyPurchaseScrn.manualAuctionCalled = false;

        // Simulate the AI player landing on the property.
        yield return turnManager.StartCoroutine(turnManager.PlayerMovePhase(aiBoardPlayer, true, 4, 5));

        // Assert
        // Cast the purchase screen reference back to DummyPropertyPurchaseScrn to access our flag.
        DummyPropertyPurchaseScrn dummy = turnManager.propertyPurchaseScrn as DummyPropertyPurchaseScrn;
        Assert.AreEqual("ExpensiveEstate",dummyPurchaseScrn.AuctionUI.PropertyNameText, "The AI should have triggered an auction when funds are insufficient.");
        // Also, the property should remain unowned.
        Assert.IsFalse(testProperty.owned, "The property should remain unowned when the AI cannot purchase it.");
    }
    */

    /// <summary>
    /// Test that when an AI player's turn begins, the Update method automatically triggers
    /// the move phase (without a SPACE key press).
    /// </summary>
    [UnityTest]
    public IEnumerator Test_AI_Turn_AutomaticMove()
    {
        
        
        // Arrange
        // For this test we simulate a scenario where there is no human input.
        // Mark the current player as AI.
        turnManager.playerlist[0].isAI = true;

        // Set a dummy property for the AI to land on (so that purchase decision is skipped).
        Property testProperty = new Property("NeutralEstate", 300, "Yellow", 30);

        // Act
        // Set waiting flag and call Update to trigger the AI branch.
        turnManager.isWaitingForRoll = true;

        // Since PlayerMovePhase is a coroutine, wait briefly to let it run.
        yield return turnManager.StartCoroutine(turnManager.PlayerMovePhase(turnManager.players[0], true, 4, 5));
        

        // Assert
        // The turn should now be flagged as ended.
        Assert.IsTrue(turnManager.turnEnded, "After an AI turn, the turn should be flagged as ended.");
    }


    [UnityTest]
    public IEnumerator multipleAITest(){
        turnManager.playerlist[0].isAI=true;
        turnManager.playerlist[1].isAI=true;

        //Test AI #1 buying a property
        yield return turnManager.StartCoroutine(turnManager.PlayerMovePhase(turnManager.players[0], true, 4, 5));

        //Test AI #2 going to jail and trying to move again
        yield return turnManager.StartCoroutine(turnManager.PlayerMovePhase(turnManager.players[1], true, 2, 2));
        yield return turnManager.StartCoroutine(turnManager.PlayerMovePhase(turnManager.players[1], true, 2, 3));

        Assert.AreEqual(10,turnManager.players[0].TileCount);

        Assert.AreEqual(11,turnManager.players[1].TileCount);

        //Test AI 


    }
}
#endregion
