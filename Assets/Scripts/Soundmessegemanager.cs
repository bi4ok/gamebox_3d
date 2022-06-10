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
        print(soundIndex + "  " + messeges.Length + " sound index");
        if (soundIndex < messeges.Length) 
        {
            audioSource.clip = messeges[soundIndex];
            audioSource.Play();
            soundIndex += 1;
            print(soundIndex + " увеличен на 1");
        }
        else
        {
            audioSource.clip = circlemesseges[Random.Range(0, circlemesseges.Length)];
            audioSource.Play();
        }
    }
}
