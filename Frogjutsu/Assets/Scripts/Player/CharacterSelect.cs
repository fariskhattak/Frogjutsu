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

    String[] classes = { "warrior", "mage", "ranger", "assassin" };

    private void Awake()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        foreach (GameObject player in skins)
        {
            player.SetActive(false);
        }
        skins[selectedCharacter].SetActive(true);
        className.text = classes[selectedCharacter];
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
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        SceneManager.LoadSceneAsync("Level 1");
    }
}
