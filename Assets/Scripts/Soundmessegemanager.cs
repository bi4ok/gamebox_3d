using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundmessegemanager : MonoBehaviour
{
    [SerializeField] private AudioClip[] messeges;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] circlemesseges;
    public void PlayMessege()
    {
        print("fffffffffff");
        int i = 0;
        if (i < messeges.Length) 
        {
            audioSource.clip = messeges[i];
            audioSource.Play();
        }
        else
        {
            audioSource.clip = circlemesseges[Random.Range(0, circlemesseges.Length)];
            audioSource.Play();
        }
    }
}
