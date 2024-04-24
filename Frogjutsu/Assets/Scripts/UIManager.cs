using UnityEngine;
using UnityEngine.UI; // Required for UI components
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public Image characterDisplayUI; // Assign this in the inspector with your UI Image
    public Sprite[] characterSprites; // Assign the character sprites in the inspector
    public Color[] characterColors;
    public Image characterProfileBorder;

    public static bool paused = false;
    public GameObject pauseMenuUI;
    public GameObject playButton;

    public GameObject exitMenuUI;
    public GameObject noButton;
    PlayerManager playerManager;
    Player player;

    void Start () {
        pauseMenuUI.SetActive(false);
        exitMenuUI.SetActive(false);
        playerManager = FindObjectOfType<PlayerManager>();
        characterDisplayUI.sprite = characterSprites[playerManager.characterIndex];
        characterProfileBorder.color = characterColors[playerManager.characterIndex];
        characterDisplayUI.enabled = true; // Make sure the image is enabled

        player = FindObjectOfType<Player>();
    }

    void Update () {
        if (Input.GetButtonDown("Pause")) {
            if (paused)
            {
                Resume();
            } else 
            {
                Pause();
            }
        }
    }

    public void Resume () 
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        player.GetComponent<PlayerMovement>().enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(playButton);
        Time.timeScale = 0f;
        paused = true;
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    public void ExitNo()
    {
        exitMenuUI.SetActive(false);
        Pause();
    }

    public void ExitYes()
    {
        Time.timeScale = 1f;
        paused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowMainMenuExitUI()
    {
        pauseMenuUI.SetActive(false);
        exitMenuUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(noButton);
    }
}
