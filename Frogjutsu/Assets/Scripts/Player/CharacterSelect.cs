using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] skins;
    private int selectedCharacter;

    [SerializeField] private TMP_Text className;
    [SerializeField] private TMP_Text description;

    String[] classes = { "Raphfrog", "Donafrog", "Leofrog", "Michafrog" };

    String[] classDescriptions = {
        "A stout warrior, wielding mighty weapons to crush foes",
        "A master of arcane arts, casting powerful spells to dominate battles",
        "A skilled archer, striking from afar with precision and agility",
        "A silent shadow, swiftly dispatching enemies with lethal precision"
    };

    private void Awake()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        foreach (GameObject player in skins)
        {
            player.SetActive(false);
        }
        skins[selectedCharacter].SetActive(true);
        className.text = classes[selectedCharacter];
        description.text = classDescriptions[selectedCharacter];
    }

    public void ChangeNext()
    {
        skins[selectedCharacter].SetActive(false);
        selectedCharacter++;
        if (selectedCharacter == skins.Length)
        {
            selectedCharacter = 0;
        }

        skins[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        className.text = classes[selectedCharacter];
        description.text = classDescriptions[selectedCharacter];
    }

    public void ChangeBack()
    {
        skins[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter = skins.Length - 1;
        }

        skins[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        className.text = classes[selectedCharacter];
        description.text = classDescriptions[selectedCharacter];
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        SceneManager.LoadSceneAsync("Level 1");
    }
}
