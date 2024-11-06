using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    public GameObject WinSound;
    public AudioSource soundEffect;

    private void Start()
    {
        WinSound.SetActive(false);
    }

    public void PlayWinSound()
    {
        WinSound.SetActive(true);
        soundEffect.Play();
    }
}
