using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseUI : MonoBehaviour
{
    public GameObject LoseSound;
    public AudioSource soundEffect;

    private void Start()
    {
        LoseSound.SetActive(false);
    }

    public void PlayLoseSound()
    {
        LoseSound.SetActive(true);
        soundEffect.Play();
    }
}
