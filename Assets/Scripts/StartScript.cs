using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Dropdown gameModeDropdown; // Dropdown for selecting the game mode

    public void PlayGame()
    {
        string selectedGameMode = gameModeDropdown.options[gameModeDropdown.value].text;
        gModeManager.Instance.SetGameMode(selectedGameMode);
        Debug.Log("[StartMenu] Selected Game Mode: " + selectedGameMode);
        SceneManager.LoadScene("SampleScene");
    }

    public void OnGameModeChanged(int value)
    {
        string selectedGameMode = gameModeDropdown.options[value].text;
        gModeManager.Instance.SetGameMode(selectedGameMode);
        Debug.Log("[StartMenu] Game Mode Updated to: " + selectedGameMode);
    }

    public void QuitGame()
    {
        Debug.Log("[StartMenu] Quit Game");
        Application.Quit();
    }
}