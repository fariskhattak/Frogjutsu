using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class UIManager : MonoBehaviour
{
    public Image characterDisplayUI; // Assign this in the inspector with your UI Image
    public Sprite[] characterSprites; // Assign the character sprites in the inspector

    PlayerManager playerManager;

    void Start () {
        playerManager = FindObjectOfType<PlayerManager>();
        characterDisplayUI.sprite = characterSprites[playerManager.characterIndex];
        characterDisplayUI.enabled = true; // Make sure the image is enabled
    }

    // // Call this method when the player selects a character
    // public void SelectCharacter(int characterIndex)
    // {
    //     if(characterIndex >= 0 && characterIndex < characterSprites.Length)
    //     {
    //         // Set the UI Image to display the selected character's sprite
    //         characterDisplayUI.sprite = characterSprites[characterIndex];
    //         characterDisplayUI.enabled = true; // Make sure the image is enabled
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Character index out of range: " + characterIndex);
    //     }
    // }
}
