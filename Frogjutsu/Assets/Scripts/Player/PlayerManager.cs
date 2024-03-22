using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Vector2 startPosition = new Vector2(-4, 0);
    public GameObject[] playerPrefabs;
    public int characterIndex;

    public HealthBar healthBar;

    [SerializeField] private CameraController cameraController;

    private void Awake()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject player = Instantiate(playerPrefabs[characterIndex], startPosition, Quaternion.identity);
        cameraController.player = player.transform;
    }

}
