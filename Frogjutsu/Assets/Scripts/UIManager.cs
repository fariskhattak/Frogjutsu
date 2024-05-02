using UnityEngine;
using UnityEngine.UI; // Required for UI components
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;


public class UIManager : MonoBehaviour
{
    public Image characterDisplayUI; // Assign this in the inspector with your UI Image
    public Sprite[] characterSprites; // Assign the character sprites in the inspector
    public Color[] characterColors;
    public Image characterProfileBorder;
    public TMP_Text[] statsNumbers;

    public Image characterStatsProfile;
    public Image characterStatsProfileBorder;

    public static bool paused = false;
    public GameObject pauseMenuUI;
    public GameObject playButton;

    public GameObject exitMenuUI;
    public GameObject noButton;
    
    public GameObject statsMenuUI;
    public GameObject backButton;
    PlayerManager playerManager;
    Player player;

    void Start () {
        pauseMenuUI.SetActive(false);
        exitMenuUI.SetActive(false);
        statsMenuUI.SetActive(false);
        playerManager = PlayerManager.Instance;
        characterDisplayUI.sprite = characterSprites[playerManager.characterIndex];
        characterProfileBorder.color = characterColors[playerManager.characterIndex];
        characterDisplayUI.enabled = true; // Make sure the image is enabled

        characterStatsProfile.sprite = characterSprites[playerManager.characterIndex];
        characterStatsProfileBorder.color = characterColors[playerManager.characterIndex];

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
        statsMenuUI.SetActive(false);
        exitMenuUI.SetActive(false);
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
        player.playerStats.DeathReset();
        SceneManager.LoadScene("MainMenu");
        PlayerManager.Instance.playerStats = new Stats();
    }

    public void ShowMainMenuExitUI()
    {
        pauseMenuUI.SetActive(false);
        exitMenuUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(noButton);
    }

    public void OpenStatsMenu()
    {
        pauseMenuUI.SetActive(false);
        statsMenuUI.SetActive(true);
        statsNumbers[0].text = "" + (int) playerManager.playerStats.maxHealth;
        statsNumbers[1].text = "" + (int) playerManager.playerStats.damage;
        statsNumbers[2].text = "" + (int) playerManager.playerStats.defense;
        statsNumbers[3].text = "" + (int) playerManager.playerStats.moveSpeed;
        statsNumbers[4].text = "" + (int) playerManager.playerStats.maxMana;
        EventSystem.current.SetSelectedGameObject(backButton);
    }
}
