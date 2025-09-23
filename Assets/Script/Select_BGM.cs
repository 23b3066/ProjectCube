using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Select_BGM : MonoBehaviour
{

    public AudioClip sound1;
    AudioSource audioSource;
    public float targetVolume = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = targetVolume;
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void SE()
    {
        audioSource.PlayOneShot(sound1);
    }
}
