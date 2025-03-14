using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign GameOverPanel in Inspector
    public TextMeshProUGUI winnerText; // Assign WinnerText in Inspector

    private static GameOverManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        gameOverPanel.SetActive(false); // Hide Game Over Screen initially
    }

    public static void ShowGameOver(string winner)
    {
        instance.gameOverPanel.SetActive(true);
        instance.winnerText.text = winner;
        instance.DisablePlayerMovement(); //  Stop players from moving
    }

    public void RestartGame()
    {
        gameOverPanel.SetActive(false); // Hide Game Over Screen before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the game scene
    }

    void DisablePlayerMovement()
    {
        MultiplayerMovement[] players = FindObjectsOfType<MultiplayerMovement>(); //  Find all players
        foreach (MultiplayerMovement player in players)
        {
            player.enabled = false; //  Disable movement script
        }
    }
}
