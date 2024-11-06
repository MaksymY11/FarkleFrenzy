using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        Debug.Log("Music Playing");
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void MuteToggle()
    {
        audioSource.mute = !audioSource.mute; 
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}


