using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private bool unlocked; //default value is false
    public Image unlockImage;

    public void Update(){ //TODO move this method later
        UpdateLevelImage();
    }
    
    public void UpdateLevelImage(){
        if (!unlocked){ //MARKER if unlock is false means this level is locked
            unlockImage.gameObject.SetActive(true);
        }
        else { //if unlock is true means this level can play
            unlockImage.gameObject.SetActive(false)
        }
    }

    public void PressSelection(string _LevelName) { //when press level, move to the corresponding scene
        if(unlocked) {
            SceneManager.LoadScene(_LevelName)
        }
    }
}
