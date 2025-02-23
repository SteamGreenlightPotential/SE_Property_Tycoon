using UnityEngine;
using System.Collections;

public class Turn_Script : MonoBehaviour
{
    public Grid_Movement[] players; // Assigned the scripts from each piece in the Inspector
    private int currentPlayerIndex = 0;
    private bool isWaitingForRoll = true; // Wait for player to press space to roll

    void Start()
    {
        Debug.Log("Game Start!");
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
        Debug.Log("Player " + (currentPlayerIndex + 1) + "'s Turn. Press SPACE to roll.");
        isWaitingForRoll = true; // Wait for player to press space before rolling
    }

    IEnumerator PlayerMovePhase(Grid_Movement player)
    {
        // Wait for player to press space before rolling
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        // Roll the dice
        int roll = Random.Range(1, 7); // Roll dice for movement
        Debug.Log("Player " + (currentPlayerIndex + 1) + " rolled: " + roll);

        player.Move(roll); // Move the player
        yield return new WaitForSeconds(roll * 0.2f + 0.5f); // Wait for movement to finish

        Debug.Log("Press Space to Skip (THIS IS FOR THE BUY PHASE LATER)");

        // Wait for the player to press space to continue
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        // Begin the EndTurn phase
        StartCoroutine(EndTurn());
    }

    IEnumerator EndTurn()
    {
        Debug.Log("Ending Player " + (currentPlayerIndex + 1) + "'s Turn...");
        yield return new WaitForSeconds(0.5f); // Wait before moving to the next turn

        // Move to the next player
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

        // Start the next player's turn
        StartTurn();
    }
}
