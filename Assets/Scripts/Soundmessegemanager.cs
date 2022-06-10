using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundmessegemanager : MonoBehaviour
{
    [SerializeField] private AudioClip[] messeges;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] circlemesseges;

    private int soundIndex=0;

    private void Awake()
    {
    }
    public void PlayMessege()
    {
        if (soundIndex < messeges.Length) 
        {
            audioSource.clip = messeges[soundIndex];
            audioSource.Play();
            soundIndex += 1;
        }
        else
        {
            if (circlemesseges.Length > 0)
            {
                audioSource.clip = circlemesseges[Random.Range(0, circlemesseges.Length)];
                audioSource.Play();
            }

        }
    }
}
