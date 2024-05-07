using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject infoPanel;
    public GameObject noButton;
    public GameObject playButton;

    void Start()
    {
        infoPanel.SetActive(false);
    }

    void Update()
    {
        // Detect axis input from the controller left joystick
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // If joystick moves and no button is selected
        if ((Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f) && EventSystem.current.currentSelectedGameObject == null)
        {
            // Select the play button
            EventSystem.current.SetSelectedGameObject(playButton);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("CharacterSelect");
    }

    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(noButton);
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
