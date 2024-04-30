using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundsPlayer : MonoBehaviour
{
    public AudioSource ButtonSounds;
    public AudioClip vineBoom;

    public void Button1() {
        ButtonSounds.clip = vineBoom;
        ButtonSounds.Play();
    }

    public void Button2() {
        ButtonSounds.clip = vineBoom;
        ButtonSounds.Play();
    }

    public void Button3() {
        ButtonSounds.clip = vineBoom;
        ButtonSounds.Play();
    }

}
