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
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event
        InitializeUI();
    }

    void Update () {
        if (IsLevelScene() && Input.GetButtonDown("Pause"))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void InitializeUI()
    {
        playerManager = PlayerManager.Instance;
        player = FindAnyObjectByType<Player>();

        pauseMenuUI.SetActive(false);
        exitMenuUI.SetActive(false);
        statsMenuUI.SetActive(false);
        
        characterDisplayUI.sprite = characterSprites[playerManager.characterIndex];
        characterProfileBorder.color = characterColors[playerManager.characterIndex];
        characterDisplayUI.enabled = true;
        characterStatsProfile.sprite = characterSprites[playerManager.characterIndex];
        characterStatsProfileBorder.color = characterColors[playerManager.characterIndex];
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsLevelScene()) {
            InitializeUI();
        }
    }

    public void Resume () 
    {
        pauseMenuUI.SetActive(false);
        statsMenuUI.SetActive(false);
        exitMenuUI.SetActive(false);
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

    bool IsLevelScene()
    {
        // Define the names of your level scenes here
        string[] levelScenes = { "Level 1", "Level 2", "Level 3", "Level 4" }; // Add your level scene names here

        // Get the current scene name
        string currentScene = SceneManager.GetActiveScene().name;

        // Check if the current scene is a level scene
        foreach (string levelScene in levelScenes)
        {
            if (currentScene.Equals(levelScene))
            {
                return true;
            }
        }
        return false;
    }
}
