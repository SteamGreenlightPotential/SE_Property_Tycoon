using System.Collections;
using System.Collections.Generic;
using PropertyTycoon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gModeManager : MonoBehaviour
{
    public static gModeManager Instance;
    public Text timerText; // Reference to the Timer UI element

    private string currentGameMode = "Normal Mode"; // normal Mode as default
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[gModeManager] OnSceneLoaded: " + scene.name);
        ReassignTimerText();
        InitializeGameMode();
    }

    public void SetGameMode(string gameMode)
    {
        currentGameMode = gameMode.Trim();
        Debug.Log("[gModeManager] Game Mode Set to: '" + currentGameMode + "'");
    }

    public void InitializeGameMode()
    {
        Debug.Log("[gModeManager] Initializing game mode: '" + currentGameMode + "'");
        if (currentGameMode == "Quick Game")
        {
            if (timerText != null)
            {
                timerText.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("[gModeManager] TimerText is null in InitializeGameMode.");
            }
            Debug.Log("[gModeManager] Quick Game Mode Initialization... starting timer.");
            StartQuickGame();
        }
        else
        {
            Debug.Log("[gModeManager] Normal Mode started.");
            if (timerText != null)
            {
                timerText.gameObject.SetActive(false);
            }
        }
    }

    public void StartQuickGame()
    {
        Debug.Log("[gModeManager] Quick Game Mode starting! Launching timer...");
        StartCoroutine(QuickGameTimer());
    }

    private IEnumerator QuickGameTimer()
    {
        Debug.Log("[gModeManager] QuickGameTimer coroutine started.");
        float quickGameDuration = 15 * 60f; // 15 minutes in seconds
        float timeRemaining = quickGameDuration;

        while (timeRemaining > 0)
        {
            yield return new WaitForSecondsRealtime(1f); // Real-time wait
            timeRemaining--;

            if (timerText != null)
            {
                timerText.text = $"Time Remaining: {Mathf.FloorToInt(timeRemaining / 60)}:{Mathf.FloorToInt(timeRemaining % 60):00}";
            }
            else
            {
                Debug.LogError("[gModeManager] TimerText reference is null during update!");
            }
        }

        Debug.Log("[gModeManager] Quick Game Over!");
        EndQuickGame();
    }

    private void EndQuickGame()
    {
        Debug.Log("[gModeManager] Quick Game Over! Ending game...");
        Player winner = FindWinner();
        if (winner != null)
        {
            Debug.Log($"[gModeManager] Quick Game Winner: {winner.Name} with £{winner.Balance}");
        }
        else
        {
            Debug.LogError("[gModeManager] No players found.");
        }
    }

    private Player FindWinner()
    {
        List<Player> players = GameManager.Instance.players; // Fetch players from GameManager
        if (players == null || players.Count == 0)
        {
            Debug.LogError("[gModeManager] No players found!");
            return null;
        }

        Player richestPlayer = players[0];
        foreach (Player player in players)
        {
            if (player.Balance > richestPlayer.Balance)
            {
                richestPlayer = player;
            }
        }
        return richestPlayer;
    }

    public void ReassignTimerText()
    {
        Debug.Log("[gModeManager] Attempting to reassign TimerText...");
        GameObject timerObject = GameObject.Find("TimerText");
        if (timerObject == null)
        {
            Debug.LogError("[gModeManager] TimerText GameObject not found in the current scene!");
            return;
        }

        timerText = timerObject.GetComponent<Text>();
        if (timerText == null)
        {
            Debug.LogError("[gModeManager] Text component not found on TimerText GameObject!");
        }
        else
        {
            Debug.Log("[gModeManager] TimerText successfully reassigned: '" + timerText.text + "'");
        }
    }
}