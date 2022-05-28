using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayClipOnTrigger : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource audioSource;

    void OnTriggerEnter()//Collider other)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    void OnValidate()
    {
        if(!audioSource)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }
    }
}
