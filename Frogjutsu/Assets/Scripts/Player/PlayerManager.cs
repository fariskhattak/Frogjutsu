using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public static Vector2 startPosition = new Vector2(-4, 0);
    public GameObject[] playerPrefabs;
    private GameObject currentPlayer;
    public Stats playerStats;
    public int characterIndex;

    public HealthBar healthBar;

    private CameraController cameraController;

    // List of scenes where the player should not be instantiated
    private HashSet<string> nonPlayerScenes = new HashSet<string> { "MainMenu", "Level Selection", "CharacterSelect", "Game Over"};

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Destroy the new instance if one already exists
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // Persist this manager across scenes

        SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to the sceneLoaded event

        InitPlayerStats();  // Initialize player stats
        InstantiatePlayer();  // Instantiate the player character
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Unsubscribe when destroyed to avoid memory leaks
    }

    private void InitPlayerStats()
    {
        // Initialize the Stats object with default values or modify them as needed
        int baseMaxHealth = 100;
        int baseMaxMana = 50;
        float baseMoveSpeed = 7f;
        float baseDamage = 10f;
        float baseDefense = 5f;
        float baseJumpForce = 14f;
        playerStats = new Stats(baseMaxHealth, baseMaxMana, baseMoveSpeed, baseDamage, baseDefense, baseJumpForce);
    }

    private void InstantiatePlayer()
    {
        if (currentPlayer == null)
        {
            characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
            currentPlayer = Instantiate(playerPrefabs[characterIndex], startPosition, Quaternion.identity);
            // cameraController.player = currentPlayer.transform;
            InitCameraController(currentPlayer);
            playerStats.DeathReset();
            currentPlayer.GetComponent<Player>().playerStats = playerStats;
        }
    }

    private void InitCameraController(GameObject player)
    {
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject != null)
        {
            cameraController = cameraObject.GetComponent<CameraController>();
            if (cameraController != null)
            {
                cameraController.player = player.transform;
            }
            else
            {
                Debug.LogError("The CameraController component was not found on the object with tag 'MainCamera'.");
            }
        }
        else
        {
            Debug.LogError("An object with the tag 'MainCamera' was not found in the scene.");
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("New scene loaded: " + scene.name);
        if (!nonPlayerScenes.Contains(scene.name)) {
            InstantiatePlayer();
            Debug.Log("Instantiated player, Current Health is: " + playerStats.currentHealth);
        } else {
            Debug.Log("Player not instantiated, scene is not for gameplay: " + scene.name);
            EnsurePlayerIsDestroyed();
        }
    }

    private void EnsurePlayerIsDestroyed() {
        if (currentPlayer != null) {
            Destroy(currentPlayer);
            currentPlayer = null;
        }
    }

}
