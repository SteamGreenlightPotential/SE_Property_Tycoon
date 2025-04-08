using UnityEngine;
using UnityEngine.UI; // For using Dropdown

public class PlayerSelection : MonoBehaviour
{
    public Dropdown playerDropdown; // Reference to the Dropdown
    public static int numberOfPlayers; // Store the selected number of players
    public static int aiCount;

    void Start()
    {
        // Set a default value, e.g., 2 players
        numberOfPlayers = 2;

        // Add listener to detect when dropdown value changes
        playerDropdown.onValueChanged.AddListener(DropdownValueChanged);
    }

    void DropdownValueChanged(int value)
    {
        numberOfPlayers = value + 2; // Dropdown index starts from 0, so +2 for players
        Debug.Log("Number of Players: " + numberOfPlayers);
    }

    void aiValue(int value){
        aiCount=value;
    }
}