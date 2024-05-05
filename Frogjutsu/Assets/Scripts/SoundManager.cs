using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;

    private void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);  // Destroy the new instance if one already exists
            return;
        }

        DontDestroyOnLoad(gameObject);  // Persist this manager across scenes
        instance = this;
        source = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip _sound) {
        source.PlayOneShot(_sound);
    }
}
