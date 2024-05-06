using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject gameOverPanel;
    public GameObject noButton;

    // Variable to track if the video has finished playing
    private bool videoFinished = false;

    void Start()
    {
        // Initialize the game over panel to be inactive
        gameOverPanel.SetActive(false);

        // Subscribe to the videoPlayer's loopPointReached event
        videoPlayer.loopPointReached += OnVideoFinished;

        // Start playing the video
        videoPlayer.Play();
    }

    void Update()
    {
        // Check if the video has finished playing
        if (videoFinished)
        {
            // If the video has finished, pause the video player
            videoPlayer.Pause();
        }
    }

    // Function called when the video finishes playing
    void OnVideoFinished(VideoPlayer vp)
    {
        // Set the videoFinished flag to true
        videoFinished = true;

        // Show the game over panel
        ShowGameOverPanel();
    }

    // Function to show the game over panel
    void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(noButton);
        }
        else
        {
            Debug.LogError("Game over panel is not assigned.");
        }
    }

    // Function to play again
    public void PlayAgain()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    // Function to quit the game
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
