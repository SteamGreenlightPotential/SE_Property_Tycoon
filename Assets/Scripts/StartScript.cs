using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes

public class StartMenu : MonoBehaviour
{
    // Function to start the game
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Function to quit the game
    public void QuitGame()
    {
        Debug.Log("Quit Game"); // Logs to the Console (useful for testing)
        Application.Quit(); // Quits the application (works in builds, not in the Editor)
    }
}