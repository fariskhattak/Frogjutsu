using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class LevelSelection : MonoBehaviour
{
    public bool[] unlocked; // Default value is false
    [SerializeField] private GameObject[] buttons;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to the sceneLoaded event
        unlocked[0] = true; // Unlock the first level by default

        UpdateButtonNavigation(); // Initialize navigation setup
    }

    public void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            unlocked[i] = i < PlayerManager.Instance.playerStats.unlockedLevels;
            Button button = buttons[i].GetComponent<Button>();
            button.interactable = unlocked[i];
        }

        UpdateButtonNavigation(); // Update navigation dynamically
    }

    void UpdateButtonNavigation()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i].GetComponent<Button>();
            Navigation navigation = button.navigation;

            // Check the previous button's state
            if (i > 0 && !unlocked[i - 1])
            {
                navigation.selectOnLeft = null; // Disable navigation from previous button if not unlocked
            }
            else
            {
                navigation.selectOnLeft = i > 0 ? buttons[i - 1].GetComponent<Selectable>() : null;
            }

            // Check the next button's state
            if (i < buttons.Length - 1 && !unlocked[i + 1])
            {
                navigation.selectOnRight = null; // Disable navigation to next button if not unlocked
            }
            else
            {
                navigation.selectOnRight = i < buttons.Length - 1 ? buttons[i + 1].GetComponent<Selectable>() : null;
            }

            button.navigation = navigation;
        }
    }

    public void PressSelection(int _levelNumber)
    { // When press level, move to the corresponding scene
        if (unlocked[_levelNumber - 1])
        {
            SceneManager.LoadScene("Level " + _levelNumber);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventSystem.current.SetSelectedGameObject(buttons[0]);
    }
}
