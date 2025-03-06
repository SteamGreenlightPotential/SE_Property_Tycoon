using UnityEngine;
using System.Collections;

public class End_Turn_Script : MonoBehaviour
{
    public Turn_Script turnScript; // Reference to Turn_Script

    private void OnMouseUpAsButton()
    {
        StartCoroutine(EndTurnSequence()); // Start the coroutine
    }

    IEnumerator EndTurnSequence()
    {
        turnScript.EndTurnButtonClicked(); // Call the method in Turn_Script
        transform.localScale = new Vector3(1f, 1f, 0.05f); // Make button thinner
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        transform.localScale = new Vector3(1f, 1f, 0.1f); // Restore button size
    }
}